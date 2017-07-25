using System;
using System.Net;
using System.Net.Mail;
using Zuul.Model;

namespace Zuul.BusinessLogic.Helpers
{
    public static class EmailHelper
    {
        private static string ForumEmailAccountAddress = System.Configuration.ConfigurationManager.AppSettings["Registration.EmailAccountName"];
        private static string FromPassword = System.Configuration.ConfigurationManager.AppSettings["Registration.EmailAccountPassword"];
        private const string ForumEmailAccountName = "The Forum";

        public static void SendUserRegistrationEmail(string emailAddress, string username, int userId, Guid token)
        {
            var fromAddress = new MailAddress(ForumEmailAccountAddress, ForumEmailAccountName);
            var toAddress = new MailAddress(emailAddress, username);
            
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
                Credentials = new NetworkCredential(fromAddress.Address, FromPassword),
                Timeout = 20000
            };
            using (var message = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = body })
            {
                smtp.Send(message);
            }
        }

        public static void SendUserPasswordResetEmail(UserAccount userAccount)
        {
            var fromAddress = new MailAddress(ForumEmailAccountAddress, ForumEmailAccountName);
            var toAddress = new MailAddress(userAccount.EmailAddress, userAccount.Username);
            const string subject = "The Forum: Password Reset";
            var body = string.Format(
@"To reset your password please copy and paste the following code into the update password form:

{0}

This code will expire after 24 hours.", userAccount.ResetToken);
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, FromPassword),
                Timeout = 20000
            };
            using (var message = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = body })
            {
                smtp.Send(message);
            }
        }
    }
}
