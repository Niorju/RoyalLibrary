using Application.Interfaces;
using Application.Profiles;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Services;
using Infra;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using RoyalLibrary.Infra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RoyalLibrary.WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {           

            //NOTE: Configurando o EF Core In-Memory na aplicação
            services.AddDbContext<InfraDbContext>(opt => opt.UseInMemoryDatabase("SGPDataBase"));

            services.AddMvc(setupAction =>
            {
                setupAction.Filters.Add(
                    new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
                setupAction.Filters.Add(
                    new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
                setupAction.Filters.Add(
                    new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
                setupAction.Filters.Add(
                    new ProducesDefaultResponseTypeAttribute());
                setupAction.Filters.Add(
                    new ProducesResponseTypeAttribute(StatusCodes.Status401Unauthorized));

                setupAction.Filters.Add(
                    new AuthorizeFilter());

                setupAction.ReturnHttpNotAcceptable = true;

                setupAction.OutputFormatters.Add(new XmlSerializerOutputFormatter());

                var jsonOutputFormatter = setupAction.OutputFormatters
                    .OfType<JsonOutputFormatter>().FirstOrDefault();

                if (jsonOutputFormatter != null)
                {
                    // NOTE: Remove por que não é padrão 
                    // trabalhar com esse tipo para API
                    if (jsonOutputFormatter.SupportedMediaTypes.Contains("text/json"))
                    {
                        jsonOutputFormatter.SupportedMediaTypes.Remove("text/json");
                    }
                }
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var actionExecutingContext =
                        actionContext as Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;

                    // NOTE: Se houver erros no ModelState e todas as 
                    // chaves estiverem corretas estamos lidando com erros de validação
                    if (actionContext.ModelState.ErrorCount > 0
                        && actionExecutingContext?.ActionArguments.Count == actionContext.ActionDescriptor.Parameters.Count)
                    {
                        return new UnprocessableEntityObjectResult(actionContext.ModelState);
                    }

                    //NOTE: Se uma das chaves não estiver correta ou não foi encontrtada
                    // estamos lidando com erros de validação
                    return new BadRequestObjectResult(actionContext.ModelState);
                };
            });

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            //NOTE: cria uma única instância do serviço quando é solicitada 
            //pela primeira vez e reutiliza a mesma instância em todos os locais 
            //onde esse serviço é necessário.
            services.AddSingleton(mapper);

            //NOTE: Um serviço com escopo definido com cada solicitação HTTP, 
            //obtemos uma nova instância
            //Se o serviço for necessário em vários locais, como na visualização e no controlador, 
            //a mesma instância será fornecida para todo o escopo dessa solicitação HTTP
            services.AddScoped<ICenasRepositorie, InfraDbContext>();
            services.AddScoped<ICenasDomainServices, CenasDomainService>();
            services.AddScoped<ICenasApplicationServices, CenasApplicationServices>();

            services.AddVersionedApiExplorer(setupAction =>
            {
                setupAction.GroupNameFormat = "'v'VV";
            });

            //NOTE: Autenticação Básica (Como demonstrativo)
            services.AddAuthentication("Basic")
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);

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

            //Middleware
            services.AddSwaggerGen(setupAction =>
            {
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    setupAction.SwaggerDoc(
                        $"LibraryOpenAPISpecification{description.GroupName}",
                        new Microsoft.OpenApi.Models.OpenApiInfo()
                        {
                            Title = "Desafio Rede Globo",
                            Version = description.ApiVersion.ToString(),
                            Description = "API de Cenas",
                            Contact = new OpenApiContact()
                            {
                                Email = "jorge_junior@yahoo.com.br",
                                Name = "Jorge Junior",
                                Url = new Uri("https://github.com/")
                            }                           
                        });
                }
                setupAction.AddSecurityDefinition("basicAuth", new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    Description = "Informe seu nome de usuário e senha para acessar esta API"
                });

                setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "basicAuth" }
                        }, new List<string>() }
                });

                setupAction.DocInclusionPredicate((documentName, apiDescription) =>
                {
                    var actionApiVersionModel = apiDescription.ActionDescriptor
                    .GetApiVersionModel(ApiVersionMapping.Explicit | ApiVersionMapping.Implicit);

                    if (actionApiVersionModel == null)
                    {
                        return true;
                    }

                    if (actionApiVersionModel.DeclaredApiVersions.Any())
                    {
                        return actionApiVersionModel.DeclaredApiVersions.Any(v =>
                        $"LibraryOpenAPISpecificationv{v.ToString()}" == documentName);
                    }
                    return actionApiVersionModel.ImplementedApiVersions.Any(v =>
                        $"LibraryOpenAPISpecificationv{v.ToString()}" == documentName);
                });

                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

                setupAction.IncludeXmlComments(xmlCommentsFullPath);

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
                 IApiVersionDescriptionProvider apiVersionDescriptionProvider, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. 
                // You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }           

            app.UseHttpsRedirection();
            app.UseSwagger();

            app.UseSwaggerUI(setupAction =>
            {
                setupAction.InjectStylesheet("/Assets/custom-ui.css");
                setupAction.IndexStream = ()
                        => GetType().Assembly
                        .GetManifestResourceStream("WebAPI.NETCore.EmbeddedAssets.index.html");

                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    setupAction.SwaggerEndpoint($"/swagger/" +
                        $"LibraryOpenAPISpecification{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }              

                setupAction.RoutePrefix = "";

                setupAction.DefaultModelExpandDepth(2);
                setupAction.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);
                setupAction.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                setupAction.EnableDeepLinking();
                setupAction.DisplayOperationId();
            });

            app.UseStaticFiles();

            app.UseAuthentication();           

            //NOTE: Carregando Dados na memória
            var context = serviceProvider.GetRequiredService<InfraDbContext>();            
            LoadCenas(context);

            app.UseMvc();

        }

        //NOTE: Incluindo dados de testes no banco de dados in-memory quando a aplicação for inicializada
        private static void LoadCenas(InfraDbContext context)
        {
            DateTime dt = DateTime.Now;
            //string dateFormated = dt.ToString("dd/MM/yyyy HH:mm:ss");

            Cenas cenas = new Cenas()
            {
                Id = 1,
                NmCena = "Cena 1",
                ConteudoCena = "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit",
                DataCena = dt, // Convert.ToDateTime(dateFormated),
                DataAtualizacao = dt, //Convert.ToDateTime(dateFormated),
                StatusCenas = "Pendente"
            };
            context.Cenas.Add(cenas);
            cenas = new Cenas()
            {
                Id = 2,
                NmCena = "Cena 2",
                ConteudoCena = "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit",
                DataCena = dt, // Convert.ToDateTime(dateFormated),
                DataAtualizacao = dt, //Convert.ToDateTime(dateFormated),
                StatusCenas = "Preparada"
            };
            context.Cenas.Add(cenas);
            cenas = new Cenas()
            {
                Id = 3,
                NmCena = "Cena 3",
                ConteudoCena = "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit",
                DataCena = dt, // Convert.ToDateTime(dateFormated),
                DataAtualizacao = dt, //Convert.ToDateTime(dateFormated),
                StatusCenas = "Pendurada"
            };
            context.Cenas.Add(cenas);
            cenas = new Cenas()
            {
                Id = 4,
                NmCena = "Cena 4",
                ConteudoCena = "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit",
                DataCena = dt, // Convert.ToDateTime(dateFormated),
                DataAtualizacao = dt, //Convert.ToDateTime(dateFormated),
                StatusCenas = "Gravada"
            };
            context.Cenas.Add(cenas);
            cenas = new Cenas()
            {
                Id = 5,
                NmCena = "Cena 5",
                ConteudoCena = "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit",
                DataCena = dt, // Convert.ToDateTime(dateFormated),
                DataAtualizacao = dt, //Convert.ToDateTime(dateFormated),
                StatusCenas = "Pendente"
            };
            context.Cenas.Add(cenas);
            cenas = new Cenas()
            {
                Id = 6,
                NmCena = "Cena 6",
                ConteudoCena = "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit",
                DataCena = dt, // Convert.ToDateTime(dateFormated),
                DataAtualizacao = dt, //Convert.ToDateTime(dateFormated),
                StatusCenas = "Preparada"
            };
            context.Cenas.Add(cenas);
            cenas = new Cenas()
            {
                Id = 7,
                NmCena = "Cena 7",
                ConteudoCena = "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit",
                DataCena = dt, // Convert.ToDateTime(dateFormated),
                DataAtualizacao = dt, //Convert.ToDateTime(dateFormated),
                StatusCenas = "Pendurada"
            };
            context.Cenas.Add(cenas);
            cenas = new Cenas()
            {
                Id = 8,
                NmCena = "Cena 8",
                ConteudoCena = "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit",
                DataCena = dt, // Convert.ToDateTime(dateFormated),
                DataAtualizacao = dt, //Convert.ToDateTime(dateFormated),
                StatusCenas = "Gravada"
            };
            context.Cenas.Add(cenas);
            cenas = new Cenas()
            {
                Id = 9,
                NmCena = "Cena 9",
                ConteudoCena = "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit",
                DataCena = dt, // Convert.ToDateTime(dateFormated),
                DataAtualizacao = dt, //Convert.ToDateTime(dateFormated),
                StatusCenas = "Pendente"
            };
            context.Cenas.Add(cenas);
            cenas = new Cenas()
            {
                Id = 10,
                NmCena = "Cena 10",
                ConteudoCena = "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit",
                DataCena = dt, // Convert.ToDateTime(dateFormated),
                DataAtualizacao = dt, //Convert.ToDateTime(dateFormated),
                StatusCenas = "Gravada"
            };
            context.Cenas.Add(cenas);

            context.SaveChanges();
        }
    }
}
