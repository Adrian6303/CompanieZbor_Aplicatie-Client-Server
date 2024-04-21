using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZbor.model
{
    [Serializable]
    public class Turist : Entity<int>
    {
        private string nume;

        public Turist(string touristName)
        {
            this.nume = touristName;
        }

        public string TouristName
        {
            get { return nume; }
            set { nume = value; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Turist other = (Turist)obj;
            return nume == other.nume;
        }

        public override string ToString()
        {
            return nume;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
