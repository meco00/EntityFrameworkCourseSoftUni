namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto;
    using VaporStore.DataProcessor.Dto.Import;

    public static class Deserializer
    {
        private const string ERROR_MESSAGE = "Invalid Data";

        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var gamesInputModel = JsonConvert.DeserializeObject<IEnumerable<GameInputModel>>(jsonString);




            foreach (var game in gamesInputModel)
            {


                if (!IsValid(game) || game.Tags.Length == 0)
                {
                    sb.AppendLine(ERROR_MESSAGE);
                    continue;
                }

                DateTime dateTime;

                var isReleased = DateTime
                    .TryParseExact(game.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out dateTime);

                if (!isReleased)
                {
                    sb.AppendLine(ERROR_MESSAGE);
                    continue;
                }


                var developer = context.Developers.FirstOrDefault(x => x.Name == game.Developer) ?? new Developer
                {
                    Name = game.Developer
                };



                var genre = context.Genres.FirstOrDefault(x => x.Name == game.Genre) ?? new Genre
                {
                    Name = game.Genre
                };





                var currentGame = new Game
                {
                    Name = game.Name,
                    Price = game.Price,
                    ReleaseDate = dateTime
                };

                currentGame.Developer = developer;

                currentGame.Genre = genre;



                foreach (var tag in game.Tags)
                {
                    var currentTag = context.Tags.FirstOrDefault(x => x.Name == tag) ?? new Tag
                    {
                        Name = tag
                    };



                    currentGame.GameTags.Add(new GameTag
                    {
                        Game = currentGame,
                        Tag = currentTag
                    });





                }

                sb.AppendLine($"Added {currentGame.Name} ({currentGame.Genre.Name}) with {currentGame.GameTags.Count} tags");

                context.Games.Add(currentGame);
                context.SaveChanges();

            }

            return sb.ToString().Trim();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var users = JsonConvert.DeserializeObject<IEnumerable<UserAndCardInputModel>>(jsonString);

            var usersToImportInDB = new List<User>();

            foreach (var user in users)
            {
                if (!IsValid(user) || user.Cards.Any(x => !IsValid(x)))
                {
                    sb.AppendLine(ERROR_MESSAGE);
                    continue;

                }


                var userToImport = new User
                {
                    FullName = user.FullName,
                    Username = user.Username,
                    Email = user.Email,
                    Age = user.Age,
                    Cards = user.Cards.Select(x => new Card
                    {
                        Number = x.Number,
                        Cvc = x.Cvc,
                        Type = Enum.Parse<CardType>(x.Type)

                    })
                    .ToList()



                };

                usersToImportInDB.Add(userToImport);

                sb.AppendLine($"Imported {userToImport.Username} with {userToImport.Cards.Count} cards");
            }

            context.Users.AddRange(usersToImportInDB);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PurchaseInputModel[]), new XmlRootAttribute("Purchases"));

            using (StringReader reader = new StringReader(xmlString))
            {
                PurchaseInputModel[] purchasesDtos = (PurchaseInputModel[])xmlSerializer.Deserialize(reader);

                List<Purchase> purchaseToAdd = new List<Purchase>();

                foreach (var purchasesDto in purchasesDtos)
                {
                    if (!IsValid(purchasesDto))
                    {
                        sb.AppendLine(ERROR_MESSAGE);
                        continue;
                    }

                    DateTime date;
                    bool isDateValid = DateTime.TryParseExact(purchasesDto.Date, "dd/MM/yyyy HH:mm",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

                    if (!isDateValid)
                    {
                        sb.AppendLine(ERROR_MESSAGE);
                        continue;
                    }

                   

                    Card card = context.Cards.FirstOrDefault(c => c.Number == purchasesDto.Card);
                    if (card == null)
                    {
                        sb.AppendLine(ERROR_MESSAGE);
                        continue;
                    }

                    Game game = context.Games.FirstOrDefault(g => g.Name == purchasesDto.Title);
                    if (game == null)
                    {
                        sb.AppendLine(ERROR_MESSAGE);
                        continue;
                    }

                    Purchase purchase = new Purchase()
                    {
                        Card = card,
                        Date = date,
                        Game = game,
                        ProductKey = purchasesDto.Key,
                        Type = Enum.Parse<PurchaseType>(purchasesDto.Type)
                    };

                    purchaseToAdd.Add(purchase);
                    sb.AppendLine($"Imported {game.Name} for {card.User.Username}");
                }

                context.Purchases.AddRange(purchaseToAdd);
                context.SaveChanges();

                return sb.ToString().Trim();



            }


        }
        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }

    }
}
