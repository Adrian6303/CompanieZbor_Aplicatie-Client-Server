using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CZbor.model;
using CZbor.repository;
using log4net;

namespace CZbor.repository
{
    public class ZborRepo : IRepository<int, Zbor>
    {
        private static readonly ILog log = LogManager.GetLogger("ZborRepository");

        IDictionary<String, string> props;

        public ZborRepo(IDictionary<String, string> props)
        {
            log.Info("Creating FlightRepository ");
            this.props = props;
        }

        public Zbor findOne(int id)
        {
            log.InfoFormat("Finding Flight by ID: {0}", id);
            IDbConnection con = DBUtils.getConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM zbor WHERE id=@id; ";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);

                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        string destination = dataR.GetString(1);
                        string d = dataR.GetString(2);
                        DateTime date = DateTime.ParseExact(d, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        string airport = dataR.GetString(3);
                        int noSeats = dataR.GetInt32(4);
                        Zbor flight = new Zbor(destination, date, airport, noSeats);
                        flight.Id = id;
                        log.InfoFormat("Found Flight: {0}", flight);
                        return flight;

                    }
                }
            }
            log.InfoFormat("Flight not found with ID: {0}", id);
            return null;
        }

        public IEnumerable<Zbor> findAll()
        {
            log.InfoFormat("Finding All Flights");
            IDbConnection con = DBUtils.getConnection(props);
            IList<Zbor> flights = new List<Zbor>();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM zbor; ";

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        int id = dataR.GetInt32(0);
                        string destination = dataR.GetString(1);
                        string d = dataR.GetString(2);
                        DateTime date = DateTime.ParseExact(d, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        string airport = dataR.GetString(3);
                        int noSeats = dataR.GetInt32(4);
                        Zbor flight = new Zbor(destination, date, airport, noSeats);
                        flight.Id = id;
                        flights.Add(flight);

                    }
                }
            }
            return flights;
        }

        public void save(Zbor entity)
        {
            log.InfoFormat("Saving Flight: {0}", entity);

            var connection = DBUtils.getConnection(props);

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO zbor (destinatia, dataPlecarii, aeroportul, nrLocuri) VALUES (@destination, @date, @airport, @noTotalSeats);";

                var destination = command.CreateParameter();
                destination.ParameterName = "@destination";
                destination.Value = entity.Destination;
                command.Parameters.Add(destination);

                var date = command.CreateParameter();
                date.ParameterName = "@date";
                date.Value = entity.Date.ToString();
                command.Parameters.Add(date);

                var airport = command.CreateParameter();
                airport.ParameterName = "@airport";
                airport.Value = entity.Airport;
                command.Parameters.Add(airport);

                var noSeats = command.CreateParameter();
                noSeats.ParameterName = "@noTotalSeats";
                noSeats.Value = entity.NoTotalSeats;
                command.Parameters.Add(noSeats);

                var result = command.ExecuteNonQuery();
                if (result == 0)
                {
                    log.InfoFormat("Flight {0} NOT saved", entity);
                    throw new Exception("No flight added!");

                }
                log.InfoFormat("Flight {0} saved", entity);

            }

        }

        public void delete(int id)
        {
            log.InfoFormat("Deleting Flight with ID: {0}", id);
            IDbConnection connection = DBUtils.getConnection(props);
            using (var comm = connection.CreateCommand())
            {
                comm.CommandText = "DELETE FROM zbor WHERE id=@id;";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);
                var dataR = comm.ExecuteNonQuery();
                if (dataR == 0)
                {
                    log.InfoFormat("Flight with ID {0} NOT deleted", id);
                    throw new Exception("No deleted flight!");
                }
            }

        }

        public void update(Zbor entity)
        {
            log.InfoFormat("Updating Flight with ID: {0}", entity.Id);
            IDbConnection connection = DBUtils.getConnection(props);
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE zbor SET nrLocuri=@nrLocuri WHERE id=@id;";


                IDbDataParameter nrLocuri = command.CreateParameter();
                nrLocuri.ParameterName = "@nrLocuri";
                nrLocuri.Value = entity.NoTotalSeats;
                command.Parameters.Add(nrLocuri);

                IDbDataParameter id = command.CreateParameter();
                id.ParameterName = "@id";
                id.Value = entity.Id;
                command.Parameters.Add(id);


                var result = command.ExecuteNonQuery();
                if (result == 0)
                {
                    log.InfoFormat("Flight {0} NOT updated", entity);
                    throw new Exception("Flight NOT updated");
                }
            }
        }
    }
}
