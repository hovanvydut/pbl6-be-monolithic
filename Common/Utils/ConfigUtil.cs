namespace Monolithic.Common;

public class ConfigUtil : IConfigUtil
{
    private readonly IConfiguration _config;

    public ConfigUtil(IConfiguration config)
    {
        _config = config;
    }

    private IConfigurationSection getAWSConfig()
    {
        return _config.GetSection("AWSS3");
    }

    public string getAWSAccessKey()
    {
        return this.getAWSConfig().GetSection("AccessKey").Value;
    }

    public string getAWSSecretKey()
    {
        return this.getAWSConfig().GetSection("SecretKey").Value;
    }

    public string getAWSRegion()
    {
        return this.getAWSConfig().GetSection("region").Value;
    }

    public string getAWSBucketName()
    {
        return this.getAWSConfig().GetSection("bucket").Value;
    }

    public string getAPIVersion()
    {
        return _config.GetSection("APIVersion").Value;
    }
}