using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CZbor.model;
using log4net;
using CZbor.repository;

namespace CZbor.repository
{
    public class BiletRepo : IRepository<int, Bilet>
    {
        private readonly AngajatRepo angajatRepository;
        private readonly ZborRepo zborRepository;
        private readonly TuristRepo touristRepository;
        private static readonly ILog log = LogManager.GetLogger("Trip Repository");
        IDictionary<String, string> props;

        public BiletRepo(IDictionary<String, string> props, AngajatRepo angajatRepo, ZborRepo zborRepo, TuristRepo touristRepository)
        {
            log.Info("Creating UserRepository ");
            this.props = props;
            this.angajatRepository = angajatRepo;
            this.zborRepository = zborRepo;
            this.touristRepository = touristRepository;

        }


        public Bilet findOne(int id)
        {
            log.InfoFormat("Finding Trip by ID: {0}", id);
            IDbConnection con = DBUtils.getConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM bilet WHERE id=@id; ";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);

                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        int zborID = dataR.GetInt32(1);
                        int touristID = dataR.GetInt32(2);
                        //IEnumerable < Turist > = null;
                        string adresaClient = dataR.GetString(4);
                        int nrLocuri = dataR.GetInt32(5);
                        int angajatID = dataR.GetInt32(6);

                        Angajat a = angajatRepository.findOne(angajatID);
                        Zbor z = zborRepository.findOne(zborID);
                        Turist t = touristRepository.findOne(touristID);

                        Bilet bilet = new Bilet(a, z, t, null, adresaClient, nrLocuri);
                        bilet.Id = id;

                        log.InfoFormat("Found Trip: {0}", bilet);
                        return bilet;

                    }
                }
            }
            log.InfoFormat("Trip not found with ID: {0}", id);
            return null;
        }

        public IEnumerable<Bilet> findAll()
        {
            log.InfoFormat("Finding All Trips");
            IDbConnection con = DBUtils.getConnection(props);
            IList<Bilet> trips = new List<Bilet>();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM bilet; ";

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        int id = dataR.GetInt32(0);
                        int zborID = dataR.GetInt32(1);
                        int touristID = dataR.GetInt32(2);
                        //IEnumerable < Turist > = null;
                        string adresaClient = dataR.GetString(4);
                        int nrLocuri = dataR.GetInt32(5);
                        int angajatID = dataR.GetInt32(6);

                        Angajat a = angajatRepository.findOne(angajatID);
                        Zbor z = zborRepository.findOne(zborID);
                        Turist t = touristRepository.findOne(touristID);

                        Bilet bilet = new Bilet(a, z, t, null, adresaClient, nrLocuri);
                        bilet.Id = id;

                        trips.Add(bilet);

                    }
                }
            }
            return trips;
        }

        public void save(Bilet entity)
        {
            log.InfoFormat("Saving Trip: {0}", entity);

            var connection = DBUtils.getConnection(props);

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO bilet(zbor, client,listaTuristi,adresaClient,nrLocuri,angajat) VALUES (@zbor, @client,@listaTuristi,@adresaClient,@nrLocuri,@angajat);";

                var ZborID = command.CreateParameter();
                ZborID.ParameterName = "@zbor";
                ZborID.Value = entity.Zbor.Id;
                command.Parameters.Add(ZborID);

                var clientID = command.CreateParameter();
                clientID.ParameterName = "@client";
                clientID.Value = entity.Angajat.Id;
                command.Parameters.Add(clientID);

                var listaTuristi = command.CreateParameter();
                listaTuristi.ParameterName = "@listaTuristi";
                foreach (Turist t in entity.ListaTuristi)
                {
                    listaTuristi.Value += t.TouristName + ";";
                }
                listaTuristi.Value = listaTuristi.Value.ToString().Substring(0, listaTuristi.Value.ToString().Length - 1);
                command.Parameters.Add(listaTuristi);

                var adresaClient = command.CreateParameter();
                adresaClient.ParameterName = "@adresaClient";
                adresaClient.Value = entity.AdresaClient;
                command.Parameters.Add(adresaClient);

                var nrLocuri = command.CreateParameter();
                nrLocuri.ParameterName = "@nrLocuri";
                nrLocuri.Value = entity.NrLocuri;
                command.Parameters.Add(nrLocuri);

                var angajatID = command.CreateParameter();
                angajatID.ParameterName = "@angajat";
                angajatID.Value = entity.Angajat.Id;
                command.Parameters.Add(angajatID);



                var result = command.ExecuteNonQuery();
                if (result == 0)
                {
                    log.InfoFormat("trip {0} NOT saved", entity);
                    throw new Exception("No trip added!");

                }
                log.InfoFormat("Trip {0} saved", entity);

            }

        }

        public void delete(int id)
        {
            log.InfoFormat("Deleting trip with ID: {0}", id);
            IDbConnection connection = DBUtils.getConnection(props);
            using (var comm = connection.CreateCommand())
            {
                comm.CommandText = "DELETE FROM bilet WHERE id=@id;";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);
                var dataR = comm.ExecuteNonQuery();
                if (dataR == 0)
                {
                    log.InfoFormat("Trip with ID {0} NOT deleted", id);
                    throw new Exception("No deleted trip!");
                }
            }
        }

        public void update(Bilet entity)
        {

        }
    }


}
