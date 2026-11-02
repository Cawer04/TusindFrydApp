using System;
using System.Collections.Generic;

namespace TusindFrydApp
{
    // Program-klassen udgør UI-laget (User Interface) i vores 3-lags arkitektur
    // Den håndterer udelukkende præsentationen på skærmen og brugerinput fra konsollen.
    // Den har intet kendskab til domæneobjekternes direkte forretningslogik, men kommunikerer
    // udelukkende igennem Controller-klassen
    internal class Program
    {
        
        static void Main(string[] args)
        {
            // Vi instantiere Controlleren, som tager sig af alle bagvedliggende data og forretningsregler.
            Controller controller = new Controller();

            // Variabel til at styre vores hovedmenu-loop, så programmet kører indtil brugeren vælger at afslutte.
            bool kører = true;

            while (kører)
            {
                // Renser konsolskærmen for at holde menuen pæn og overskuelig hver gang man vender tilbage.
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("        TUSINDFRYD GARTNERI SYSTEM      ");
                Console.WriteLine("=========================================");
                Console.WriteLine("1. Etabler ny blomstersort");
                Console.WriteLine("2. Start produktion");
                Console.WriteLine("3. Gennemse produktion / Optælling");
                Console.WriteLine("4. Afslut produktion");
                Console.WriteLine("5. Vis alle produktioner");
                Console.WriteLine("6. Afslut program");
                Console.WriteLine("========================================");
                Console.Write("Vælg en mulighed (1-6): ");

                // Læser brugerens menuvalg fra tastaturet.
                string valg = Console.ReadLine();
                Console.WriteLine();

                // Switch-statement der sender brugeren videre til den rette undermenu ud fra valg.
                switch (valg)
                {
                    case "1":
                        PrøvEtablerBlomstersort(controller);
                        break;
                    case "2":
                        PrøvStartProduktion(controller);
                        break;
                    case "3":
                        PrøvGennemseProduktion(controller);
                        break;
                    case "4":
                        PrøvAfslutProduktion(controller);
                        break;
                    case "5":
                        VisAlleProduktioner(controller);
                        break;
                    case "6":
                        // Sætter kører-flaget til false, hvilket får while-løkken til at stoppe.
                        kører = false;
                        Console.WriteLine("Programmet afsluttes...");
                        break;
                    default:
                        // Håndterer hvis brugeren trykker på en ugyldig tast.
                        Console.WriteLine("Ugyldigt valg. Tryk på en tast...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // Hjælpemetode til Pr21 Øvelse 3: Etabler ny blomstersort
        // Indsamler oplysninger fra brugeren og kaster dem videre til controlleren.
        static void PrøvEtablerBlomstersort(Controller controller)
        {
            Console.WriteLine("--- 1. ETABLER NY BLOMSTERSORT ---");
            Console.Write("Indtast navn på blomstersort (f.eks. Marguerit): ");
            string navn = Console.ReadLine();

            // Vi konverterer tekstinput fra Console.ReadLine() til heltal (int) eller decimaltal (double)
            Console.Write("Indtast produktionstid i dage: ");
            int prodTid = int.Parse(Console.ReadLine());

            Console.Write("Indtast halveringstid i dage: ");
            int halvering = int.Parse(Console.ReadLine());

            Console.Write("Indtast størrelse: ");
            double størrelse = double.Parse(Console.ReadLine());

            // Sender data videre til controlleren (Array.Empty<byte>() fungerer som en tom pladsholder for billedet)
            controller.EtablerNyBlomstersort(navn, Array.Empty<byte>(), prodTid, halvering, størrelse);

            Console.WriteLine($"\n--> Blomstersort '{navn}' blev oprettet i systemet!");
            Console.WriteLine("\nTryk på en tast for at gå tilbage...");
            Console.ReadKey();
        }

        // Hjælpemetode til Pr22 Øvelse 3: Start produktion
        // Modtager parametre til at starte en ny produktion på en specifik bakke.
        static void PrøvStartProduktion(Controller controller)
        {
            Console.WriteLine("--- 2. START PRODUKTION ---");
            Console.Write("Indtast drivhusnummer: ");
            string drivhus = Console.ReadLine();

            Console.Write("Indtast produktionsbakke navn: ");
            string bakke = Console.ReadLine();

            Console.Write("Indtast blomstersort: ");
            string sort = Console.ReadLine();

            Console.Write("Indtast startantal: ");
            int startAntal = int.Parse(Console.ReadLine());

            // Controlleren udregner det forventede slutantal og returnerer det direkte
            int forventet = controller.StartProduktion(drivhus, bakke, sort, startAntal, DateTime.Now);

            Console.WriteLine($"\n--> Produktion startet på bakke {bakke} med {sort}!");
            Console.WriteLine($"--> Beregnet forventet slutantal: {forventet}");
            Console.WriteLine("\nTryk på en tast for at gå tilbage...");
            Console.ReadKey();
        }

        // Hjælpemetode til Pr23 Øvelse 3: Gennemse produktion / Optælling
        // Kører sekvensen med at angive medarbejder, vælge bakke og indtaste det faktiske tælleret resultat
        static void PrøvGennemseProduktion(Controller controller)
        {
            Console.WriteLine("--- 3. GENNEMSE PRODUKTION ---");
            Console.Write("Indtast dine initialer: ");
            string initialer = Console.ReadLine();

            // Del 1 af SD: Sætter aktiv medarbejder i controllerens tilstand
            controller.GennemsetAfMedarbejder(initialer);

            Console.Write("Indtast produktionsbakke der skal gennemses: ");
            string bakke = Console.ReadLine();

            // Del 2 af SD: Sætter den valgte bakke i controllerens tilstand
            controller.VælgeProduktionsbakke(bakke);

            Console.Write("Indtast optalt antal blomster: ");
            int optalt = int.Parse(Console.ReadLine());

            // Del 3 af SD: Gemmer optællingen og får beregnede værdier retur via en tuple (res.beregnetSlutantal og res.afvigelseProcent)
            var res = controller.AngivOptællingsinformation(optalt, DateTime.Now);

            Console.WriteLine($"\n Optælling gemt for medarbejder {initialer} på bakke {bakke}!");
            Console.WriteLine($" Beregnet slutantal: {res.beregnetSlutantal}");
            Console.WriteLine($" Afvigelse i procent: {res.afvigelseProcent}%");
            Console.WriteLine("\nTryk på en tast for at gå tilbage...");
            Console.ReadKey();
        }

        // Hjælpemetode til at afslutte en valgt produktion
        static void PrøvAfslutProduktion(Controller controller)
        {
            Console.WriteLine("--- 4. AFSLUT PRODUKTION ---");
            Console.Write("Indtast produktionsbakke der skal afsluttes: ");
            string bakke = Console.ReadLine();

            // Sætter status for produktionen på den valgte bakke til 'Afsluttet'
            controller.AfslutProduktion(bakke, true);

            Console.WriteLine($"\n--> Produktion på bakke {bakke} er nu markeret som AFSLUTTET!");
            Console.WriteLine("\nTryk på en tast for at gå tilbage...");
            Console.ReadKey();
        }

        // Udskriver en komplet oversigt over alle oprettede produktioner i systemet.
        static void VisAlleProduktioner(Controller controller)
        {
            Console.WriteLine("--- 5. OVERSIGT OVER PRODUKTIONER ---");

            // Henter listen af produktioner fra controlleren.
            List<Produktion> liste = controller.HentAlleProduktioner();

            if (liste.Count == 0)
            {
                Console.WriteLine("Der er endnu ingen produktioner i systemet.");
            }
            else
            {
                // Løber igennem hver produktion og udskriver dens detaljer i konsollen.
                foreach (var prod in liste)
                {
                    string status = prod.Afsluttet ? "Afsluttet" : "Aktiv";
                    Console.WriteLine($"Bakke: {prod.Produktionsbakke.Navn} | Sort: {prod.Blomstersort.Navn} | StartDato: {prod.StartDato:dd-MM-yyyy} | StartAntal: {prod.StartAntal} | ForventetSlut: {prod.ForventetSlutAntal} | Status: {status}");

                    // Hvis der er foretaget en optælling på produktionen, udskrives disse detaljer også under
                    if (prod.Optælling != null)
                    {
                        Console.WriteLine($"    Seneste optælling: {prod.Optælling.OptaltAntal} blomster | Afvigelse: {prod.Optælling.AfvigelsesProcent}% | Medarbejder: {prod.Optælling.Medarbejder.Initialer}");
                    }
                    Console.WriteLine(new string('-', 80));
                }
            }

            Console.WriteLine("\nTryk på en tast for at gå tilbage...");
            Console.ReadKey();
        }
    }
}