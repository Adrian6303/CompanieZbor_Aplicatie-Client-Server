using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZbor.model
{
    [Serializable]
    public class Angajat : Entity<int>
    {
        private string username;
        private string password;


        public Angajat(string username, string password)
        {
            this.username = username;
            this.password = password;
        }


        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Angajat other = (Angajat)obj;
            return username == other.username &&
                   password == other.password;
        }

        public override string ToString()
        {
            return username + " " + password + '\n';
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

    }
}
