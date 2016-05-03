using SnapRegisters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Snap_Register_System_Interface.RegisterWindowParts.Business_Objects
{
    public class Email
    {
        private string sender_email = "snapregisters@gmail.com";

        Email()
        {

        }
        public Email(Customer receiver, RegisterMainWindow win)
        {
            if( receiver.email.Length < 8)
            {
                System.Windows.Forms.MessageBox.Show( "Invalid Email" );
            }
            else
            {
                SmtpClient client = new SmtpClient( );
                client.Port = 35;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = "smtp.gmail.com";
                // Specify the e-mail sender.
                // Create a mailing address that includes a UTF8 character
                // in the display name.
                MailAddress from = new MailAddress( sender_email,
                   "Snap " + (char)0xD8 + " Registers",
                System.Text.Encoding.UTF8 );
                // Set destinations for the e-mail message.
                MailAddress to = new MailAddress( receiver.email ); //reciever.email
                // Specify the message content.
                MailMessage message = new MailMessage( from, to );
                message.Body = "This is a test e-mail message sent by an application. ";
                // Include some non-ASCII characters in body and subject.
                string someArrows = new string( new char[] { '\u2190', '\u2191', '\u2192', '\u2193' } );
                message.Body += Environment.NewLine + someArrows;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.Subject = "test message " + someArrows;
                message.SubjectEncoding = System.Text.Encoding.UTF8;
               
                string userState = "test message1";
                client.SendAsync( message, userState );
                message.Dispose();
            }
        }
    }
}
