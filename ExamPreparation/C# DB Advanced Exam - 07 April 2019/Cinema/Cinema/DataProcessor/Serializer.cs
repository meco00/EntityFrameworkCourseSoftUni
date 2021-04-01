namespace Cinema.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Cinema.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportTopMovies(CinemaContext context, int rating)
        {
            var topMovies = context.Movies
                .ToArray()
                .Where(x=>x.Rating>=rating && x.Projections.Any(p=>p.Tickets.Count()>=1))
                 .OrderByDescending(x => x.Rating)
                .ThenByDescending(x => x.Projections.Sum(pr => pr.Tickets.Sum(p => p.Price)))
                .Select(x => new
            {
                MovieName=x.Title,
                Rating=x.Rating.ToString("F2"),
                TotalIncomes=(x.Projections.Sum(pr=>pr.Tickets.Sum(p=>p.Price))).ToString("F2"),
                Customers=x.Projections.SelectMany(p=>p.Tickets).Select(c=> new
                {
                    FirstName= c.Customer.FirstName,
                    LastName=c.Customer.LastName,
                    Balance=c.Customer.Balance.ToString("f2")
                })
                .OrderByDescending(c=>c.Balance)
                .ThenBy(c=>c.FirstName)
                .ThenBy(c=>c.LastName)
                .ToList()

            }
                )
               
                .Take(10)
                .ToList();


            var json = JsonConvert.SerializeObject(topMovies, Formatting.Indented);

            return json;
        }





        public static string ExportTopCustomers(CinemaContext context, int age)
        {
            var customersOutputModel = context.Customers
                .ToArray()
                .Where(x=>x.Age >=  age)
                .OrderByDescending(x => (x.Tickets.Sum(t => t.Price)))
                
                 .Select(x => new CustomerExportXmlModel
                 {
                     FirstName = x.FirstName,
                     LastName = x.LastName,
                     SpentMoney = (x.Tickets.Sum(t => t.Price)).ToString("f2"),
                     SpentTime = TimeSpan.FromMilliseconds((x.Tickets.Sum(t => t.Projection.Movie.Duration.TotalMilliseconds)))
                     .ToString(@"hh\:mm\:ss")

                 })
                 .Take(10)

                 .ToArray();

            //.ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture)


            var serializer = new XmlSerializer(typeof(CustomerExportXmlModel[]), new XmlRootAttribute("Customers"));

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            var sb = new StringBuilder();

            serializer.Serialize(new StringWriter(sb), customersOutputModel, ns);

            return sb.ToString().TrimEnd();

        }
    }
}