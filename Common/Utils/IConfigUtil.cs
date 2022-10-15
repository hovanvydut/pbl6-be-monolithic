namespace Monolithic.Common;

public interface IConfigUtil
{
    string getAPIVersion();
    string getAWSAccessKey();
    string getAWSSecretKey();
    public string getAWSRegion();
    public string getAWSBucketName();
}