using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Monolithic.Common;
using Monolithic.Extensions;
using Monolithic.Middlewares;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Network;

var builder = WebApplication.CreateBuilder(args);

// Cors
builder.Services.AddCors(c =>
    c.AddDefaultPolicy(options =>
    {
        options.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    })
);

// Add services to the container.
builder.Services.ConfigureDataContext(builder.Configuration);
builder.Services.ConfigureModelSetting(builder.Configuration);
builder.Services.ConfigureDI(new ConfigUtil(builder.Configuration));
builder.Services.ConfigureAuth(builder.Configuration);

builder.Services.AddControllers();

// generate lowercase URLs
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// s3
AWSOptions awsOptions = new AWSOptions
{
    Credentials = new BasicAWSCredentials("AKIAXVARHJLQETQ5NGF2", "hFY184c9ff+IX3HW40VJXaaIIPFw32tzHYW+D0db")
};
builder.Services.AddDefaultAWSOptions(awsOptions);
// builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();

// sentry
builder.WebHost.UseSentry();

// Logging
var log = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithExceptionDetails()
    .WriteTo.Console()
    .WriteTo.TCPSink("tcp://node-3.silk-cat.software", 50000)
    // .WriteTo.DurableHttpUsingFileSizeRolledBuffers(requestUri: "http://localhost:8001/pbl6-api")
    .CreateLogger();

builder.Logging.AddSerilog(log);

var app = builder.Build();

// sentry
app.UseSentryTracing();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
app.UseSwagger();
app.UseSwaggerUI();

app.ConfigureErrorHandler();

app.UseCustomAuthResponse();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();