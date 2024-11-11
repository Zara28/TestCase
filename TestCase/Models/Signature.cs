using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCase.Models
{
    public class Signature
    {
        public string Base64Header { get; set; }
        public string Base64Payload { get; set; }
        public string SecretKey { get; set; }
    }
}
