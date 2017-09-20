using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Net.Mail;
using System.Net;

namespace Wave_Lab
{

    public partial class Contact : MetroWindow
    {
        public Contact()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        string GetString(RichTextBox rtb)
        {
            var textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            return textRange.Text;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            var fromAddress = new MailAddress("wavelabnoreply@gmail.com", "Wave Lab");
            var toAddress = new MailAddress("dm_fekih@esi.dz", "Mohamed Anis Fekih"); // anis Chef d'équipe
            const string fromPassword = "FacileADeviner";
            string subject = objet.Text;
            string body = GetString(contenu);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
            //handaoui
            toAddress = new MailAddress("dm_handaoui@esi.dz", "Handaoui Mohamed");
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
            //houda
            toAddress = new MailAddress("dh_bouchaour@esi.dz", "houda BOUCHAOUR");
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
            //ahlem
            toAddress = new MailAddress("da_aissaoui@esi.dz", "ahlam AISSAOUI");
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}
