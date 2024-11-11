using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCase.Models
{
    public class Protected
    {
        public string alg { get; set; }
        public string kid { get; set; }
        public DateTime signdate { get; set; }
        public string cty { get; set; } = "application/json";
    }
}
