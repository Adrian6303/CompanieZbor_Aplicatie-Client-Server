using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CZbor.model;
using CZbor.repository;

namespace CZbor.service
{
    public class ServerImpl : IService
    {
        private ZborRepo zborRepo;
        private AngajatRepo angajatRepo;
        private BiletRepo biletRepo;
        private TuristRepo turistRepo;
        private readonly IDictionary<String, IObserver> loggedClients;


        public ServerImpl(AngajatRepo angajatRepo, ZborRepo zborRepo, TuristRepo turistRepo, BiletRepo biletRepo)
        {
            this.angajatRepo = angajatRepo;
            this.zborRepo = zborRepo;
            this.turistRepo = turistRepo;
            this.biletRepo = biletRepo;
            loggedClients = new Dictionary<string, IObserver>();
        }
        public IEnumerable<Zbor> FindAllZboruri()
        {
            return zborRepo.findAll();
        }

        public IEnumerable<Angajat> FindAllAngajati()
        {
            return angajatRepo.findAll();
        }

        public IEnumerable<Turist> FindAllTuristi()
        {
            return turistRepo.findAll();
        }

        public IEnumerable<Bilet> FindAllBilete()
        {
            return biletRepo.findAll();
        }
        public IEnumerable<Zbor> FindAllAvailableFlights()
        {
            List<Zbor> zboruri = zborRepo.findAll().ToList();
            List<Zbor> zboruriDisponibile = new List<Zbor>();

            foreach (Zbor zbor in zboruri)
            {
                if (zbor.NoTotalSeats > 0)
                {
                    zboruriDisponibile.Add(zbor);
                }
            }
            return zboruriDisponibile;
        }


        public IEnumerable<Zbor> FindZborByDestinatieAndDate(string destinatie, DateTime data)
        {
            List<Zbor> zboruri = zborRepo.findAll().ToList();
            List<Zbor> zboruriFiltrate = new List<Zbor>();

            foreach (Zbor zbor in zboruri)
            {
                DateTime dataZbor = data.Date;


                if (zbor.Destination.Equals(destinatie) && zbor.Date.Date.Equals(dataZbor) && zbor.NoTotalSeats > 0)
                {
                    zboruriFiltrate.Add(zbor);
                }
            }
            return zboruriFiltrate;
        }

        public Zbor FindZborById(int id)
        {
            return zborRepo.findOne(id);
        }

        public Turist FindTuristByName(string name)
        {
            return turistRepo.findOneByName(name);
        }

        public void SaveBilet(Bilet bilet)
        {
            biletRepo.save(bilet);
        }

        public void SaveZbor(Zbor zbor)
        {
            zborRepo.save(zbor);
        }

        public void SaveAngajat(Angajat angajat)
        {
            angajatRepo.save(angajat);
        }

        public void SaveTurist(Turist turist)
        {
            turistRepo.save(turist);

        }

        public void UpdateZbor(Zbor zbor)
        {
            zborRepo.update(zbor);
            notifyObservers();
        }
        public void notifyObservers()
        {
            foreach (var entry in loggedClients)
            {
                try
                {
                    List<Zbor> zboruri = FindAllAvailableFlights().ToList();

                    entry.Value.updateZbor();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public Angajat FindAngajat(string username, string password, IObserver client)
        {
            Angajat angajat = angajatRepo.findOneUserPass(username, password);
            if (angajat != null)
            {
                if (loggedClients.ContainsKey(angajat.Username))
                    throw new Exception("User already logged in.");
                loggedClients[angajat.Username] = client;
                return angajat;
            }
            else
            {
                throw new Exception("Authentication failed.");
            }
        }

        public void Logout(Angajat angajat)
        {
            foreach (var entry in loggedClients)
            {
                if (entry.Key == angajat.Username)
                {
                    loggedClients.Remove(entry.Key);
                    return;
                }
            }
        }

        public void SetObsForm(Angajat angajat,IObserver client)
        {
            foreach (var entry in loggedClients)
            {
                if (entry.Key == angajat.Username)
                {
                    loggedClients[angajat.Username] = client;
                    return;
                }
            }
        }
    }
}
