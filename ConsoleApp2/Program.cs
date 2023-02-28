using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.IO;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json");

            var configuration = builder.Build();
            SendEmail(configuration);
        }
        static void SendEmail(IConfiguration config)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(config.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse("davidslobodian08@gmail.com"));
            email.Subject = "Test Email Subject";
            email.Body = new TextPart(TextFormat.Html) { Text = "Test message" };

            using var smtp = new SmtpClient();

            smtp.Connect(config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(config.GetSection("EmailUsername").Value, config.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
