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
            Response resp = new Response.Builder().Type(ResponseType.UPDATE_ZBOR).Build();
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
            if (request.Type is RequestType.LOGIN)
            {
                Console.WriteLine("Login request ...");
                Angajat angajat = (Angajat)request.Data;
               
                try
                {
                    Angajat ang= null;
                    lock (server)
                    {
                        ang = server.FindAngajat(angajat.Username,angajat.Password, this);
                    }
                    return new Response.Builder().Type(ResponseType.OK).Data(ang).Build();
                }
                catch (Exception e)
                {
                    connected = false;
                    return new Response.Builder().Type(ResponseType.ERROR).Build();
                }
            }
            if (request.Type is RequestType.LOGOUT)
            {
                Console.WriteLine("Logout request ...");

                Angajat angajat = (Angajat)request.Data;

                try
                {
                    lock (server)
                    {
                        server.Logout(angajat);
                    }
                    return new Response.Builder().Type(ResponseType.OK).Build();
                }
                catch (Exception e)
                {
                    connected = false;
                    return new Response.Builder().Type(ResponseType.ERROR).Build();
                }
            }
            if (request.Type is RequestType.GET_ZBORURI)
            {
                Console.WriteLine("GetZboruriRequest ...");
                List<Zbor> zboruri= new List<Zbor>();
                try
                {
                    lock (server)
                    {
                        zboruri = (List<Zbor>)server.FindAllAvailableFlights();
                    }
                    return new Response.Builder().Type(ResponseType.GET_ZBORURI).Data(zboruri).Build(); ;
                }
                catch (Exception e)
                {
                    connected = false;
                    return new Response.Builder().Type(ResponseType.ERROR).Build();
                }
            }

            if (request.Type is RequestType.FILTER_ZBORURI)
            {
                Console.WriteLine("FilterZboruriRequest ...");
                List<Zbor> zboruri = new List<Zbor>();
                Zbor zbor = (Zbor)request.Data;
                string destinatie=zbor.Destination;
                DateTime data=zbor.Date;
                try
                {
                    lock (server)
                    {
                        zboruri = (List<Zbor>)server.FindZborByDestinatieAndDate(destinatie,data);
                    }
                    return new Response.Builder().Type(ResponseType.FILTER_ZBORURI).Data(zboruri).Build(); ;
                }
                catch (Exception e)
                {
                    connected = false;
                    return new Response.Builder().Type(ResponseType.ERROR).Build(); ;
                
                }
            }
            if (request.Type is RequestType.FIND_ADD_TURIST)
            {
                Console.WriteLine("FindAddTuristRequest ...");
                string nume = (string)request.Data;
                try
                {
                    Turist turist = null;
                    lock (server)
                    {
                        turist = (Turist) server.findOrAddTurist(nume);
                    }

                    return new Response.Builder().Type(ResponseType.FIND_ADD_TURIST).Data(turist).Build();
                }
                catch (Exception e)
                {
                    connected = false;
                    return new Response.Builder().Type(ResponseType.ERROR).Build();
                }
            }
            if (request.Type is RequestType.BUY_BILET)
            {
                Console.WriteLine("BuyBiletRequest ...");
                Bilet bilet = (Bilet)request.Data;
                try
                {
                    lock (server)
                    {
                        server.SaveBilet(bilet);
                    }

                    return new Response.Builder().Type(ResponseType.OK).Build();
                }
                catch (Exception e)
                {
                    connected = false;
                    return new Response.Builder().Type(ResponseType.ERROR).Build();
                }
            }
            if (request.Type is RequestType.UPDATE_ZBOR)
            {
                Console.WriteLine("UpdateZborRequest ...");
                Zbor zbor = (Zbor)request.Data;
                try
                {
                    lock (server)
                    {
                    server.UpdateZbor(zbor);
                    }

                    return new Response.Builder().Type(ResponseType.OK).Build();
                }
                catch (Exception e)
                {
                    connected = false;
                    return new Response.Builder().Type(ResponseType.ERROR).Build();
                }
            }
            if (request.Type is RequestType.SET_OBSERVER)
            {
                Console.WriteLine("SetObsRequest ...");
                Angajat angajat = (Angajat)request.Data;
                try
                {
                    lock (server)
                    {
                        server.SetObsForm(angajat,this);
                    }

                    return new Response.Builder().Type(ResponseType.OK).Build();
                }
                catch (Exception e)
                {
                    connected = false;
                    return new Response.Builder().Type(ResponseType.ERROR).Build();
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
