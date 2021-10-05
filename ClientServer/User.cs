using System;
using System.Collections.Generic;
using System.Text;

namespace ClientServer
{
    [Serializable]
    public class User
    {
        public string Login { get; }
        public string Password { get; }

        public User()
        {

        }
        public User(string login,string password)
        {
            Login = login;
            Password = password;
        }

        public override string ToString()
        {
            return this.Login + " " + this.Password;
        }

    }
}
