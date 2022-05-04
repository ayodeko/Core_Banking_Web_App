using System;
using System.Web.UI.WebControls;
using System.Net.Mail;

public partial class SendMail : System.Web.UI.Page
{
    public void btn_SendMessage_Click(object sender, EventArgs e)
    {
        SmtpClient smtpClient = new SmtpClient("domain.a2hosted.com", 25);

        smtpClient.Credentials = new System.Net.NetworkCredential("user@example.com", "password");
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

        MailMessage mailMessage = new MailMessage("akindekoayooluwa@gmail.com", "akindekoayooluwa@gmail.com");
        mailMessage.Subject = "Login Details";
        mailMessage.Body = "Kaipaichumarimarichopako";

       /* try
        {
            smtpClient.Send(mailMessage);
            Label1.Text = "Message sent";
        }
        catch (Exception ex)
        {
            Label1.Text = ex.ToString();
        }*/
    }
}