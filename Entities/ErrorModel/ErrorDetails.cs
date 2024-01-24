using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Entities.ErrorModel
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        //override tostring method to convert the ErrorDetails object to a JSON-formatted string
        public override string ToString() => JsonSerializer.Serialize(this);
    }
}
