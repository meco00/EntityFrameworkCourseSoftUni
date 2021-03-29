namespace BookShop.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ExportDto;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportMostCraziestAuthors(BookShopContext context)
        {
            var craziestAuthors = context.Authors
                .Select(x => new
                {
                    AuthorName = x.FirstName + " " + x.LastName,
                    Books = x.AuthorsBooks
                    .OrderByDescending(b => b.Book.Price)
                    .Select(b => new
                    {
                        BookName = b.Book.Name,
                        BookPrice = b.Book.Price.ToString("F2")
                    })
                    .ToList()

                })
                 .ToList()
                 .OrderByDescending(x => x.Books.Count())
                 .ThenBy(x => x.AuthorName)
                 .ToList();


            var json = JsonConvert.SerializeObject(craziestAuthors, Formatting.Indented);

            return json;
        }

        public static string ExportOldestBooks(BookShopContext context, DateTime date)
        {
            var serializer = 
                new XmlSerializer(typeof(OldestBookExportModel[]),new XmlRootAttribute("Books"));

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            var sb = new StringBuilder();


            var oldestBooks = context.Books
                .Where(x => x.PublishedOn < date&& x.Genre==Genre.Science)
                .ToList()
                 .OrderByDescending(x => x.Pages)
                .ThenByDescending(x => x.PublishedOn)
                .Select(x=> new OldestBookExportModel
                { 
                    Name=x.Name,
                    Pages=x.Pages,
                    Date=x.PublishedOn.ToString("d",CultureInfo.InvariantCulture)
                }
                )
                .Take(10)
                .ToArray();


             serializer.Serialize(new StringWriter(sb), oldestBooks,ns);

            return sb.ToString().TrimEnd();
              
        }
    }
}