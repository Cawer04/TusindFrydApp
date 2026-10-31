using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;

namespace TusindFrydApp
{
    // Controller-klassen fungerer som bindeled mellem vores brugergrænseflade (UI) og vores domænemodel (forretningslogik).
    // Det er herfra, vi styrer alle systemets handlinger ud fra operationskontrakterne (SOC).
    public class Controller
    {
        // Vi opretter lister i hukommelsen til at holde styr på vores data,
        // da vi ikke har en rigtig database påkoblet endnu (kaldet in-memory storage).
        private List<Drivhus> drivhus = new List<Drivhus>();
        private List<Produktionsbakke> produktionsbakker = new List<Produktionsbakke>();
        private List<Blomstersort> blomstersorter = new List<Blomstersort>();
        private List<Medarbejder> medarbejdere = new List<Medarbejder>();
        private List<Produktion> produktioner = new List<Produktion>();

        // Disse tilstandsvariabler husker, hvem der er logget ind/aktiveret, 
        // og hvilken bakke der arbejdes på lige nu undervejs i et brugsscenarie.
        private Medarbejder aktivMedarbejder;
        private Produktionsbakke valgtProduktionsBakke;

        // Constructor: Kører automatisk, når Controlleren bliver instantieret i Program.cs.
        public Controller()
        {
            // Vi fylder systemet op med nogle startdata med det samme, så vi har noget at teste på.
            OpretFakeData();
        }

        // Hjælpemetode der opretter testdata og lægger dem ind i vores lister.
        private void OpretFakeData()
        {
            // Opretter to drivhuse og tilføjer dem til listen
            Drivhus d1 = new Drivhus(1);
            Drivhus d2 = new Drivhus(2);
            drivhus.AddRange(new Drivhus[] { d1, d2 });

            // Opretter tre produktionsbakker placeret i drivhusene
            produktionsbakker.Add(new Produktionsbakke("1A", 10, d1));
            produktionsbakker.Add(new Produktionsbakke("2A", 10, d2));
            produktionsbakker.Add(new Produktionsbakke("3C", 10, d2));

            // Tilføjer et par blomstersorter (Array.Empty<byte>() bruges som dummy for et billede)
            blomstersorter.Add(new Blomstersort("Kornblomst", Array.Empty<byte>(), 30, 150, 0.01));
            blomstersorter.Add(new Blomstersort("Tagetes", Array.Empty<byte>(), 40, 120, 0.0225));

            // Tilføjer medarbejdere med deres initialer
            medarbejdere.Add(new Medarbejder("HR"));
            medarbejdere.Add(new Medarbejder("OR"));
        }

        // PR21 ØVELSE 3.1 & 3.3: etablerNyBlomstersort
        // Tager imod oplysninger om en ny sort, opretter objektet og gemmer det i listen.
        public void EtablerNyBlomstersort(string navn, byte[] billede, int produktionstid, int halveringstid, double størrelse)
        {
            Blomstersort sort = new Blomstersort(navn, billede, produktionstid, halveringstid, størrelse);
            blomstersorter.Add(sort);
        }

        // PR22 ØVELSE 3.1 & 3.3: startProduktion
        // Slår bakke og sort op i listerne, opretter en ny produktion og returnerer det automatisk beregnede forventede slutantal.
        public int StartProduktion(string drivhusNummer, string bakkeNavn, string sortNavn, int startAntal, DateTime startDato)
        {
            // Vi bruger .First() til at finde det præcise objekt i vores lister, der matcher navnet (uafhængigt af store/små bogstaver)
            Produktionsbakke bakke = produktionsbakker.First(b => b.Navn.Equals(bakkeNavn, StringComparison.OrdinalIgnoreCase));
            Blomstersort sort = blomstersorter.First(s => s.Navn.Equals(sortNavn, StringComparison.OrdinalIgnoreCase));

            // Opretter selve produktionsobjektet ud fra oplysningerne og gemmer det i listen 'produktioner'
            Produktion produktion = new Produktion(startDato, startAntal, bakke, sort);
            produktioner.Add(produktion);

            // Returnerer det tal, som Produktion-klassen selv har beregnet internt
            return produktion.ForventetSlutAntal;
        }

        // PR23 ØVELSE 3.1 & 3.3: Gennemse produktion (Del 1 af 3)
        // Registrerer hvilken medarbejder der udfører arbejdet og gemmer objektet i 'aktivMedarbejder'.
        public void GennemsetAfMedarbejder(string initialer)
        {
            aktivMedarbejder = medarbejdere.First(m => m.Initialer.Equals(initialer, StringComparison.OrdinalIgnoreCase));
        }

        // PR23 ØVELSE 3.1 & 3.3: Gennemse produktion (Del 2 af 3)
        // Finder og gemmer den bakke, som medarbejderen ønsker at kontrollere i 'valgtProduktionsBakke'.
        public void VælgeProduktionsbakke(string navn)
        {
            valgtProduktionsBakke = produktionsbakker.First(b => b.Navn.Equals(navn, StringComparison.OrdinalIgnoreCase));
        }

        // PR23 ØVELSE 3.1 & 3.3: Gennemse produktion (Del 3 af 3)
        // Finder den igangværende produktion for den valgte bakke, lader produktionen oprette en optælling og returnerer resultatet i en tuple (to værdier på én gang).
        public (int beregnetSlutantal, double afvigelseProcent) AngivOptællingsinformation(int antal, DateTime dato)
        {
            // Vi søger efter en produktion på den valgte bakke, som endnu IKKE er markeret som afsluttet (!p.Afsluttet)
            Produktion produktion = produktioner.First(p => p.Produktionsbakke == valgtProduktionsBakke && !p.Afsluttet);

            // Lader produktionsobjektet stå for at oprette Optælling-objektet (GRASP: Creator)
            Optælling optælling = produktion.OpretOptælling(antal, dato, aktivMedarbejder);

            // Returnerer både beregnet slutantal og afvigelsesprocenten samlet tilbage til UI
            return (optælling.BeregninetSlutAntal, optælling.AfvigelsesProcent);
        }

        // Operation: afslutProduktion
        // Finder produktionen på den valgte bakke og sætter dens status-flag 'Afsluttet' til true/false.
        public void AfslutProduktion(string bakkeNavn, bool afsluttet)
        {
            Produktion produktion = produktioner.First(p => p.Produktionsbakke.Navn.Equals(bakkeNavn, StringComparison.OrdinalIgnoreCase));
            produktion.Afsluttet = afsluttet;
        }

        // Ekstra hjælpemetode til UI: Giver brugergrænsefladen mulighed for at hente hele listen og vise oversigten på skærmen.
        public List<Produktion> HentAlleProduktioner()
        {
            return produktioner;
        }
    }
}