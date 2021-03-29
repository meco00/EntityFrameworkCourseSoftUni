namespace VaporStore.DataProcessor
{
	using System;
    using System.Globalization;
    using System.Linq;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using VaporStore.DataProcessor.Dto;

    public static class Serializer
	{
		public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
		{
			var genres = context.Genres
				
				.Where(x => genreNames.Contains(x.Name))
				.ToList()
				.Select(x => new
				{
					Id = x.Id,
					Genre = x.Name,
					Games = x.Games
					.Where(g => g.Purchases.Any())
					.Select(g => new
					{
						Id = g.Id,
						Title = g.Name,
						Developer = g.Developer.Name,
						Tags = String.Join(", ", g.GameTags.Select(x => x.Tag.Name)),
						Players = g.Purchases.Count

					})
					.OrderByDescending(g=>g.Players)
					.ThenBy(g=>g.Id)
					.ToList(),

					TotalPlayers = x.Games.Sum(x=>x.Purchases.Count)



				}
					)
				.OrderByDescending(x=>x.TotalPlayers)
				.ThenBy(x=>x.Id)
				.ToList();

			

			var json = JsonConvert.SerializeObject(genres,Formatting.Indented);

			return json;

		}

		public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
		{
			var userPurchasesByType = context.Users
				.Include(x=>x.Cards)
				.ThenInclude(x=>x.Purchases)
				.ThenInclude(x=>x.Game)
				.ToList()
				.Select(x => new UserExportModel
			   {
				Username = x.Username,
				
				Purchases = x.Cards
		       .SelectMany(x => x.Purchases)
				.Where(x=>x.Type.ToString()==storeType)
				.Select(x => new PurchaseExportModel
				{
					CardNumber = x.Card.Number,
					CvC = x.Card.Cvc,
					Date = x.Date.ToString("yyyy-MM-dd HH:mm",CultureInfo.InvariantCulture),
					Game = new GameExportModel
					{
						Title = x.Game.Name,
						GenreName = x.Game.Genre.Name,
						Price = x.Game.Price
					},



				})
				.OrderBy(x=>x.Date)
			    .ToArray(),

				  TotalSpent=x.Cards
				  .Sum(x=>x.Purchases.Where(x=>x.Type.ToString()==storeType)
				  .Sum(x=>x.Game.Price))

			})
				.Where(x=>x.Purchases.Any())
				.OrderByDescending(x=>x.TotalSpent)
				.ThenBy(x=>x.Username)
				.ToArray();

			;

			var xml = XmlConverter.Serialize<UserExportModel[]>(userPurchasesByType, "Users");

			return xml;
		}
	}
}