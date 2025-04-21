using System.Reflection;
using System.Text.Json.Serialization;
using Robobobot.Core;
using Robobobot.Server.BackgroundServices;
using Robobobot.Server.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddControllers()
                .AddJsonOptions(options => 
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddSingleton<BattleService>();
builder.Services.AddSingleton<GameLoopHostedService>();
builder.Services.AddSingleton<IFpsController>(provider => provider.GetService<GameLoopHostedService>()!);
builder.Services.AddHostedService<GameLoopHostedService>(provider => provider.GetService<GameLoopHostedService>()!);

builder.Services.AddCors(options =>
{
    options.AddPolicy("anythingGoesPolicy",
        policy =>
        {
            policy.AllowAnyMethod();
            policy.AllowAnyOrigin();
            policy.SetIsOriginAllowed(origin => true);
            policy.AllowAnyHeader();
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