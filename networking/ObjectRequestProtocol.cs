using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CZbor.model;
using CZbor.service;

namespace CZbor.networking
{


    public interface Request
    {
    }


    [Serializable]
    public class LoginRequest : Request
    {
        private Angajat angajat;

        public LoginRequest(Angajat angajat)
        {
            this.angajat = angajat;
        }

        public virtual Angajat Angajat
        {
            get
            {
                return angajat;
            }
        }
        
    }

    [Serializable]
    public class LogoutRequest : Request
    {
        private Angajat angajat;

        public LogoutRequest(Angajat angajat)
        {
            this.angajat = angajat;

        }

        public virtual Angajat Angajat
        {
            get
            {
                return angajat;
            }
        }
    }

    [Serializable]
    public class GetZboruriRequest : Request
    {

        public GetZboruriRequest()
        {
        }

    }

    [Serializable]
    public class FilterZboruriRequest : Request
    {
        private string destinatie;
        private DateTime data;

        public FilterZboruriRequest(string destinatie, DateTime data)
        {
            this.destinatie = destinatie;
            this.data = data;
        }
        public virtual string Destinatie
        {
            get
            {
                return destinatie;
            }
        }
        public virtual DateTime Data
        {
            get
            {
                return data;
            }
        }

    }

    [Serializable]
    public class FindZborRequest : Request
    {
        private int id;

        public FindZborRequest(int id)
        {
            this.id = id;
        }
        public virtual int ID
        {
            get
            {
                return id;
            }
        }

    }

    [Serializable]
    public class FindTuristRequest : Request
    {
        private string nume;

        public FindTuristRequest(string nume)
        {
            this.nume= nume;
        }
        public virtual string Nume
        {
            get
            {
                return nume;
            }
        }

    }

    [Serializable]
    public class SaveTuristRequest : Request
    {
        private Turist turist;

        public SaveTuristRequest(Turist turist)
        {
            this.turist = turist;
        }
        public virtual Turist Turist
        {
            get
            {
                return turist;
            }
        }

    }
    [Serializable]
    public class SaveBiletRequest : Request
    {
        private Bilet bilet;

        public SaveBiletRequest(Bilet bilet)
        {
            this.bilet = bilet;
        }
        public virtual Bilet Bilet
        {
            get
            {
                return bilet;
            }
        }

    }
    [Serializable]
    public class UpdateZborRequest : Request
    {
        private Zbor zbor;

        public UpdateZborRequest(Zbor zbor)
        {
            this.zbor = zbor;
        }
        public virtual Zbor Zbor
        {
            get
            {
                return zbor;
            }
        }

    }


    [Serializable]
    public class SetObsRequest :Request
    {
        private Angajat angajat;

        public SetObsRequest(Angajat angajat)
        {
            this.angajat = angajat;
        }
        public virtual Angajat Angajat
        {
            get
            {
                return angajat;
            }
        }
    }


}
