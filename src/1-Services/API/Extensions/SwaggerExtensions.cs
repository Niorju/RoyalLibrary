using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace WebAPI.NETCore.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();
            return services;
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((document, request) =>
                {
                    document.Servers = new List<OpenApiServer>
                {
                        new OpenApiServer {Url = $"https://{request.Host.Value}"}
                };
                });
            });
            app.UseSwaggerUI(c =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    var notice = description.IsDeprecated ? " This API version has been deprecated." : string.Empty;
                    c.SwaggerEndpoint(
                    $"./{description.GroupName}/swagger.json",
                    $"Torc V{description.ApiVersion} API {description.GroupName.ToUpperInvariant()}" + notice);
                }
                c.DocExpansion(DocExpansion.List);
            });
            return app;
        }
    }
}

