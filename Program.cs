
using System.Security.Claims;
using AspNetCore.Authentication.Basic;
using dotNET_lab_8.Data;
using dotNET_lab_8.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddSingleton<IFoxesRepository, FoxesRepository>();

builder.Services.AddAuthentication(BasicDefaults.AuthenticationScheme).AddBasic(options =>
{
    options.Realm = "Fox API";
    options.Events = new BasicEvents
    {
        OnValidateCredentials = async (context) =>
        {
            var user = context.Username;

            var isValid = user == "user" && context.Password == "password";
            if (isValid)
            {
                context.Response.Headers.Add("ValidationCustomHeader", "From OnValidateCredentials");
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, context.Username, ClaimValueTypes.String, context.Options.ClaimsIssuer),
                    new Claim(ClaimTypes.Name, context.Username, ClaimValueTypes.String, context.Options.ClaimsIssuer)
                };
                context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name)
                );
                context.Success();
            }
            else
            {
                context.NoResult();
            }
        }
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();


app.UseAuthentication();
app.UseAuthorization();

app.UseFileServer();
app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
