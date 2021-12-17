using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API_RVOES.AppCode.Services
{
    public interface IEmailService
    {
        int Send(string from, string to, string subject, string html);
        string getBodyNuevoUsuario(string sClaveUsuario, string sPassword, string sUrl, string NombreCompleto);
    }
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        //private IHostingEnvironment _env;
        private IWebHostEnvironment _env;
        public EmailService(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        public int Send(string from, string to, string subject, string html)
        {
            SmtpClient smtp = new SmtpClient();
            MimeMessage email = new MimeMessage();
            try
            {
                // create message
                email.From.Add(MailboxAddress.Parse(from));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = html };

                // send email
                string sHost = _config["EmailData:MailServer"];
                int sPort = Int32.Parse(_config["EmailData:Port"]);
                string sSmtpUser = _config["EmailData:Account"];
                string sSmtpPass = _config["EmailData:Password"];


                smtp.Connect(sHost, sPort, SecureSocketOptions.StartTls);
                smtp.Authenticate(sSmtpUser, sSmtpPass);
                smtp.Send(email);

                return 0;
            }
            catch (SmtpCommandException ex)
            {
                SmtpStatusCode statusCode = ex.StatusCode;
                if (statusCode == SmtpStatusCode.MailboxBusy ||
                statusCode == SmtpStatusCode.MailboxUnavailable ||
                statusCode == SmtpStatusCode.TransactionFailed)
                {
                    // Esperamos 5 segundos y se intenta enviar el correo nuevamente
                    Thread.Sleep(5000);
                    smtp.Send(email);
                    return 0;
                }
                else
                {
                    throw ex;
                }
            }
            finally
            {
                smtp.Disconnect(true);
            }
        }

        public string getBodyNuevoUsuario(string sClaveUsuario, string sPassword, string sUrl, string NombreCompleto)
        {
            string strBody = string.Empty;
            try
            {
                BodyBuilder bodyBuilder = new BodyBuilder();
                var pathToFile = _env.ContentRootPath
                                + Path.DirectorySeparatorChar.ToString()
                                + "Templates"
                                + Path.DirectorySeparatorChar.ToString()
                                + "EmailTemplates"
                                + Path.DirectorySeparatorChar.ToString()
                                + "AccesoUsuario.html";
                using (StreamReader reader = System.IO.File.OpenText(pathToFile))
                {
                    bodyBuilder.HtmlBody = reader.ReadToEnd();
                }
                strBody = bodyBuilder.HtmlBody;
                strBody = strBody.Replace("{USERNAME}", NombreCompleto);
                strBody = strBody.Replace("{USER}", sClaveUsuario);
                strBody = strBody.Replace("{PASSW}", sPassword);
                strBody = strBody.Replace("{URL}", sUrl);

            }
            catch (Exception ex)
            {
                throw;
            }
            return strBody;
        }

    }
}
