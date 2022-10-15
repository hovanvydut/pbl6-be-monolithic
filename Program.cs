using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Monolithic.Extensions;

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
builder.Services.ConfigureDI();

builder.Services.AddControllers();
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

var app = builder.Build();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
