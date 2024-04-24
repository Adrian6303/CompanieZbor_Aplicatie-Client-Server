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

namespace CZbor.networking
{
    public class ServerProxy : IService
    {
        private string host;
        private int port;

        private IObserver client;

        private NetworkStream stream;

        private IFormatter formatter;
        private TcpClient connection;

        private Queue<Response> responses;
        private volatile bool finished;
        private EventWaitHandle _waitHandle;
        public ServerProxy(string host, int port)
        {
            this.host = host;
            this.port = port;
            responses = new Queue<Response>();
        }
        

        private void closeConnection()
        {
            finished = true;
            try
            {
                stream.Close();

                connection.Close();
                _waitHandle.Close();
                client = null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

        }

        private void sendRequest(Request request)
        {
            try
            {
                formatter.Serialize(stream, request);
                stream.Flush();
            }
            catch (Exception e)
            {
                throw new Exception("Error sending object " + e);
            }

        }

        private Response readResponse()
        {
            Response response = null;
            try
            {
                _waitHandle.WaitOne();
                lock (responses)
                {
                    response = responses.Dequeue();

                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return response;
        }
        private void initializeConnection()
        {
            try
            {
                connection = new TcpClient(host, port);
                stream = connection.GetStream();
                formatter = new BinaryFormatter();
                finished = false;
                _waitHandle = new AutoResetEvent(false);
                startReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
        private void startReader()
        {
            Thread tw = new Thread(run);
            tw.Start();
        }

        private bool isUpdate(Response response)
        {
            return response.Type is ResponseType.UPDATE_ZBOR;
        }
        private void handleUpdate(Response response)
        {
            if (response.Type is ResponseType.UPDATE_ZBOR)
            {
                try {       
                    
                    client.updateZbor();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
              
            }
        }

        public virtual void run()
        {
            while (!finished)
            {
                try
                {
                    object response = formatter.Deserialize(stream);
                    Console.WriteLine("response received " + response);
                    if (isUpdate((Response)response))
                    {
                        handleUpdate((Response)response);
                    }
                    else
                    {

                        lock (responses)
                        {


                            responses.Enqueue((Response)response);

                        }
                        _waitHandle.Set();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Reading error " + e);
                }

            }
        }

        public IEnumerable<Zbor> FindAllAvailableFlights()
        {
            Request request = new Request.Builder().Type(RequestType.GET_ZBORURI).Build();
            sendRequest(request);
            Response response = readResponse();

            return (List<Zbor>) response.Data;
        }

        public IEnumerable<Zbor> FindZborByDestinatieAndDate(string destinatie, DateTime data)
        {
            Zbor zbor = new Zbor(destinatie,data,null, 0);
            Request request = new Request.Builder().Type(RequestType.FILTER_ZBORURI).Data(zbor).Build();
            sendRequest(request);
            Response response = readResponse();

            return (List<Zbor>)response.Data;
        }

        public Turist findOrAddTurist(string name)
        {
            Request request = new Request.Builder().Type(RequestType.FIND_ADD_TURIST).Data(name).Build();
            sendRequest(request);
            Response response = readResponse();

            return (Turist)response.Data;
        }

        public void SaveBilet(Bilet bilet)
        {
            Request request = new Request.Builder().Type(RequestType.BUY_BILET).Data(bilet).Build();
            sendRequest(request);
            Response response = readResponse();
        }

        public void UpdateZbor(Zbor zbor)
        {
            Request request = new Request.Builder().Type(RequestType.UPDATE_ZBOR).Data(zbor).Build();
            sendRequest(request);
            Response response = readResponse();
        }

        public Angajat FindAngajat(string username, string password, IObserver client)
        {
            initializeConnection();
            Angajat angajat = new Angajat(username, password);
            Request request = new Request.Builder().Type(RequestType.LOGIN).Data(angajat).Build();
            sendRequest(request);
            Response response = readResponse();
            if (response.Type is ResponseType.OK)
            {
                this.client = client;
                return (Angajat)response.Data;
            }
            if (response.Type is ResponseType.ERROR)
            {
                closeConnection();
                return null;
                
            }
            return null;
        }

        public void Logout(Angajat angajat)
        {
            Request request = new Request.Builder().Type(RequestType.LOGOUT).Data(angajat).Build();
            sendRequest(request);
            Response response = readResponse();
        }

        public void SetObsForm(Angajat angajat, IObserver client)
        {
            Request request = new Request.Builder().Type(RequestType.SET_OBSERVER).Data(angajat).Build();
            sendRequest(request);
            Response response = readResponse();
            this.client = client;
        }
    }
}
