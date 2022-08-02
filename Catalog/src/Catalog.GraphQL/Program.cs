using Catalog.GraphQL;
using Catalog.GraphQL.GraphQL;
using Catalog.Infrastructure.Persistence;
using GraphQL.Server.Ui.Playground;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebApiServices(builder.Configuration);

var app = builder.Build();

app.UseGraphQL<CategoriesSchema>("/api/categories");
app.UseGraphQL<ProductsSchema>("/api/products");

app.UseGraphQLPlayground(new PlaygroundOptions { GraphQLEndPoint = "/api/categories" }, "/ui/categories");
app.UseGraphQLPlayground(new PlaygroundOptions { GraphQLEndPoint = "/api/products" }, "/ui/products");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();

    // Initialise and seed database
    using (var scope = app.Services.CreateScope())
    {
        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
    }
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }