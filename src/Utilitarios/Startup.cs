using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Utilitarios
{

    public static class StartupCompartilhado
    {

        public static IServiceCollection ConfiguraApi(this IServiceCollection services, string contextoApi)
        {
            services.AddControllers();

            services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = Constantes.EnderecoIdentityServer;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });

            services.AddAuthorization(autorization =>
            {
                autorization.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", contextoApi);
                });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = contextoApi, Version = "v1" });
            });

            services.AddCors(option =>
            {
                option.AddPolicy("dafault", policy =>
                {
                    policy.AllowAnyMethod()
                          .AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            return services;
        }

        public static IApplicationBuilder ConfiguraApi(this IApplicationBuilder app, IWebHostEnvironment env, string contextoApi)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{contextoApi} v1"));
            }

            app.UseCors("default");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync(contextoApi + " OK!");
                }).RequireAuthorization("ApiScope");

                endpoints.MapControllers().RequireAuthorization("ApiScope");
            });

            return app;
        }
    }
}
