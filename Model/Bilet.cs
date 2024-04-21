using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZbor.model
{
    [Serializable]
    public class Bilet : Entity<int>
    {
        private Angajat angajat;
        private Zbor zbor;
        private Turist turist;
        private List<Turist> listaTuristi;
        private string adresaClient;
        private int nrLocuri;

        public Bilet(Angajat angajat, Zbor zbor, Turist turist, List<Turist> listaTuristi, string adresaClient, int nrLocuri)
        {
            this.angajat = angajat;
            this.zbor = zbor;
            this.turist = turist;
            this.listaTuristi = listaTuristi;
            this.adresaClient = adresaClient;
            this.nrLocuri = nrLocuri;
        }


        public Angajat Angajat
        {
            get { return angajat; }
            set { angajat = value; }
        }

        public Zbor Zbor
        {
            get { return zbor; }
            set { zbor = value; }
        }

        public Turist Turist
        {
            get { return turist; }
            set { turist = value; }
        }

        public List<Turist> ListaTuristi
        {
            get { return listaTuristi; }
            set { listaTuristi = value; }
        }

        public string AdresaClient
        {
            get { return adresaClient; }
            set { adresaClient = value; }
        }

        public int NrLocuri
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

            Bilet ticket = (Bilet)obj;
            return nrLocuri == ticket.nrLocuri &&
                   angajat == ticket.angajat &&
                   zbor == ticket.zbor &&
                   turist == ticket.turist &&
                   listaTuristi == ticket.listaTuristi &&
                   adresaClient == ticket.adresaClient;
        }

        public override string ToString()
        {
            return $"Ticket for {turist} at {zbor} from {angajat}. Number of Seats={nrLocuri}";
        }

        public override int GetHashCode()
        {
            return nrLocuri.GetHashCode();
        }
    }

}
