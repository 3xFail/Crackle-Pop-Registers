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
                client.Credentials = new NetworkCredential( sender_email, "snap_admin" );
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                

                MailMessage msg = new MailMessage();

                msg.From = new MailAddress( sender_email );
                msg.To.Add( new MailAddress( receiver.email ));

                msg.Body = "Hi " + win.m_customer.fname.ToString() + ",\n"; 
                msg.Body += "Thank you for using Snap's Crackle Pop registers!\nHere is your reciept for your transaction.\n\n";
                msg.Body += '\t' + "Name".PadRight(20, ' ') + "Original Price   " + "Final Price" + '\n';

                foreach (Item item in win.m_transaction.m_Items)
                {
                    //this line is so ugly but so good
                    msg.Body += '\t' + item.ItemName.Substring(0, Math.Min(20, item.ItemName.Length)).PadRight(24, ' ') + item.OriginalPrice.ToString("C").PadRight(12, ' ') + "   " + item.Price.ToString("C") + '\n';
                }

                msg.Body += "\t\t\nTotal before discounts:  " + win.m_costTotal.ToString("C")
                    + "\n\t\t\nTotal discounts:            " + win.m_savingsTotal.ToString( "C" )
                    + "\n\t\t\nTotal after discounts:     " + win.m_totalTotal.ToString( "C" )
                    + '\n';

                msg.Subject = "Snap Registers Order: " + DateTime.UtcNow.ToString("d");
                
                try
                {
                    client.Send( msg );
                }
                catch(Exception e)
                {
                    System.Windows.Forms.MessageBox.Show( e.ToString() );
                }
                msg.Dispose();
            }
        }
    }
}
