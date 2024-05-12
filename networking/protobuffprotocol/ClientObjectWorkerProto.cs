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
using Google.Protobuf;

namespace CZbor.protobuffprotocol
{
    public class ClientObjectWorkerProto : IObserver
    {
        private IService server;
        private TcpClient connection;

        private NetworkStream stream;
       // private IFormatter formatter;
        private volatile bool connected;
        public ClientObjectWorkerProto(IService server, TcpClient connection)
        {
            this.server = server;
            this.connection = connection;
            try
            {

                stream = connection.GetStream();
                //formatter = new BinaryFormatter();
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
                    Proto.Request request = Proto.Request.Parser.ParseDelimitedFrom(stream);
                    Proto.Response response = handleRequest(request);
                    //object request = formatter.Deserialize(stream);

                    //object response = handleRequest((Request)request);
                    if (response != null)
                    {
                        sendResponse((Proto.Response)response);
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
            Proto.Response resp = ProtoUtils.CreateUpdateZborResponse();
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



        private Proto.Response handleRequest(Proto.Request request)
        {
            Proto.Response response = null;
            Proto.Request.Types.RequestType requestType = request.Type;
            if (requestType is Proto.Request.Types.RequestType.Login)
            {
                Console.WriteLine("Login request ...");
                CZbor.model.Angajat angajat = ProtoUtils.GetAngajat(request);

                try
                {
                    CZbor.model.Angajat ang = null;
                    Proto.Angajat angProto = null;
                    lock (server)
                    {
                        ang = server.FindAngajat(angajat.Username, angajat.Password, this);
                        angProto = new Proto.Angajat { User = ang.Username, Password = ang.Password };
                    }
                    return ProtoUtils.CreateOkResponse(angProto);
                }
                catch (Exception e)
                {
                    connected = false;
                    return ProtoUtils.CreateErrorResponse();
                }
            }
            if (requestType is Proto.Request.Types.RequestType.Logout)
            {
                Console.WriteLine("Logout request ...");

                CZbor.model.Angajat angajat = ProtoUtils.GetAngajat(request);

                try
                {
                    lock (server)
                    {
                        server.Logout(angajat);
                    }
                    return ProtoUtils.CreateOkResponse();
                }
                catch (Exception e)
                {
                    connected = false;
                    return ProtoUtils.CreateErrorResponse();
                }
            }
            if (requestType is Proto.Request.Types.RequestType.GetZboruri)
            {
                Console.WriteLine("GetZboruriRequest ...");
                List<CZbor.model.Zbor> zboruri = new List<CZbor.model.Zbor>();
                List<Proto.Zbor> zboruriProto = new List<Proto.Zbor>();
                try
                {
                    lock (server)
                    {
                        zboruri = (List<CZbor.model.Zbor>)server.FindAllAvailableFlights();
                    }
                    foreach (var zbor in zboruri)
                    {
                        Proto.Zbor protoZbor = new Proto.Zbor();
                        protoZbor.Destinatia = zbor.Destination;
                        protoZbor.DataPlecarii = zbor.Date.ToString();
                        protoZbor.Aeroport = zbor.Airport;
                        protoZbor.NrLocuri = zbor.NoTotalSeats;
                        protoZbor.Id = zbor.Id;

                        zboruriProto.Add(protoZbor);
                    }
                    return ProtoUtils.CreateGetZboruriResponse(zboruriProto);
                }
                catch (Exception e)
                {
                    connected = false;
                    return ProtoUtils.CreateErrorResponse();
                }
            }

            if (requestType is Proto.Request.Types.RequestType.FilterZboruri)
            {
                Console.WriteLine("FilterZboruriRequest ...");
                List<CZbor.model.Zbor> zboruri = new List<CZbor.model.Zbor>();
                CZbor.model.Zbor zbor = ProtoUtils.GetZbor(request);
                string destinatie = zbor.Destination;
                DateTime data = zbor.Date;
                List<Proto.Zbor> zboruriProto = new List<Proto.Zbor>();
                try
                {
                    lock (server)
                    {
                        zboruri = (List<CZbor.model.Zbor>)server.FindZborByDestinatieAndDate(destinatie, data);
                    }
                    foreach (var zbor1 in zboruri)
                    {
                        Proto.Zbor protoZbor = new Proto.Zbor();
                        protoZbor.Destinatia = zbor1.Destination;
                        protoZbor.DataPlecarii = zbor1.Date.ToString();
                        protoZbor.Aeroport = zbor1.Airport;
                        protoZbor.NrLocuri = zbor1.NoTotalSeats;
                        protoZbor.Id = zbor1.Id;

                        zboruriProto.Add(protoZbor);
                    }
                    return ProtoUtils.CreateFilterZboruriResponse(zboruriProto);
                }
                catch (Exception e)
                {
                    connected = false;
                    return ProtoUtils.CreateErrorResponse();

                }
            }
            if (requestType is Proto.Request.Types.RequestType.FindAddTurist)
            {
                Console.WriteLine("FindAddTuristRequest ...");
                string nume = request.Name;
                try
                {
                    CZbor.model.Turist turist = null;
                    Proto.Turist turistProto = null;
                    lock (server)
                    {
                        turist = (CZbor.model.Turist)server.findOrAddTurist(nume);
                    }
                    turistProto = new Proto.Turist { Nume = turist.TouristName };
                    return ProtoUtils.CreateFindAddTuristResponse(turistProto);
                }
                catch (Exception e)
                {
                    connected = false;
                    return ProtoUtils.CreateErrorResponse();
                }
            }
            if (requestType is Proto.Request.Types.RequestType.BuyBilet)
            {
                Console.WriteLine("BuyBiletRequest ...");
                CZbor.model.Bilet bilet = ProtoUtils.GetBilet(request);
                try
                {
                    lock (server)
                    {
                        server.SaveBilet(bilet);
                    }

                    return ProtoUtils.CreateOkResponse();
                }
                catch (Exception e)
                {
                    connected = false;
                    return ProtoUtils.CreateErrorResponse();
                }
            }
            if (requestType is Proto.Request.Types.RequestType.UpdateZbor)
            {
                Console.WriteLine("UpdateZborRequest ...");
                CZbor.model.Zbor zbor = ProtoUtils.GetZbor(request);
                try
                {
                    lock (server)
                    {
                        server.UpdateZbor(zbor);
                    }

                    return ProtoUtils.CreateOkResponse();
                }
                catch (Exception e)
                {
                    connected = false;
                    return ProtoUtils.CreateErrorResponse();
                }
            }


            return response;
        }

        private void sendResponse(Proto.Response response)
        {
            Console.WriteLine("sending response " + response);
            lock (stream)
            {
                response.WriteDelimitedTo(stream);
                //formatter.Serialize(stream, response);
                stream.Flush();
            }

        }

    }

}