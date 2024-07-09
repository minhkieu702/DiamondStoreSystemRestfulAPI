using DiamondStoreSystem.BusinessLayer.Commons;
using DiamondStoreSystem.BusinessLayer.IServices;
using DiamondStoreSystem.BusinessLayer.ResponseModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondStoreSystem.BusinessLayer.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _config;

        public MailService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<IDSSResult> SendMail(string to, string subject, string htmlMessage)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(_config["MailSettings:DisplayName"], _config["MailSettings:Mail"]);
            email.From.Add(new MailboxAddress(_config["MailSettings:DisplayName"], _config["MailSettings:Mail"]));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };
            email.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                smtp.Connect(_config["MailSettings:Host"], int.Parse(_config["MailSettings:Port"]), MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate(_config["MailSettings:Mail"], _config["MailSettings:Password"]);
                var message = await smtp.SendAsync(email);
                if (message == null) return new DSSResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                return new DSSResult(Const.SUCCESS_CREATE_CODE, message);
            }
            catch (Exception ex)
            {
                return new DSSResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
    }
}
