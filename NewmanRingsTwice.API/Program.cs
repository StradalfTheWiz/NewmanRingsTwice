using Microsoft.AspNetCore.HttpOverrides;
using NewmanRingsTwice.API.Setup;
using NewmanRingsTwice.Domain.Shared;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var env = EnvironmentService.GetEnvironmentTypeEnum(builder.Environment);
var clientOrigin = env != EnvironmentType.Production ? "http://localhost:5173" : "https://newman.alvrineom.com";

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
    builder => builder
        .WithOrigins(clientOrigin)
        .AllowAnyMethod()
        .AllowAnyHeader()
    );
});

builder.Services.SetServices(env, builder.Configuration);

var app = builder.Build();

app.SetMiddleware();

if (!app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });
}

app.UseCors("CorsPolicy");
app.MapControllers();
app.Run();