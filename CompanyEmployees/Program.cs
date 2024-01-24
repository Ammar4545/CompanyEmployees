using CompanyEmployees.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;
using Presentation;

var builder = WebApplication.CreateBuilder(args);

//var dirPath = Directory.GetCurrentDirectory() + "\\nlog.config";
//LogManager.Setup().LoadConfigurationFromFile(dirPath);
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config")); // this is replaced with the code up

// Add services to the container.

builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureSqlContext(builder.Configuration);
// this line is adding controllers from the specified assembly to the API project.
// It allows your API project to dynamically discover and include controllers from another assembly at runtime.
builder.Services.AddControllers()
    .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();//adds HTTP Strict Transport Security (HSTS) headers to the responses,
                  //instructing browsers to enforce secure connections over HTTPS.
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseStaticFiles(); // if u did not use it will use a wwwroot folder in our project by default.

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All 
}); 
app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
