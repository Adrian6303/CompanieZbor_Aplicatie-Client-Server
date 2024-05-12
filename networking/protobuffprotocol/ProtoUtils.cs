using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Proto;

namespace CZbor.protobuffprotocol
{
    public class ProtoUtils
    {
        public static CZbor.model.Angajat GetAngajat(Proto.Request request)
        {
            var user = new CZbor.model.Angajat(request.Angajat.User, request.Angajat.Password);
            return user;
        }

        public static CZbor.model.Bilet GetBilet(Proto.Request request)
        {
            CZbor.model.Angajat angajat1 = new CZbor.model.Angajat(request.Bilet.Angajat.User, request.Bilet.Angajat.Password);
            CZbor.model.Zbor zbor1 = new CZbor.model.Zbor(request.Bilet.Zbor.Destinatia, DateTime.Parse(request.Bilet.Zbor.DataPlecarii), request.Bilet.Zbor.Aeroport, request.Bilet.Zbor.NrLocuri);
            CZbor.model.Turist client1 = new CZbor.model.Turist(request.Bilet.Client.Nume);
            List<CZbor.model.Turist> turisti = new List<CZbor.model.Turist>();
            foreach (var turist in request.Bilet.ListaTuristi)
            {
                CZbor.model.Turist turist1 = new CZbor.model.Turist(turist.Nume);
                turisti.Add(turist1);
            }

            var bilet = new CZbor.model.Bilet(angajat1,zbor1, client1, turisti, request.Bilet.AdresaClient, request.Bilet.NrLocuri);
            return bilet;
        }

        public static CZbor.model.Zbor GetZbor(Proto.Request request)
        {
            var zbor = new CZbor.model.Zbor(request.Zbor.Destinatia, DateTime.Parse(request.Zbor.DataPlecarii), request.Zbor.Aeroport, request.Zbor.NrLocuri);
            zbor.Id = request.Zbor.Id;
            return zbor;
        }

        public static Proto.Response CreateOkResponse()
        {
            Proto.Response response = new Proto.Response { Type = Proto.Response.Types.ResponseType.Ok };
            return response;
        }
        public static Proto.Response CreateOkResponse(Angajat angajat)
        {

            Proto.Response response = new Proto.Response { Type = Proto.Response.Types.ResponseType.Ok };
            response.Angajat = new Proto.Angajat { User = angajat.User, Password = angajat.Password };
            return response;
        }



        public static Proto.Response CreateErrorResponse()
        {
            Proto.Response response = new Proto.Response { Type = Proto.Response.Types.ResponseType.Error};
            return response;
        }

        public static Proto.Response CreateGetZboruriResponse(List<Zbor> zbors)
        {
            Proto.Response response = new Proto.Response { Type = Proto.Response.Types.ResponseType.GetZboruri };
            foreach (var zbor in zbors)
            {
                Proto.Zbor protoZbor = new Proto.Zbor();
                protoZbor.Destinatia = zbor.Destinatia;
                protoZbor.DataPlecarii = zbor.DataPlecarii;
                protoZbor.Aeroport = zbor.Aeroport;
                protoZbor.NrLocuri = zbor.NrLocuri;
                protoZbor.Id = zbor.Id;

                response.Zboruri.Add(protoZbor);
            }
            return response;
        }

        public static Proto.Response CreateFilterZboruriResponse(List<Zbor> zbors)
        {
            Proto.Response response = new Proto.Response { Type = Proto.Response.Types.ResponseType.FilterZboruri };
            foreach (var zbor in zbors)
            {
                Proto.Zbor protoZbor = new Proto.Zbor();
                protoZbor.Destinatia = zbor.Destinatia;
                protoZbor.DataPlecarii = zbor.DataPlecarii;
                protoZbor.Aeroport = zbor.Aeroport;
                protoZbor.NrLocuri = zbor.NrLocuri;
                protoZbor.Id = zbor.Id;

                response.Zboruri.Add(protoZbor);
            }
            return response;
        }

        public static Proto.Response CreateFindAddTuristResponse(Turist turist)
        {
            Proto.Response response = new Proto.Response { Type = Proto.Response.Types.ResponseType.FindAddTurist };
            response.Turist = new Proto.Turist { Nume = turist.Nume };
            return response;
        }

        public static Proto.Response CreateUpdateZborResponse()
        {
            Proto.Response response = new Proto.Response { Type = Proto.Response.Types.ResponseType.UpdateZbor };
            return response;
        }



    }

       
}
