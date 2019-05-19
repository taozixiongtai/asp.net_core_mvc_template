using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace mymvc_core.Services.Email.MimeKit                
{


    public class EmailSender : IEmailSender
    {

        /// <summary>
        /// 使用MailKit发送邮件
        /// </summary>
        /// <param name="email">收件人邮箱地址</param>
        /// <param name="subject">标题</param>
        /// <param name="message">内容</param>
        /// <returns></returns>
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var Email = new MimeMessage();
            Email.From.Add(new MailboxAddress("taozixiongtai", "980077563@qq.com"));
            Email.To.Add(new MailboxAddress("桃子兄台", email));

            Email.Subject = subject;

            Email.Body = new TextPart("plain") { Text = message };

            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect("smtp.live.com", 587, false);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication     
                client.Authenticate("980077563@qq.com", "woshitaoziO0");
                await client.SendAsync(Email);
                client.Disconnect(true);
            }
        }
    }
}
