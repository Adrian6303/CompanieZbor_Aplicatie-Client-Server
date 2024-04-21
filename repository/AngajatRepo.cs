using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using CZbor.model;

namespace CZbor.repository
{
    public class AngajatRepo : IRepository<int, Angajat>
    {
        private static readonly ILog log = LogManager.GetLogger("Angajat Repository");
        IDictionary<String, string> props;

        public AngajatRepo(IDictionary<String, string> props)
        {
            log.Info("Creating AngajatRepository ");
            this.props = props;
        }

        public Angajat findOne(int id)
        {
            log.InfoFormat("Finding Angajat by ID: {0}", id);
            IDbConnection con = DBUtils.getConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM angajat WHERE id=@id; ";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);

                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        string username = dataR.GetString(1);
                        string password = dataR.GetString(2);
                        Angajat u = new Angajat(username, password);
                        u.Id = id;
                        log.InfoFormat("Found Angajat: {0}", u);
                        return u;

                    }
                }
            }
            log.InfoFormat("Angajat not found with ID: {0}", id);
            return null;
        }

        public Angajat findOneUserPass(string user, string password)
        {
            log.InfoFormat("Finding Angajat by user: {0} , and pass {1} ", user, password);
            IDbConnection con = DBUtils.getConnection(props);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM angajat WHERE user=@user and password=@password; ";
                IDbDataParameter paramUser = comm.CreateParameter();
                paramUser.ParameterName = "@user";
                paramUser.Value = user;
                comm.Parameters.Add(paramUser);

                IDbDataParameter paramPass = comm.CreateParameter();
                paramPass.ParameterName = "@password";
                paramPass.Value = password;
                comm.Parameters.Add(paramPass);


                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        int id = dataR.GetInt32(0);
                        Angajat u = new Angajat(user, password);
                        u.Id = id;
                        log.InfoFormat("Found Angajat: {0}", u);
                        return u;

                    }
                }
            }
            log.InfoFormat("Angajat not found with user: {0} , and pass {1} ", user, password);
            return null;
        }

        public IEnumerable<Angajat> findAll()
        {
            log.InfoFormat("Finding All Angajati");
            IDbConnection con = DBUtils.getConnection(props);
            IList<Angajat> users = new List<Angajat>();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM angajat; ";

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        int id = dataR.GetInt32(0);
                        string username = dataR.GetString(1);
                        string password = dataR.GetString(2);
                        Angajat u = new Angajat(username, password);
                        u.Id = id;
                        users.Add(u);
                    }
                }
            }
            return users;
        }

        public void save(Angajat entity)
        {
            log.InfoFormat("Saving Angajat: {0}", entity);

            var connection = DBUtils.getConnection(props);

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO angajat (user, password) VALUES (@username, @password);";

                var username = command.CreateParameter();
                username.ParameterName = "@username";
                username.Value = entity.Username;
                command.Parameters.Add(username);

                var password = command.CreateParameter();
                password.ParameterName = "@password";
                password.Value = entity.Password;
                command.Parameters.Add(password);

                var result = command.ExecuteNonQuery();
                if (result == 0)
                {
                    log.InfoFormat("Angajat {0} NOT saved", entity);
                    throw new Exception("No angajat added!");

                }
                log.InfoFormat("Angajat{0} saved", entity);

            }
        }

        public void delete(int id)
        {
            log.InfoFormat("Deleting Angajat with ID: {0}", id);
            IDbConnection connection = DBUtils.getConnection(props);
            using (var comm = connection.CreateCommand())
            {
                comm.CommandText = "DELETE FROM angajat WHERE id=@id;";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);
                var dataR = comm.ExecuteNonQuery();
                if (dataR == 0)
                {
                    log.InfoFormat("Angajat with ID {0} NOT deleted", id);
                    throw new Exception("No deleted angajat!");
                }
            }
        }

        public void update(Angajat entity)
        {

        }
    }

}
