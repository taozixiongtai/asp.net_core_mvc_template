using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace mymvc_core.Services.Email.SendGrid                 //这是官方文档推荐发邮件的方式。
{
  

    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<EmailOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public EmailOptions Options { get; }



        /// <summary>
        /// 邮件发送：单个收件人
        /// </summary>
        /// <param name="email">收件人邮箱</param>
        /// <param name="subject">主题</param>
        /// <param name="message">内容</param>
        /// <returns></returns>
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SendGridClient(Options.SendGridKey);
            var msg = new SendGridMessage()
            {
                From = Options.SendGridEmailFrom,
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            var resp = client.SendEmailAsync(msg);
            return resp;
        }


        #region      给多个人发送邮件
        /// <summary>
        /// 邮件发送：多个收件人
        /// </summary>
        /// <param name="emails">收件人邮箱列表</param>
        /// <param name="subject">主题</param>
        /// <param name="message">内容</param>
        /// <returns></returns>
        //public Task SendMultiEmailAsync(List<string> emails, string subject, string message)
        //{
        //    var client = new SendGridClient(Options.SendGridKey);
        //    var msg = new SendGridMessage()
        //    {
        //        From = Options.SendGridEmailFrom,
        //        Subject = subject,
        //        PlainTextContent = message,
        //        HtmlContent = message
        //    };
        //    msg.AddTos(emails.Select(email => { return new EmailAddress(email); }).ToList());
        //    var resp = client.SendEmailAsync(msg);
        //    return resp;
        //}
        #endregion

    }
}
