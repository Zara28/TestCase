using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCase.Models
{   
    public class Answer
    {
        public long Id { get; set; }
        public CardInfo Info { get; set; }

        public string Status { get; set; }
        public Error Error { get; set; }
    }

    public class Error
    {
        public string Code { get; set; }

        public string Message { get; set; }
    }
}
