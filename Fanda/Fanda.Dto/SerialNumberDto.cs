using Fanda.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanda.Dto
{
    public class SerialNumberDto
    {
        public class SerialNumber
        {
            // YY, YYYY, MM, DD
            // JJJ
            // N, NNN
            // HH, MI, SS, MS
            public SerialNumberModule Module { get; set; }
            public string Prefix { get; set; }
            public string SerialFormat { get; set; }
            public string Suffix { get; set; }
            public string LastValue { get; set; }
            public int LastNumber { get; set; }
            public SerialNumberReset Reset { get; set; }
        }
    }
}
