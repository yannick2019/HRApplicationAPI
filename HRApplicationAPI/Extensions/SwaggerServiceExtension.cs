using HRApplicationAPI.Filters;
using Microsoft.OpenApi.Models;

namespace HRApplicationAPI.Extensions
{
    public static class SwaggerServiceExtension
    {
        public static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "HR Application CRUD",
                    Version = "v1"
                });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                option.OperationFilter<AuthResponsesOperationFilter>();
            });

            return services;
        }
    }
}
