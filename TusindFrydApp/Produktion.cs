using System;
using System.Collections.Generic;
using System.Text;

namespace TusindFrydApp
{
    // Produktion-klassen er en central domæneklasse (Domain Object).
    // Den repræsenterer en konkret blomsterproduktion i gartneriet og holder styr på
    // dens tilstand, tilknyttede bakke, blomstersort samt optællinger.
    public class Produktion
    {
        // Properties (egenskaber) der gemmer oplysninger om produktionens start og forventede status.
        public DateTime StartDato { get; set; }
        public int StartAntal { get; set; }
        public int ForventetSlutAntal { get; set; }
        public bool Afsluttet { get; set; }

        // Associationer i henhold til domænemodellen:
        // Produktion kender til den bakke den står i, hvilken sort der produceres, og en eventuel optælling.
        public Produktionsbakke Produktionsbakke { get; set; }
        public Blomstersort Blomstersort { get; set; }
        public Optælling Optælling { get; set; }

        // Constructor: Kører når vi opretter en ny produktion (ny i gang-sætning)[cite: 2].
        // Den modtager de nødvendige startdata og danner relationerne til bakke og blomstersort.
        public Produktion(DateTime startDato, int startAntal, Produktionsbakke produktionsbakke, Blomstersort blomstersort)
        {
            StartDato = startDato;
            StartAntal = startAntal;
            Produktionsbakke = produktionsbakke;
            Blomstersort = blomstersort;

            // En ny oprettet produktion er som standard ikke afsluttet endnu
            Afsluttet = false;

            // Lige så snart produktionen oprettes, beregnes det forventede slutantal automatisk
            BeregnetForventetSlutantal();
        }

        // Hjælpemetode (forretningslogik) der udregner forventet slutantal.
       
        private void BeregnetForventetSlutantal()
        {
            ForventetSlutAntal = (int)(StartAntal * 0.871);
        }

        // PR23 / GRASP Creator princippet:
        // Da Produktion-klassen har tæt kendskab til sine egne data (forventetSlutAntal) og har en relation til Optælling,
        // er det Produktion-klassen selv, der har ansvaret for at oprette et nyt Optælling-objekt.
        public Optælling OpretOptælling(int optaltAntal, DateTime optaltDato, Medarbejder medarbejder)
        {
            // Opretter optællingen, gemmer den på denne produktion og returnerer den
            Optælling = new Optælling(optaltDato, optaltAntal, ForventetSlutAntal, medarbejder);
            return Optælling;
        }
    }
}