using System;
using System.Collections.Generic;
using System.Text;

namespace TusindFrydApp
{
    public class Blomstersort
    {
        public string Navn { get; set; }
        public byte[] Billede { get; set; }
        public int ProduktionstidDage { get; set; }
        public int Halveringstid { get; set; }
        public double Størrelse { get; set; }

        public Blomstersort(string navn, byte[] billede, int produktionstidDage, int halveringstid, double størrelse)
        {
            Navn = navn;
            Billede = billede;
            ProduktionstidDage = produktionstidDage;
            Halveringstid = halveringstid;
            Størrelse = størrelse;
        }
    }
}
