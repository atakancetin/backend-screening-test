// See https://aka.ms/new-console-template for more information
using BackEnd.Screening.Business;
using BackEnd.Screening.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;


Console.WriteLine("App is starting");

var builder = new ConfigurationBuilder();
builder.SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

IConfiguration config = builder.Build();
ScriptBuilder scriptBuilder = new ScriptBuilder();

ScreenTestBusiness business = new ScreenTestBusiness(config, scriptBuilder);
business.StartTest();
static IHostBuilder CreateDefaultBuilder()
{
    return Host.CreateDefaultBuilder()
        .ConfigureAppConfiguration(app =>
        {
            app.AddJsonFile("appsettings.json");
        });
}