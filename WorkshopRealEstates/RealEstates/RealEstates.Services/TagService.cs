using RealEstates.Data;
using RealEstates.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstates.Services
{
    public class TagService : ITagService
    {
        private readonly ApplicationDbContext db;

        private readonly IPropertiesService propertiesService;

        public TagService(ApplicationDbContext context,IPropertiesService propertiesService)
        {
            db = context;
            this.propertiesService = propertiesService;

        }

        public void Add(string name, int? importance = null)
        {
            var tag = new Tag
            {
                Name = name,
                Importance = importance

            };

            var contains = db.Tags.Any(x => x.Name == tag.Name);

            if (contains)
            {
                throw new ArgumentException("This tag already exists in context");
            }

            db.Tags.Add(tag);
            db.SaveChanges();

        }

        public void BulkAdd()
        {
            var TagNamesToImport = new string[]
            {
                "скъп-имот",
                "евтин-имот",
                "нов-имот",
                "стар-имот",
                "голям-имот",
                "малък имот",
                "последен етаж",
                "първи етаж"

            };

            foreach (var tagName in TagNamesToImport)
            {
                var importance = new Random().Next(0, 8);

                Add(tagName, importance);



            }


        }

        public void MapTagsToProperties()
        {
            var properties = db.Properties.ToList();

            foreach (var property in properties)
            {
                var averagePricePerSquareMeterOfDistrict = 
                    propertiesService
                    .AveragePricePerSquareMeterForDistrict(property.DistrictId);

                ;

                if (property.Price.HasValue && property.Price.Value> averagePricePerSquareMeterOfDistrict)
                {
                    var expensiveTag = GetTag("скъп-имот");

                    property.Tags.Add(expensiveTag);

                }
                else
                {
                    var cheapTag = GetTag("евтин-имот");

                    property.Tags.Add(cheapTag);
                }

                var year = DateTime.Now.AddYears(-15).Year;

                if (property.Year>year)
                {
                    var newBuildingTag = GetTag("нов-имот");

                    property.Tags.Add(newBuildingTag);
                }
                else
                {
                    var oldBuildingTag= GetTag("стар-имот");

                    property.Tags.Add(oldBuildingTag);

                }

                var averageSizeForDistrict = 
                    propertiesService
                    .AverageSize(property.DistrictId);


                if (property.Size > averageSizeForDistrict )
                {
                    Tag bigBuildingTag = GetTag("голям-имот");

                    property.Tags.Add(bigBuildingTag);

                }
                else
                {
                    var smallBuildingTag= GetTag("малък имот");

                    property.Tags.Add(smallBuildingTag);
                }

                if (property.Floor.HasValue && property.TotalFloors.HasValue && property.Floor.Value == property.TotalFloors.Value)
                {
                    var lastFloorTag= GetTag("последен етаж");

                    property.Tags.Add(lastFloorTag);

                }
                else if (property.Floor.HasValue && property.Floor.Value==1)
                {
                    var lastFloorTag = GetTag("първи етаж");

                    property.Tags.Add(lastFloorTag);
                }


                db.SaveChanges();











            }



        }

        private Tag GetTag(string name)
        {
            return db.Tags.FirstOrDefault(x => x.Name == name);
        }
    }
}
