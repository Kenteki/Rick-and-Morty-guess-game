using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTables;

namespace Rick_Morty_console_game.core
{
    public class Statistics
    {
        public void DisplayStatistics(IMorty mortyType, int amountOfBoxes, int stayedRound, int switchedRound, int stayedWin, int switchedWin)
        {
            var finalProbabilities = calculationProbability(mortyType, amountOfBoxes, stayedRound, switchedRound, stayedWin, switchedWin);
            Console.WriteLine("                    GAME RESULT");
            var finalTable = new ConsoleTable("Game results", "Rick switched", "Rick stayed");
            finalTable.AddRow("Rounds", switchedRound, stayedRound);
            finalTable.AddRow("Wins", switchedWin, stayedWin);
            finalTable.AddRow("P (estimate)", finalProbabilities.estimateOfSwitches, finalProbabilities.estimateOfStayed);
            finalTable.AddRow("P (exact)", finalProbabilities.theoryOfSwitches, finalProbabilities.theoryOfStayed);

            finalTable.Configure(o => o.NumberAlignment = Alignment.Right);
            finalTable.Write(Format.Alternative);
        }

        public (string estimateOfSwitches, string estimateOfStayed, string theoryOfSwitches, string theoryOfStayed) 
            calculationProbability(IMorty mortyType, int amountOfBoxes, int stayedRound, int switchedRound, int stayedWin, int switchedWin)
        {
            string estimateOfSwitches = switchedRound > 0 ? (switchedWin / (double)switchedRound).ToString("F3") : "?";
            string estimateOfStayed = stayedRound > 0 ? (stayedWin / (double)stayedRound).ToString("F3") : "?";
            double theoryOfSwitches = mortyType.CalculateProbability(true, amountOfBoxes);
            double theoryOfStayed = mortyType.CalculateProbability(false, amountOfBoxes);

            return (estimateOfSwitches, estimateOfStayed, theoryOfSwitches.ToString(), theoryOfStayed.ToString());
        }
    }
}
