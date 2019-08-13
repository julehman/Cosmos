using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.Classes
{
    public static class CEmail
    {
        public static bool SendEmail(string addresseeEmail, string senderEmail, string senderPassword, string caption, string text)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(senderEmail, senderPassword);

            MailMessage mm = new MailMessage("bugreport.mailservice@gmail.com", addresseeEmail, caption, text);

            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            try
            {
                client.Send(mm);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
