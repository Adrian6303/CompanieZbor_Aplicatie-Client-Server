using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZbor.model
{
    [Serializable]
    public class Zbor : Entity<int>
    {
        private string destinatia;
        private DateTime dataPlecarii;
        private string aeroportul;
        private int nrLocuri;

        public Zbor(string destination, DateTime date, string airport, int noSeats)
        {
            this.destinatia = destination;
            this.dataPlecarii = date;
            this.aeroportul = airport;
            this.nrLocuri = noSeats;
        }

        public string Destination
        {
            get { return destinatia; }
            set { destinatia = value; }
        }

        public DateTime Date
        {
            get { return dataPlecarii; }
            set { dataPlecarii = value; }
        }

        public string Airport
        {
            get { return aeroportul; }
            set { aeroportul = value; }
        }

        public int NoTotalSeats
        {
            get { return nrLocuri; }
            set { nrLocuri = value; }
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;

            if (obj == null || GetType() != obj.GetType())
                return false;

            Zbor flight = (Zbor)obj;
            return nrLocuri == flight.nrLocuri &&
                   destinatia == flight.destinatia &&
                   dataPlecarii == flight.dataPlecarii &&
                   aeroportul == flight.aeroportul;
        }

        public override string ToString()
        {
            return $"Flight to {destinatia} at {dataPlecarii} from {aeroportul}. Number of Seats={nrLocuri}";
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }

}
