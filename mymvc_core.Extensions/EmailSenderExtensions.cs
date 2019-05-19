using mymvc_core.Services.Email;
using System.Text.Encodings.Web;
using System.Threading.Tasks;


namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    /// �����ʼ�����չ
    /// </summary>
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "������ȷ������",
                $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>");
        }
    }
}
