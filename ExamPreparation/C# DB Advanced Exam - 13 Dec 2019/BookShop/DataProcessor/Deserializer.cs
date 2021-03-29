namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using BookShop.Data.Models;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(BookInputModel[]), new XmlRootAttribute("Books"));

            var stringReader = new StringReader(xmlString);

            var booksToImport = new List<Book>();

            using (stringReader)
            {
            var booksInputModel = (BookInputModel[])serializer.Deserialize(stringReader);

                foreach (var book in booksInputModel)
                {
                    
                    if (!IsValid(book))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime date;

                    var IsDateValid = DateTime
                        .TryParseExact(book.PublishedOn,
                        "MM/dd/yyyy",
                        CultureInfo.InvariantCulture,DateTimeStyles.None, out date);

                    if (!IsDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var currentBook = new Book
                    {
                        Name = book.Name,
                        Genre = Enum.Parse<Genre>(book.Genre),
                        Price = book.Price,
                        Pages = book.Pages,
                        PublishedOn = date
                    };

                    booksToImport.Add(currentBook);

                    sb.AppendLine(string.Format(SuccessfullyImportedBook,currentBook.Name,currentBook.Price));

                }

            }

            ;
            context.Books.AddRange(booksToImport);

            context.SaveChanges();

            return sb.ToString().TrimEnd();




        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            var authorsInputModel = JsonConvert
                .DeserializeObject<IEnumerable<AuthorInputModel>>(jsonString);

            var sb = new StringBuilder();

            var emails = context.Authors.Select(x => x.Email).ToList();

            var contextBooksIds = context.Books.Select(x => x.Id).ToList();

            var authorsToBeImported = new List<Author>();

            foreach (var authorDTO in authorsInputModel)
            {
                
                if (!IsValid(authorDTO))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;

                }

                var isEmailExists = emails.Contains(authorDTO.Email);

                if (isEmailExists)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;

                }

                var booksIdsToAdd = authorDTO.Books
                  .Where(x => x.Id.HasValue && contextBooksIds.Contains(x.Id.Value))
                  .Select(x=>x.Id.Value)
                  .ToList();

                if (!booksIdsToAdd.Any())
                {
                    sb.AppendLine(ErrorMessage);
                    continue;

                }

                var authorToImport = new Author
                {
                    FirstName = authorDTO.FirstName,
                    LastName = authorDTO.LastName,
                    Phone = authorDTO.Phone,
                    Email = authorDTO.Email,
                    AuthorsBooks = booksIdsToAdd.Select(x => new AuthorBook
                    {
                        BookId = x
                    })
                    .ToList()


                };

                emails.Add(authorToImport.Email);

                authorsToBeImported.Add(authorToImport);

                sb.AppendLine($"Successfully imported author - {authorToImport.FirstName+" "+authorToImport.LastName} with {authorToImport.AuthorsBooks.Count} books.");

            }

           
            context.Authors.AddRange(authorsToBeImported);

            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}