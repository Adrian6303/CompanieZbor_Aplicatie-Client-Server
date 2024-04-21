using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CZbor.model;

namespace CZbor.networking
{

    public interface Response
    {
    }

    [Serializable]
    public class OkResponse : Response
    {
        
    }

    [Serializable]
    public class ErrorResponse : Response
    {
        private string message;

        public ErrorResponse(string message)
        {
            this.message = message;
        }

        public virtual string Message
        {
            get
            {
                return message;
            }
        }
    }

    [Serializable]
    public class LoginResponse : Response
    {
        private Angajat angajat;

        public LoginResponse(Angajat angajat)
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
    public class GetZboruriResponse : Response
    {
        private List<Zbor> zboruri;

        public GetZboruriResponse(List<Zbor> zboruri)
        {
            this.zboruri = zboruri;
        }

        public virtual List<Zbor> Zboruri
        {
            get
            {
                return zboruri;
            }
        }
        
    }

    [Serializable]
    public class FilterZboruriResponse : Response
    {
        private List<Zbor> zboruri;

        public FilterZboruriResponse(List<Zbor> zboruri)
        {
            this.zboruri = zboruri;
        }

        public virtual List<Zbor> Zboruri
        {
            get
            {
                return zboruri;
            }
        }

    }
    [Serializable]
    public class FindZborResponse : Response
    {
        private Zbor zbor;

        public FindZborResponse(Zbor zbor)
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
    public class FindTuristResponse : Response
    {
        private Turist turist;

        public FindTuristResponse(Turist turist)
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
    public class UpdateZborResponse : Response
    {
        //private List<Zbor> zboruri;
        public UpdateZborResponse()
        {
           // this.zboruri = zboruri;
        }

       // public virtual List<Zbor> Zboruri
        //{
          //  get
            //{
              //  return zboruri;
           // }
        //}

    }


    public interface UpdateResponse : Response
    {
    }
    
}
