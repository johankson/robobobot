using System.Reflection;
using Robobobot.Core;
using Robobobot.Server.BackgroundServices;
using Robobobot.Server.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddSingleton(new BattleService());
builder.Services.AddSingleton<TimedHostedService>();
builder.Services.AddSingleton<IFpsController>(provider => provider.GetService<TimedHostedService>()!);
builder.Services.AddHostedService<TimedHostedService>(provider => provider.GetService<TimedHostedService>()!);

builder.Services.AddCors(options =>
{
    options.AddPolicy("anythingGoesPolicy",
        policy =>
        {
            policy.AllowAnyOrigin();
            //.WithExposedHeaders("x-player-token");
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors("anythingGoesPolicy");

app.Run();