using mymvc_core.Services.Email;
using System.Text.Encodings.Web;
using System.Threading.Tasks;


namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    /// 发送邮件的扩展
    /// </summary>
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "标题是确定邮箱",
                $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>");
        }
    }
}
