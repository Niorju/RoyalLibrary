
using Application.Interfaces;
using Application.Profiles;
using Application.Services;
using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoyalLibrary.Domain.Entities;
using RoyalLibrary.Infra;
using System;
using WebAPI.NETCore.Extensions;

namespace WebAPI.NETCore
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<InfraDbContext>(opt => opt.UseInMemoryDatabase("TorcDataBase"));

            services
                .AddMemoryCache()                
                .AddSwaggerConfiguration()                
                .AddCors();

            services.AddVersionedApiExplorer(setupAction =>
            {
                setupAction.GroupNameFormat = "'v'VV";
            });

            services.AddApiVersioning(setupAction =>
            {
                setupAction.AssumeDefaultVersionWhenUnspecified = true;
                setupAction.DefaultApiVersion = new ApiVersion(1, 0);
                setupAction.ReportApiVersions = true;
                setupAction.ApiVersionReader = new HeaderApiVersionReader("api-version");
                setupAction.ApiVersionReader = new MediaTypeApiVersionReader();
            });

            var apiVersionDescriptionProvider =
               services.BuildServiceProvider().GetService<IApiVersionDescriptionProvider>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();            
            services.AddSingleton(mapper);

            services.AddScoped<IBookRepositorie, InfraDbContext>();
            services.AddScoped<IBookDomainServices, BookDomainService>();
            services.AddScoped<IBookApplicationServices, BookApplicationServices>();


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider,
            IServiceProvider serviceProvider)
        {
            app.UseRouting()
                .UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader())
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                })
                .UseSwaggerConfiguration(provider);

            var context = serviceProvider.GetRequiredService<InfraDbContext>();
            LoadBooks(context);
        }
                
        private static void LoadBooks(InfraDbContext context)
        {

            var book = new Book()
            {
                BookId = 1,
                Title = "Pride and Prejudice",
                FirstName = "Jane",
                LastName = "Austen",
                TotalCopies = 100,
                CopiesInUse = 80,
                Type = "Hardcover", 
                Isbn = "1234567891",
                Category = "Fiction"
            };
            context.Books.Add(book);

            book = new Book()
            {
                BookId = 2,
                Title = "To Kill a Mockingbird",
                FirstName = "Harper",
                LastName = "Lee",
                TotalCopies = 75,
                CopiesInUse = 65,
                Type = "Paperback",
                Isbn = "1234567892",
                Category = "Fiction"
            };
            context.Books.Add(book);

            book = new Book()
            {
                BookId = 3,
                Title = "The Catcher in the Rye",
                FirstName = "J.D.",
                LastName = "Salinger",
                TotalCopies = 50,
                CopiesInUse = 45,
                Type = "Hardcover",
                Isbn = "1234567893",
                Category = "Fiction"
            };
            context.Books.Add(book);

            book = new Book()
            {
                BookId = 4,
                Title = "The Great Gatsby",
                FirstName = "F. Scott",
                LastName = "Fitzgerald",
                TotalCopies = 50,
                CopiesInUse = 22,
                Type = "Hardcover",
                Isbn = "1234567894",
                Category = "Non-Fiction"
            };
            
            context.Books.Add(book);

            context.SaveChanges();
        }
    }
}
