using ApiGateway.Aggregators;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;


//namespace ApiGateway
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            new WebHostBuilder()
//               .UseKestrel()
//               .UseContentRoot(Directory.GetCurrentDirectory())
//               .ConfigureAppConfiguration((hostingContext, config) =>
//               {
//                   config
//                       .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
//                       .AddJsonFile("appsettings.json", true, true)
//                       .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
//                       .AddJsonFile("ocelot.json")
//                       .AddEnvironmentVariables();
//               })
//               .ConfigureServices(s => {
//                   s.AddControllers();
//                   s.AddOcelot();
//                   s.AddEndpointsApiExplorer();
//                   s.AddSwaggerGen();
//               })
//               .ConfigureLogging((hostingContext, logging) =>
//               {
//                   //add your logging
//               })
//               .UseIISIntegration()
//               .Configure(app =>
//               {
//                   app.UseOcelot().Wait();
//                   app.UseSwagger();
//                   app.UseSwaggerUI();
//               })
//               .Build()
//               .Run();
//        }
//    }
//}


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOcelot()
    .AddSingletonDefinedAggregator<GetProductAggregator>();
builder.Configuration.AddJsonFile("ocelot.json")
    .AddEnvironmentVariables();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.UseOcelot().Wait();

app.Run();
