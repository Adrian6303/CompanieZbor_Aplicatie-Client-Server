using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;

using CZbor.repository;
using CZbor.service;
using CZbor.networking;
using ServerTemplate;

namespace CZbor.server
{
    class StartServer
    {
        private static int DEFAULT_PORT = 55556;
        private static String DEFAULT_IP = "127.0.0.1";
        static void Main(string[] args)
        {

            Console.WriteLine("Reading properties from app.config ...");
            int port = DEFAULT_PORT;
            String ip = DEFAULT_IP;
            String portS = ConfigurationManager.AppSettings["port"];
            if (portS == null)
            {
                Console.WriteLine("Port property not set. Using default value " + DEFAULT_PORT);
            }
            else
            {
                bool result = Int32.TryParse(portS, out port);
                if (!result)
                {
                    Console.WriteLine("Port property not a number. Using default value " + DEFAULT_PORT);
                    port = DEFAULT_PORT;
                    Console.WriteLine("Portul " + port);
                }
            }
            String ipS = ConfigurationManager.AppSettings["ip"];

            if (ipS == null)
            {
                Console.WriteLine("Port property not set. Using default value " + DEFAULT_IP);
            }
            Console.WriteLine("Configuration Settings for database {0}", GetConnectionStringByName("zboruriDB"));
            IDictionary<String, string> props = new SortedList<String, String>();
            props.Add("ConnectionString", GetConnectionStringByName("zboruriDB"));
            AngajatRepo angajatRepo = new AngajatRepo(props);
            TuristRepo turistRepo = new TuristRepo(props);
            ZborRepo zborRepo = new ZborRepo(props);
            BiletRepo biletRepo = new BiletRepo(props,angajatRepo,zborRepo,turistRepo);
            IService serviceImpl = new ServerImpl(angajatRepo, zborRepo, turistRepo,biletRepo);


            Console.WriteLine("Starting server on IP {0} and port {1}", ip, port);
            SerialServer server = new SerialServer(ip, port, serviceImpl);
            server.Start();
            Console.WriteLine("Server started ...");
            //Console.WriteLine("Press <enter> to exit...");
            Console.ReadLine();

        }



        static string GetConnectionStringByName(string name)
        {
            // Assume failure.
            string returnValue = null;

            // Look for the name in the connectionStrings section.
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[name];

            // If found, return the connection string.
            if (settings != null)
                returnValue = settings.ConnectionString;

            return returnValue;
        }
    }

    public class SerialServer : ConcurrentServer
    {
        private IService server;
        private ClientWorker worker;
        public SerialServer(string host, int port, IService server) : base(host, port)
        {
            this.server = server;
            Console.WriteLine("SerialServer...");
        }
        protected override Thread createWorker(TcpClient client)
        {
            worker = new ClientWorker(server, client);
            return new Thread(new ThreadStart(worker.run));
        }
    }
}
