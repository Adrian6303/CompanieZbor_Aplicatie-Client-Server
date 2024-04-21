/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CZbor.model;
using CZbor.service;

namespace CZbor.client
{
    public class ZboruriCtrl : IObserver
    {
        public event EventHandler<UserEventArgs> updateEvent; //ctrl calls it when it has received an update
        private readonly IService server;
        private Angajat currentAngajat;
        public ZboruriCtrl(IService server)
        {
            this.server = server;
            currentAngajat = null;
        }

        public void updateZbor()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Zbor> FindAllZboruri()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Angajat> FindAllAngajati()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Turist> FindAllTuristi()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Bilet> FindAllBilete()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Zbor> FindAllAvailableFlights()
        {
            return server.FindAllAvailableFlights();
        }

        public Angajat FindAngajat(string username, string password)
        {
            Angajat angajat=server.FindAngajat(username,password, this);
            Console.WriteLine("Login succeeded ....");
            currentAngajat = angajat;
            Console.WriteLine("Current user {0}", angajat);
            return angajat;
        }

        public IEnumerable<Zbor> FindZborByDestinatieAndDate(string destinatie, DateTime data)
        {

            return server.FindZborByDestinatieAndDate(destinatie, data).ToList();
        }

        public Zbor FindZborById(int id)
        {
            return server.FindZborById(id);
        }

        public Turist FindTuristByName(string name)
        {
           return server.FindTuristByName(name);
        }

        public void SaveBilet(Bilet bilet)
        {
            server.SaveBilet(bilet);
        }

        public void SaveZbor(Zbor zbor)
        {
            throw new NotImplementedException();
        }

        public void SaveAngajat(Angajat angajat)
        {
            throw new NotImplementedException();
        }

        public void SaveTurist(Turist turist)
        {
            server.SaveTurist(turist);
        }

        public void UpdateZbor(Zbor zbor)
        {
            server.UpdateZbor(zbor);
        }
        public void Logout(Angajat angajat)
        {
            server.Logout(angajat);
        }
    }
}
*/