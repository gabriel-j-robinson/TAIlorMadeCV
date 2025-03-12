using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAILorMadeLib.Model
{
    public class Request
    {
        public string model { get; set; }
        public Message[] messages { get; set; }
        //public double temperature { get; set; }
        //public int max_tokens { get; set; }

    }

    public class Message
    {
        public string role { get; set; }
        public string content { get; set; }
    }
}
