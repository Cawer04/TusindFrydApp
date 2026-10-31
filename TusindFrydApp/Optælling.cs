using System;
using System.Collections.Generic;
using System.Text;

namespace TusindFrydApp
{
    // Optælling-klassen er en domæneklasse, der repræsenterer en konkret kontrol af en produktion.
    // Den gemmer oplysninger om, hvornår der er talt op, hvor mange blomster der var, 
    // hvem der talte op, og hvor stor en afvigelse der var i forhold til det forventede antal[cite: 2].
    public class Optælling
    {
        // Properties (egenskaber) til at opbevare optællingens data og resultater
        public DateTime OptaltDato { get; set; }
        public int OptaltAntal { get; set; }
        public int BeregninetSlutAntal { get; set; }
        public double AfvigelsesProcent { get; set; }

        // Association til Medarbejder klassen: Gemmer en reference til den medarbejder, der udførte optællingen[cite: 2]
        public Medarbejder Medarbejder { get; set; }

        // Constructor: Kører når 'Produktion'-klassen opretter et nytt Optælling-objekt.
        // Den modtager de nødvendige data fra optællingen og beregner afvigelsesprocenten med det samme[cite: 2].
        public Optælling(DateTime optaltDato, int optaltAntal, int forventetSlutAntal, Medarbejder medarbejder)
        {
            // Gemmer input-værdierne i klassens egenskaber
            OptaltDato = optaltDato;
            OptaltAntal = optaltAntal;
            BeregninetSlutAntal = forventetSlutAntal;
            Medarbejder = medarbejder;

            // Forretningslogik: Vi tjekker, at forventetSlutAntal er større end 0 for at undgå division med nul.
            if (forventetSlutAntal > 0)
            {
                // Beregner afvigelsen i procent: ((faktisk  forventet) / forventet) * 100[2].
                // (double) typecasting er nødvendig, så C# ikke laver heltalsdivision, som ville runde forkert af.
                // Math.Round(...,2) afrunder resultatet til 2 decimaler, så det ser pænt ud[cite: 2].
                AfvigelsesProcent = Math.Round(((double)(optaltAntal - forventetSlutAntal) / forventetSlutAntal) * 100, 2);
            }
        }
    }
}