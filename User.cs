using System;
using System.Collections.Generic;
using System.Text;

namespace AuthApp
{
    public class User
    {
        public User()
        {
        }

        public User(string FirstNameVal, string LastNameVal, string PasswordVal, List<User> users)
        {
            FirstName = FirstNameVal;
            LastName = LastNameVal;
            Password = PasswordVal;
            SetUsername(users);
        }

        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String UserName { get; set;  }
        public String Password { get; set; }

        public void SetUsername(List<User> users)
        {
            string newUserName = FirstName.Substring(0, 2) + LastName.Substring(0, 2);
            foreach (User user in users)
            {
                while (user.UserName == newUserName)
                {
                    Random random = new Random();
                    newUserName = newUserName + random.Next(0, 99);
                }
            }
            UserName = newUserName.Replace(" ","_");
        }

        public void Details()
        {
            Console.WriteLine("");
            Console.WriteLine($"Name        : {FirstName} {LastName}");
            Console.WriteLine($"User Name   : {UserName}");
            Console.WriteLine($"Password    : {Password}");
        }

        public bool Validate(string firstname, string lastname, string password)
        {
            if (!String.IsNullOrWhiteSpace(firstname) &&
                !String.IsNullOrWhiteSpace(lastname) &&
                !String.IsNullOrWhiteSpace(password) &&
                firstname.Length >= 2 &&
                lastname.Length >= 2)
            {   
                return true;
            }
            else
            {
                return false;
            }
            
        }


    }
}
