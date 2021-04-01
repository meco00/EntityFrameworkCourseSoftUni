namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Cinema.Data.Models;
    using Cinema.Data.Models.Enums;
    using Cinema.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportMovie 
            = "Successfully imported {0} with genre {1} and rating {2:f2}!";
        private const string SuccessfulImportHallSeat 
            = "Successfully imported {0}({1}) with {2} seats!";
        private const string SuccessfulImportProjection 
            = "Successfully imported projection {0} on {1}!";
        private const string SuccessfulImportCustomerTicket 
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var moviesInputModel = 
                JsonConvert
                .DeserializeObject<IEnumerable<ImportMoviesJsonModel>>(jsonString);

            ;
            foreach (var movieDto in moviesInputModel)
            {
                if (!IsValid(movieDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var isMovieExist = context.Movies.Any(x=>x.Title==movieDto.Title);

                if (isMovieExist)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var currenMovie = new Movie
                {
                    Title=movieDto.Title,
                    Genre=Enum.Parse<Genre>(movieDto.Genre),
                    Duration=TimeSpan.Parse(movieDto.Duration),
                    Rating=movieDto.Rating,
                    Director=movieDto.Director
                };

                ;

                context.Movies.Add(currenMovie);

                context.SaveChanges();

                sb.AppendLine(
                    string
                    .Format(SuccessfulImportMovie,currenMovie.Title,currenMovie.Genre,currenMovie.Rating));

            }

            

            return sb.ToString().TrimEnd();
        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var hallSeatsInputModel = 
                JsonConvert
                .DeserializeObject<IEnumerable<ImportHallsSeatsJsonModel>>(jsonString);

            foreach (var hallInputDto in hallSeatsInputModel)
            {
                if (!IsValid(hallInputDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;

                }

                var currentHall = new Hall
                {
                    Name = hallInputDto.Name,
                    Is4Dx = hallInputDto.Is4Dx.Value,
                    Is3D = hallInputDto.Is3D.Value

                };

                for (int i = 0; i < hallInputDto.Seats; i++)
                {
                    currentHall.Seats.Add(new Seat
                    {
                        Hall = currentHall
                    });

                }

                var projectionType=string.Empty;

                if (currentHall.Is4Dx &&currentHall.Is3D)
                {
                    projectionType = "4Dx/3D";
                }
                else if (currentHall.Is4Dx)
                {
                    projectionType = "4Dx";
                }
                else if (currentHall.Is3D)
                {
                    projectionType = "3D";
                }
                else
                {
                    projectionType = "Normal";
                }



                context.Halls.Add(currentHall);

                context.SaveChanges();

                sb.AppendLine(
                string
                 .Format(SuccessfulImportHallSeat, currentHall.Name, projectionType, currentHall.Seats.Count()));



            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            var sb = new StringBuilder();

        var serializer = new XmlSerializer(typeof(ImportProjectionXmlModel[]), new XmlRootAttribute("Projections"));

            var projectionsInputModel = serializer.Deserialize(new StringReader(xmlString)) as ImportProjectionXmlModel[];

            foreach (var projectionDto in projectionsInputModel)
            {
                if (!IsValid(projectionDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;

                }

                var isDateValid = DateTime
                    .TryParseExact(projectionDto.DateTime,
                    "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date);

                if (!isDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }



                var movie = context.Movies.FirstOrDefault(x => x.Id == projectionDto.MovieId);

                if (movie==null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;

                }

                var hall = context.Halls.FirstOrDefault(x => x.Id == projectionDto.HallId);

                if (hall==null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;

                }



                var currentProjection = new Projection
                {
                    MovieId = projectionDto.MovieId,
                    HallId = projectionDto.HallId,
                    DateTime = date
                };

                context.Projections.Add(currentProjection);

                context.SaveChanges();

                sb.AppendLine(string.Format(SuccessfulImportProjection, movie.Title, currentProjection.DateTime.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));

            }

            return sb.ToString().TrimEnd();

          
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            var sb = new StringBuilder();

            ;

            var serializer = new XmlSerializer(typeof(ImportCustomerXmlModel[]), new XmlRootAttribute("Customers"));

            var customersInputModel = serializer.Deserialize(new StringReader(xmlString)) as ImportCustomerXmlModel[];

            foreach (var customerDto in customersInputModel)
            {
                if (!IsValid(customerDto)|| !customerDto.Tickets.All(IsValid))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var currentCustomer = new Customer
                {
                    FirstName = customerDto.FirstName,
                    LastName = customerDto.LastName,
                    Age = customerDto.Age,
                    Balance = customerDto.Balance
                };

               



                foreach (var ticket in customerDto.Tickets)
                {
                    var projection = context.Projections.FirstOrDefault(x => x.Id == ticket.ProjectionId);

                    if (projection==null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;

                    }

                    currentCustomer.Tickets.Add(new Ticket
                    {
                        Price = ticket.Price,
                        Customer = currentCustomer,
                        Projection = projection
                    });


                }

                context.Customers.Add(currentCustomer);

                context.SaveChanges();

                sb.AppendLine(
                    string.Format(SuccessfulImportCustomerTicket, currentCustomer.FirstName, currentCustomer.LastName, currentCustomer.Tickets.Count()));

            }

            return sb.ToString().TrimEnd();

        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}