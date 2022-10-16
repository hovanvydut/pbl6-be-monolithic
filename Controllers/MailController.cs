using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> Send([FromForm] MailContent mailContent)
    {
        await _sendMailHelper.SendEmailAsync(mailContent);
        return Ok();
    }
}