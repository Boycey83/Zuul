using System;
using System.Net;
using System.Net.Mail;

namespace Zuul.BusinessLogic.Helpers
{
    public static class EmailHelper
    {
        public static void SendUserRegistrationEmail(string emailAddress, string username, int userId, Guid token)
        {
            var fromAddress = new MailAddress("register.theforum@gmail.com", "The Forum");
            var toAddress = new MailAddress(emailAddress, username);
            const string fromPassword = "UuC47ZvqjeNzPQxNC3bKn9ksOLAtsV9czSwVFGSIRQ21qcSKzAABrv2zktGro2Gu";
            const string subject = "The Forum Registration: Please confirm new account";
            var body = string.Format(
                "Go to the following address to enable your new account: http://theforum.cloudapp.net/UserAccount/{0}/Confirm/{1}",
                userId, token);
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };
            using (var message = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = body })
            {
                smtp.Send(message);
            }
        }
    }
}
