var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddConfig(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebApiServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "API v2");
    options.RoutePrefix = string.Empty;
});

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }