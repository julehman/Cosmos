using Cosmos.Windows;
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
            SmtpClient client = new SmtpClient
            {
                Port = 587,
                Host = "smtp.gmail.com",
                EnableSsl = true,
                Timeout = 10000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(senderEmail, senderPassword)
            };

            MailMessage mm = new MailMessage(senderEmail, addresseeEmail, caption, text)
            {
                BodyEncoding = UTF8Encoding.UTF8,
                DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
            };

            try
            {
                client.Send(mm);
                return true;
            }
            catch (Exception ex)
            {
                CMessageBox message = new CMessageBox(ex.Message, "Error sending email", CColor.Theme.Red, CImage.ImageType.error_outline_black, CMessageBox.CMessageBoxButton.OK);
                message.ShowDialog();
                return false;
            }
        }
    }
}
