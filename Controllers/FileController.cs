using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Common;
using Monolithic.Constants;
using Monolithic.Models.Common;

namespace Monolithic.Controllers;

public class FileControler : BaseController
{
    private readonly IAmazonS3 _s3Client;
    private readonly IConfigUtil _configUtil;
    public FileControler(IConfigUtil configUtil)
    {
        _configUtil = configUtil;

        var configS3 = new AmazonS3Config()
        {
            RegionEndpoint = Amazon.RegionEndpoint.APSoutheast1
        };
        var credentials = new BasicAWSCredentials(_configUtil.getAWSAccessKey(), configUtil.getAWSSecretKey());
        _s3Client = new AmazonS3Client(credentials, configS3);
    }

    [HttpPost("upload")]
    public async Task<BaseResponse<string>> UploadFileAsync(IFormFile file, string prefix = "")
    {
        string bucketName = _configUtil.getAWSBucketName();
        string region = _configUtil.getAWSRegion();

        var bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);
        if (!bucketExists) throw new BaseException(HttpCode.NOT_FOUND, $"Bucket {bucketName} does not exist.");

        string filePath = string.IsNullOrEmpty(prefix) ? file.FileName : $"{prefix?.TrimEnd('/')}/{file.FileName}";
        var request = new PutObjectRequest()
        {
            BucketName = "pbl6",
            Key = filePath,
            InputStream = file.OpenReadStream()
        };
        request.Metadata.Add("Content-Type", file.ContentType);
        await _s3Client.PutObjectAsync(request);

        return new BaseResponse<string>($"https://{bucketName}.s3.{region}.amazonaws.com/{filePath}", HttpCode.OK);
    }
}