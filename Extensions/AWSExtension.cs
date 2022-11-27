using Amazon.Extensions.NETCore.Setup;
using Monolithic.Common;
using Amazon.Runtime;
using Amazon.S3;

namespace Monolithic.Extensions;

public static class AWSExtension
{
    public static void ConfigureAWSS3(this IServiceCollection services, IConfiguration configuration)
    {
        var s3Settings = configuration.GetSection("AWSS3").Get<AWSS3Settings>();

        AWSOptions awsOptions = new AWSOptions
        {
            Credentials = new BasicAWSCredentials(s3Settings.AccessKey, s3Settings.SecretKey)
        };
        services.AddDefaultAWSOptions(awsOptions);
        // services.AddDefaultAWSOptions(configuration.GetAWSOptions());
        services.AddAWSService<IAmazonS3>();
    }
}