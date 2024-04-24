using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZbor.networking
{
    [Serializable]
    public class Response
    {
        public ResponseType Type { get; private set; }
        public object Data { get; private set; }

        private Response() { }

        public override string ToString()
        {
            return $"Response{{type='{Type}', data='{Data}'}}";
        }

        public class Builder
        {
            private readonly Response response = new Response();

            public Builder Type(ResponseType type)
            {
                response.Type = type;
                return this;
            }

            public Builder Data(object data)
            {
                response.Data = data;
                return this;
            }

            public Response Build()
            {
                return response;
            }
        }
    }
}
