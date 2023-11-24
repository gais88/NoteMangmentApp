using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace NoteManagmentApp.UI.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var fromMail = "gais.dag88@outlook.com";
                var fromPassword = "nancygais88";

                var message = new MailMessage();
                message.From = new MailAddress(fromMail);
                message.Subject = subject;
                message.To.Add(email);
                message.Body = $"<html><body>{htmlMessage}</body></html>";
                message.IsBodyHtml = true;

                var smtpClient = new SmtpClient("smtp-mail.outlook.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromMail, fromPassword),
                    EnableSsl = true
                };

                await smtpClient.SendMailAsync(message);
            }
            catch (Exception)
            {

                throw;
            }
           
           
             
        }
    }
}
