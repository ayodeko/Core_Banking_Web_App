using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GCBA.Logic
{
    public class UserLogic
    {
        public void SendPasswordToUser(string fullName, string toMail, string username, string password)
        {
            string fromEmail = ConfigurationManager.AppSettings["mailAccount"];
            var bodyFormat = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p> <h2>Thanks and have a nice day</h2>";
            string msgBody = string.Format(bodyFormat, "put your name", fromEmail, "Dear " + fullName + ", an account has been created for you in this Core Banking App (God'sCBA). Your username is \"" + username + "\" and your password is: \"" + password + "\". Please keep safely these details as they will be required of you to access the application");
            new UtilityLogic().SendMail(fromEmail, toMail, "Your Sign In Details", msgBody);
        } 
    }

}