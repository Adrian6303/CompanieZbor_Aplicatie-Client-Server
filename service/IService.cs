using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CZbor.model;

namespace CZbor.service
{
    public interface IService
    {
        IEnumerable<Zbor> FindAllAvailableFlights();
        Angajat FindAngajat(string username, string password,IObserver client);
        IEnumerable<Zbor> FindZborByDestinatieAndDate(string destinatie, DateTime data);
        Turist findOrAddTurist(string name);
        void SaveBilet(Bilet bilet);
        void UpdateZbor(Zbor zbor);
        void Logout(Angajat angajat);
        void SetObsForm(Angajat angajat,IObserver client);
    }
}
