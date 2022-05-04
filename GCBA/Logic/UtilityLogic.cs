using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;


namespace GCBA.Logic
{
    public class UtilityLogic
    {
        public void SendMail(string from, string to, string subject, string body)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(to));
            message.From = new MailAddress(from);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = ConfigurationManager.AppSettings["mailAccount"], // replace with valid value
                    Password = ConfigurationManager.AppSettings["mailPassword"] // replace with valid value
                };

                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = credential;
                smtp.Send(message);

            }
        }

        public string GetRandomPassword()
        {
            Random rand = new Random();
            int N = rand.Next(5, 10); //6 to 10 characters of password
            char[] passwordChar = new char[N];
            int alphabetCount = N - 4;
            int numberCount = 2;
            int symbolCount = 2;

            char[] alphabets = new char[]
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u',
                'v', 'w', 'x', 'y', 'z',
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
                'V', 'W', 'X', 'Y', 'Z',
            };
            char[] numbers = new char[] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
            char[] specialCharacters = new char[] {'.', ',', ';', '/', '!', '@', '#', '$', '%', '^', '&', '*', '|'};

            //getting the alphabets
            for (int i = 0; i < alphabetCount; i++)
            {
                int index = rand.Next(0, alphabets.Count() - 1);
                var myChar = alphabets[index];
                //choose upper or lower case randomly
                int toUpper = rand.Next(0, 1);
                if (toUpper == 1)
                    myChar = myChar.ToString().ToUpper()[0]; //converts the character to upper case
                passwordChar[i] = myChar;
            }

            //getting the numbers
            int charPosition = alphabetCount;
            for (int i = 0; i < numberCount; i++)
            {
                int index = rand.Next(0, numbers.Count() - 1);
                var n = numbers[index];
                passwordChar[charPosition] = n;
                charPosition++;
            }

            //getting the special characters

            charPosition = alphabetCount + numberCount;
            for (int i = 0; i < symbolCount; i++)
            {
                int index = rand.Next(0, specialCharacters.Count() - 1);
                var symb = specialCharacters[index];
                passwordChar[charPosition] = symb;
                charPosition++;
            }

            string password = new string(passwordChar);
            return password;
        }
        


    }
}