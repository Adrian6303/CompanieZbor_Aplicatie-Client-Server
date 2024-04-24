using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CZbor.networking;

namespace CZbor.networking
{
    [Serializable]
    public class Request
    {
        public RequestType Type { get; private set; }
        public object Data { get; private set; }

        private Request() { }

        public override string ToString()
        {
            return $"Request{{type='{Type}', data='{Data}'}}";
        }

        public class Builder
        {
            private readonly Request request = new Request();

            public Builder Type(RequestType type)
            {
                request.Type = type;
                return this;
            }

            public Builder Data(object data)
            {
                request.Data = data;
                return this;
            }

            public Request Build()
            {
                return request;
            }
        }
    }
}
