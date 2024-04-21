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
                    //Monitor.Wait(responses); 
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


        private void handleUpdate(UpdateZborResponse update)
        {
                //client.updateZbor();
            
            
        }
        public virtual void run()
        {
            while (!finished)
            {
                try
                {
                    object response = formatter.Deserialize(stream);
                    Console.WriteLine("response received " + response);
                    if (response is UpdateZborResponse)
                    {
                        lock (responses)
                        {
                            //UpdateZborResponse r = (UpdateZborResponse)readResponse();
                            client.updateZbor();
                        }
                        
                        //handleUpdate((UpdateResponse)response);
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

        public IEnumerable<Zbor> FindAllZboruri()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Angajat> FindAllAngajati()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Turist> FindAllTuristi()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Bilet> FindAllBilete()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Zbor> FindAllAvailableFlights()
        {
            sendRequest(new GetZboruriRequest());
            Response response1 = readResponse();
            while (!(response1 is GetZboruriResponse))
            {
                response1 = readResponse();
            }
            return ((GetZboruriResponse)response1).Zboruri;
            
            
            
            
        }

        public IEnumerable<Zbor> FindZborByDestinatieAndDate(string destinatie, DateTime data)
        {
            sendRequest(new FilterZboruriRequest(destinatie,data));
            FilterZboruriResponse response = (FilterZboruriResponse)readResponse();

            return response.Zboruri;
        }

        public Zbor FindZborById(int id)
        {
            sendRequest(new FindZborRequest(id));
            FindZborResponse response = (FindZborResponse)readResponse();

            return response.Zbor;
        }

        public Turist FindTuristByName(string name)
        {
            sendRequest(new FindTuristRequest(name));
            FindTuristResponse response = (FindTuristResponse)readResponse();

            return response.Turist;
        }

        public void SaveBilet(Bilet bilet)
        {
            sendRequest(new SaveBiletRequest(bilet));
            Response response = readResponse();
        }

        public void SaveZbor(Zbor zbor)
        {
            throw new NotImplementedException();
        }

        public void SaveAngajat(Angajat angajat)
        {
            throw new NotImplementedException();
        }

        public void SaveTurist(Turist turist)
        {
            sendRequest(new SaveTuristRequest(turist));
            Response response = readResponse();
        }

        public void UpdateZbor(Zbor zbor)
        {
            sendRequest(new UpdateZborRequest(zbor));
            Response response = readResponse();
            //Console.WriteLine("okkkkkk");
        }

        public Angajat FindAngajat(string username, string password, IObserver client)
        {
            initializeConnection();
            Angajat angajat = new Angajat(username, password);
            sendRequest(new LoginRequest(angajat));
            Response response = readResponse();
            if (response is LoginResponse)
            {
                LoginResponse response1 = (LoginResponse)response;
                this.client = client;
                return response1.Angajat;
            }
            if (response is ErrorResponse)
            {
                ErrorResponse err = (ErrorResponse)response;
                closeConnection();
                return null;
                
            }
            return null;
        }

        public void Logout(Angajat angajat)
        {
            sendRequest(new LogoutRequest(angajat));
            Response response = readResponse();
        }

        public void SetObsForm(Angajat angajat, IObserver client)
        {
            sendRequest(new SetObsRequest(angajat));
            Response response = readResponse();
            this.client = client;
        }
    }
}
