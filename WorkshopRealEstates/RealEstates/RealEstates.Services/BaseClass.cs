using AutoMapper;
using RealEstates.Services.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstates.Services
{
   public abstract class BaseClass
    {
        protected IMapper mapper;


        public BaseClass()
        {
            InitializeMapper();
        }

        private void InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
              cfg.AddProfile<RealEstateProfile>());

            mapper = config.CreateMapper();
        }
    }
}
