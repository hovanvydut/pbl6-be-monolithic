using Monolithic.Middlewares;
using Monolithic.Extensions;
using Monolithic.Helpers;
using Monolithic.Common;

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
// Add Http client
builder.Services.AddHttpClient();

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
builder.Services.ConfigureAWSS3(builder.Configuration);

// sentry
builder.WebHost.UseSentry();

// Logging
builder.Logging.ConfigureSerilog(builder.Configuration);

var app = builder.Build();
var logger = app.Services.GetRequiredService<ILoggerManager>();

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

app.ConfigureSuccessHandler(logger);
app.ConfigureErrorHandler(logger);

app.UseCustomAuthResponse();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();