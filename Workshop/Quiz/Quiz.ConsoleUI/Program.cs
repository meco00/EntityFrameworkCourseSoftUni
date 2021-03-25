using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quiz.Data;
using Quiz.Services;
using System;
using System.IO;

namespace Quiz.ConsoleUI
{
  public  class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureService(serviceCollection);
           var serviceProvider= serviceCollection.BuildServiceProvider();

            var jsonImporter = serviceProvider.GetService<IJsonQuestionImportService>();
            jsonImporter.Import("EF-Core-Quiz.json", "EF Core Test v2");



        }

        private static void ConfigureService(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();


            services.AddDbContext<ApplicationDbContext>(options=>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
               .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddTransient<IQuizService, QuizSerivce>();
            services.AddTransient<IQuestionService, QuestionService>();
            services.AddTransient<IAnswerService, AnswerService>();
            services.AddTransient<IUserAnswerService, UserAnswerService>();
            services.AddTransient<IJsonQuestionImportService, JsonQuestionImportService>();
        }
    }
}
