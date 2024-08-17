using System.Net.Mail;
using System.Net;
using CORE.Abstract;
using CORE.Config;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace CORE.Concrete;
public class MailService : IMailService
{
    private readonly ConfigSettings _config;

    public MailService(ConfigSettings config)
    {
        _config = config;
    }

    public async Task SendMailAsync(string email, string message)
    {
        if (!string.IsNullOrEmpty(email) && email.Contains('@'))
        {
            var fromAddress = new MailAddress(_config.MailSettings.Address, _config.MailSettings.DisplayName);
            var toAddress = new MailAddress(email, email);

            var smtp = new SmtpClient
            {
                Host = _config.MailSettings.Host,
                Port = int.Parse(_config.MailSettings.Port),
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, _config.MailSettings.MailKey)
            };

            using var data = new MailMessage(fromAddress, toAddress)
            {
                Subject = _config.MailSettings.Subject,
                Body = message
            };

            await smtp.SendMailAsync(data);
        }
    }
}