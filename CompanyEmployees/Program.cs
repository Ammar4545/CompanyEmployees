using CompanyEmployees.Extensions;
using Contract;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using NLog;
using Presentation;
using Presentation.ActionFilters;

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
// add extra configuration that enable the server to format the XML response when the client tries negotiating for it.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{ 
    options.SuppressModelStateInvalidFilter = true;
});

#region
//we are creating a local function. This function configures support for JSON Patch using
//Newtonsoft.Json while leaving the other formatters unchanged.
#endregion
NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
    new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
    .Services.BuildServiceProvider().GetRequiredService<IOptions<MvcOptions>>()
    .Value.InputFormatters.OfType<NewtonsoftJsonPatchInputFormatter>().First();

builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;
    config.ReturnHttpNotAcceptable = true;
    config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
}).AddXmlDataContractSerializerFormatters();

    // { config.RespectBrowserAcceptHeader = true; })
    //.AddXmlDataContractSerializerFormatters()
    //.AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly)
    //.AddCustomCSVFormatter();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<ValidationFilterAttribute>();

//builder.Services.AddMvc().AddNewtonsoftJson();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(logger);

if (app.Environment.IsProduction()) app.UseHsts();

#region
//if (app.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//}
//else
//{
//    app.UseHsts();//adds HTTP Strict Transport Security (HSTS) headers to the responses,
//                  //instructing browsers to enforce secure connections over HTTPS.
//}
#endregion

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
