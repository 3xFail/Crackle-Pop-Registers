using SnapRegisters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Snap_Register_System_Interface.RegisterWindowParts.Business_Objects
{
    public class Email
    {
        private string sender_email = "snapregisters@gmail.com";
        //private string sender_email_pass = "snap_admin";

        Email()
        {

        }
        public Email(Customer receiver, RegisterMainWindow win)
        {
            if( receiver.email == null)
            {
                System.Windows.Forms.MessageBox.Show( "Invalid Email" );
            }
            else
            {
                SmtpClient client = new SmtpClient( );
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;              
                
                MailAddress from = new MailAddress( sender_email );
                MailAddress to = new MailAddress( receiver.email ); 
                MailMessage message = new MailMessage( from, to );
                message.Body = "This is a test e-mail message sent by an application.";
                message.Subject = "test message";
                client.Credentials = new NetworkCredential( sender_email, "snap_admin" );
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                try
                {
                    client.Send( message );
                }
                catch(Exception e)
                {
                    System.Windows.Forms.MessageBox.Show( e.ToString() );
                }
                message.Dispose();
            }
        }
    }
}
