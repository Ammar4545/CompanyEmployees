using Contract;
using LoggerService;

namespace CompanyEmployees.Extensions
{
    public static class ServiceExtensions
    {
        //cors in mandatory to send requests from a different domain to our application
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            { 
                options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin() // to allow requests form indicated source not all sources use [WithOrigins("https://example.com")] 
                .AllowAnyMethod() // u can use [WithMethods("POST", "GET")] to allow these to method 
                .AllowAnyHeader()); //the WithHeaders("accept", "content-type") method to allow only specific headers.
            });

        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(option => { });

        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddSingleton<ILoggerManager, LoggerManager>();
    }
}
