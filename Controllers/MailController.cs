using static Monolithic.Constants.PermissionPolicy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Models.Common;
using Monolithic.Helpers;

namespace Monolithic.Controllers;

public class MailController : BaseController
{
    private readonly ISendMailHelper _sendMailHelper;

    public MailController(ISendMailHelper sendMailHelper)
    {
        _sendMailHelper = sendMailHelper;
    }

    [HttpPost("Send")]
    // [Authorize(Roles = EmailPermission.Send)]
    public async Task<BaseResponse<string>> Send([FromForm] MailContent mailContent)
    {
        await _sendMailHelper.SendEmailAsync(mailContent);
        return new BaseResponse<string>($"Sent mail to {mailContent.ToEmail}");
    }
}