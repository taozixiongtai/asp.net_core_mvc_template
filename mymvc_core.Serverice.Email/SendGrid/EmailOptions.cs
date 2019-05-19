using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mymvc_core.Services.Email.SendGrid
{
    public class EmailOptions
    {
        public string SendGridKey { get; set; } = "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";
        public EmailAddress SendGridEmailFrom { get; set; } = new EmailAddress("980077563@qq.com", "桃子兄台");//邮件默认发送人与显示名称
    }
}
