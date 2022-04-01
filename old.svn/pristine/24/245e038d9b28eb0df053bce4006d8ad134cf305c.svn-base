using MessageSendLib;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartTrapWebApp.Services
{
    public class MmiEmailSender : IEmailSender
    {
        EmailService emailService;
        EmailService EmailService
        {
            get
            {
                if(emailService == null)
                {
                    emailService = EmailService.Create(new SmtpInfo()
                    {
                        Account = "mmidevuser@gmail.com",
                        SmtpServer = "smtp.gmail.com",
                        Port = 465,
                        EnableSsl = true,
                        Password = "devuser999"

                    });
                }
                return emailService;
            }
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await EmailService.SendMessageAsync(new List<string> { email }, subject, htmlMessage);
        }
    }
}
