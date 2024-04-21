using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CZbor.model;
using CZbor.repository;
using log4net;

namespace CZbor.repository
{
    public class TuristRepo : IRepository<int, Turist>
    {
        private static readonly ILog log = LogManager.GetLogger("Tourist Repository");

        IDictionary<String, string> props;

        public TuristRepo(IDictionary<String, string> props)
        {
            log.Info("Creating TouristRepository ");
            this.props = props;
        }


        public Turist findOne(int id)
        {
            log.InfoFormat("Finding Tourist by ID: {0}", id);
            IDbConnection con = DBUtils.getConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM turist WHERE id=@id; ";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);

                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        string name = dataR.GetString(1);
                        Turist tourist = new Turist(name);
                        tourist.Id = id;
                        log.InfoFormat("Found Tourist: {0}", tourist);
                        return tourist;

                    }
                }
            }
            log.InfoFormat("Tourist not found with ID: {0}", id);
            return null;
        }

        public Turist findOneByName(string name)
        {
            //find tourist by name
            log.InfoFormat("Finding Tourist by Name: {0}", name);
            IDbConnection con = DBUtils.getConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM turist WHERE nume=@name; ";
                IDbDataParameter paramName = comm.CreateParameter();
                paramName.ParameterName = "@name";
                paramName.Value = name;
                comm.Parameters.Add(paramName);

                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        int id = dataR.GetInt32(0);
                        Turist tourist = new Turist(name);
                        tourist.Id = id;
                        log.InfoFormat("Found Tourist: {0}", tourist);
                        return tourist;

                    }
                }

            }
            log.InfoFormat("Tourist not found with nume: {0}", name);
            return null;
        }

        public IEnumerable<Turist> findAll()
        {
            log.InfoFormat("Finding All Tourists");
            IDbConnection con = DBUtils.getConnection(props);
            IList<Turist> tourists = new List<Turist>();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM turist; ";

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        int id = dataR.GetInt32(0);
                        string name = dataR.GetString(1);
                        Turist tourist = new Turist(name);
                        tourist.Id = id;
                        tourists.Add(tourist);

                    }
                }
            }
            return tourists;
        }

        public void save(Turist entity)
        {
            log.InfoFormat("Saving Tourist: {0}", entity);

            var connection = DBUtils.getConnection(props);

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO turist (nume) VALUES (@name);";

                var name = command.CreateParameter();
                name.ParameterName = "@name";
                name.Value = entity.TouristName;
                command.Parameters.Add(name);

                var result = command.ExecuteNonQuery();
                if (result == 0)
                {
                    log.InfoFormat("Tourist {0} NOT saved", entity);
                    throw new Exception("No tourist added!");

                }
                log.InfoFormat("Tourist {0} saved", entity);

            }
        }

        public void delete(int id)
        {
            log.InfoFormat("Deleting Tourist with ID: {0}", id);
            IDbConnection connection = DBUtils.getConnection(props);
            using (var comm = connection.CreateCommand())
            {
                comm.CommandText = "DELETE FROM turist WHERE id=@id;";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);
                var dataR = comm.ExecuteNonQuery();
                if (dataR == 0)
                {
                    log.InfoFormat("Tourist with ID {0} NOT deleted", id);
                    throw new Exception("No deleted tourist!");
                }
            }

        }

        public void update(Turist entity)
        {
            log.InfoFormat("Updating Flight with ID: {0}", entity.Id);
            IDbConnection connection = DBUtils.getConnection(props);
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE turist SET nume=@name WHERE id=@id;";
                IDbDataParameter name = command.CreateParameter();
                name.ParameterName = "@name";
                name.Value = entity.TouristName;
                command.Parameters.Add(name);

                IDbDataParameter id = command.CreateParameter();
                id.ParameterName = "@id";
                id.Value = entity.Id;
                command.Parameters.Add(id);

                var result = command.ExecuteNonQuery();
                if (result == 0)
                {
                    log.InfoFormat("Tourist {0} NOT updated", entity);
                    throw new Exception("Tourist NOT updated");
                }
            }
        }
    }
}
