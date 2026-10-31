using System;
using System.Collections.Generic;
using System.Text;

namespace TusindFrydApp
{
    public class Drivhus
    {
        public int Nummer { get; set; }

        public Drivhus(int nummer)
        {
            Nummer = nummer;
        }
    }

    public class Produktionsbakke
    {
        public string Navn { get; set; }
        public int Størrelse { get; set; }
        public Drivhus Drivhus { get; set; }

        public Produktionsbakke(string navn, int størrelse, Drivhus drivhus)
        {
            Navn = navn;
            Størrelse = størrelse;
            Drivhus = drivhus;
        }
    }
}
