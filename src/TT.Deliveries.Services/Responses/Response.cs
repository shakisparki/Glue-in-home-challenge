using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Deliveries.Core.Enums;

namespace TT.Deliveries.Services.Responses
{
    public class Response
    {
        public Errors Error { get; set; }
        public Response(Errors error)
        {
            Error = error;
        }
        public Response(): this(Errors.Pass){}
    }

    public class Response<T> : Response
    {
        public T Value { get; set; }

        public Response(Errors error)
        {
            Error = error;
        }

        public Response(T value): base(Errors.Pass)
        {
            Value = value;
        }

        public Response(T value, Errors error) : this(value)
        {
            Error = error;
        }
    }
}
