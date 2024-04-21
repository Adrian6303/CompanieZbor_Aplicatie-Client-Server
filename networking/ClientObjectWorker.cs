using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CZbor.service;
using CZbor.model;
using System.IO;


namespace CZbor.networking
{
    public class ClientWorker : IObserver
    {
        private IService server;
        private TcpClient connection;

        private NetworkStream stream;
        private IFormatter formatter;
        private volatile bool connected;
        public ClientWorker(IService server, TcpClient connection)
        {
            this.server = server;
            this.connection = connection;
            try
            {

                stream = connection.GetStream();
                formatter = new BinaryFormatter();
                connected = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public virtual void run()
        {
            while (connected)
            {
                try
                {
                    object request = formatter.Deserialize(stream);
                    
                    object response = handleRequest((Request)request);
                    if (response != null)
                    {
                        sendResponse((Response)response);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }

                try
                {
                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
            try
            {
                stream.Close();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error " + e);
            }
        }

        public void updateZbor()
        {
            Response resp = new UpdateZborResponse();
            Console.WriteLine("Update zbor  ...");
            try
            {
                sendResponse(resp);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        

        private Response handleRequest(Request request)
        {
            Response response = null;
            if (request is LoginRequest)
            {
                Console.WriteLine("Login request ...");
                LoginRequest logReq = (LoginRequest)request;
                Angajat angajat = logReq.Angajat;
               
                try
                {
                    Angajat ang= null;
                    lock (server)
                    {
                        ang = server.FindAngajat(angajat.Username,angajat.Password, this);
                    }
                    return new LoginResponse(ang);
                }
                catch (Exception e)
                {
                    connected = false;
                    return new ErrorResponse(e.Message);
                }
            }
            if (request is LogoutRequest)
            {
                Console.WriteLine("Logout request ...");
                LogoutRequest logReq = (LogoutRequest)request;
                Angajat angajat = logReq.Angajat;

                try
                {
                    lock (server)
                    {
                        server.Logout(angajat);
                    }
                    return new OkResponse();
                }
                catch (Exception e)
                {
                    connected = false;
                    return new ErrorResponse(e.Message);
                }
            }
            if (request is GetZboruriRequest)
            {
                Console.WriteLine("GetZboruriRequest ...");
                GetZboruriRequest getReq = (GetZboruriRequest)request;
                List<Zbor> zboruri= new List<Zbor>();
                try
                {
                    lock (server)
                    {
                        zboruri = (List<Zbor>)server.FindAllAvailableFlights();
                    }
                    return new GetZboruriResponse(zboruri);
                }
                catch (Exception e)
                {
                    connected = false;
                    return new ErrorResponse(e.Message);
                }
            }

            if (request is FilterZboruriRequest)
            {
                Console.WriteLine("FilterZboruriRequest ...");
                FilterZboruriRequest getReq = (FilterZboruriRequest)request;
                List<Zbor> zboruri = new List<Zbor>();
                string destinatie=getReq.Destinatie;
                DateTime data=getReq.Data;
                try
                {
                    lock (server)
                    {
                        zboruri = (List<Zbor>)server.FindZborByDestinatieAndDate(destinatie,data);
                    }
                    return new FilterZboruriResponse(zboruri);
                }
                catch (Exception e)
                {
                    connected = false;
                    return new ErrorResponse(e.Message);
                }
            }
            if (request is FindZborRequest)
            {
                Console.WriteLine("FindZborRequest ...");
                FindZborRequest getReq = (FindZborRequest)request;
                int id = getReq.ID;
                try
                {
                    Zbor zbor=null;
                    lock (server)
                    {
                        zbor = (Zbor)server.FindZborById(id);
                    }
                    
                    return new FindZborResponse(zbor);
                }
                catch (Exception e)
                {
                    connected = false;
                    return new ErrorResponse(e.Message);
                }
            }
            if (request is FindTuristRequest)
            {
                Console.WriteLine("FindTuristRequest ...");
                FindTuristRequest getReq = (FindTuristRequest)request;
                string nume = getReq.Nume;
                try
                {
                    Turist turist = null;
                    lock (server)
                    {
                        turist = (Turist) server.FindTuristByName(nume);
                    }

                    return new FindTuristResponse(turist);
                }
                catch (Exception e)
                {
                    connected = false;
                    return new ErrorResponse(e.Message);
                }
            }
            if (request is SaveTuristRequest)
            {
                Console.WriteLine("SaveTuristRequest ...");
                SaveTuristRequest getReq = (SaveTuristRequest)request;
                Turist turist = getReq.Turist;
                try
                {
                    lock (server)
                    {
                        server.SaveTurist(turist);
                    }

                    return new OkResponse();
                }
                catch (Exception e)
                {
                    connected = false;
                    return new ErrorResponse(e.Message);
                }
            }
            if (request is SaveBiletRequest)
            {
                Console.WriteLine("SaveBiletRequest ...");
                SaveBiletRequest getReq = (SaveBiletRequest)request;
                Bilet bilet = getReq.Bilet;
                try
                {
                    lock (server)
                    {
                        server.SaveBilet(bilet);
                    }

                    return new OkResponse();
                }
                catch (Exception e)
                {
                    connected = false;
                    return new ErrorResponse(e.Message);
                }
            }
            if (request is UpdateZborRequest)
            {
                Console.WriteLine("UpdateZborRequest ...");
                UpdateZborRequest getReq = (UpdateZborRequest)request;
                Zbor zbor = getReq.Zbor;
                try
                {
                    lock (server)
                    {
                    server.UpdateZbor(zbor);
                    }

                    return new OkResponse();
                }
                catch (Exception e)
                {
                    connected = false;
                    return new ErrorResponse(e.Message);
                }
            }
            if (request is SetObsRequest)
            {
                Console.WriteLine("SetObsRequest ...");
                SetObsRequest getReq = (SetObsRequest)request;
                Angajat angajat = getReq.Angajat;
                try
                {
                    lock (server)
                    {
                        server.SetObsForm(angajat,this);
                    }

                    return new OkResponse();
                }
                catch (Exception e)
                {
                    connected = false;
                    return new ErrorResponse(e.Message);
                }
            }


            return response;
        }

        private void sendResponse(Response response)
        {
            Console.WriteLine("sending response " + response);
            lock (stream)
            {
                formatter.Serialize(stream, response);
                stream.Flush();
            }

        }

    }

}
