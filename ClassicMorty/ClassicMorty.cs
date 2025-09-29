using Rick_Morty_console_game.core;
using System;
using System.Linq;

namespace Rick_Morty_console_game.mods
{
    public class ClassicMorty : IMorty
    {
        private readonly KeyManager _keyManager;

        public string Name { get; } = "Classic Morty";

        public ClassicMorty(KeyManager keyManager, int maxBoxes)
        {
            _keyManager = keyManager;
        }

        public string GetHMAC(int round) => _keyManager.GetHMAC(round);

        public bool ShouldRemoveBoxes(int selectedBox, int portalGunBox)
        {
            return true;
        }

        public int[] SelectBoxesToKeep(int selectedBox, int portalGunBox, int totalBoxes, KeyManager keyManager)
        {
            keyManager.GenerateKey(totalBoxes - 1);
            int round = keyManager.GetCurrentRound() - 1;

            Console.WriteLine("Morty: Let's, uh, generate another value now, I mean, to select a box to keep in the game.");
            Console.WriteLine($"Morty: HMAC{round + 2}={keyManager.GetHMAC(round)}");

            int userChoice2;
            do
            {
                Console.Write($"Morty: Rick, enter your number [0,{totalBoxes - 1}) and, uh, don't say I didn't play fair, okay?\nRick: ");
                if (int.TryParse(Console.ReadLine(), out userChoice2) && userChoice2 >= 0 && userChoice2 < totalBoxes - 1)
                    break;
                Console.WriteLine($"Please enter a number between 0 and {totalBoxes - 2}");
            } while (true);

            int fairValue2 = keyManager.CalculateFairValue(userChoice2, totalBoxes - 1, round);

            if (selectedBox == portalGunBox)
            {
                var availableBoxes = Enumerable.Range(0, totalBoxes)
                    .Where(i => i != selectedBox)
                    .ToArray();
                int randomIndex = fairValue2 % availableBoxes.Length;
                int boxToKeep = availableBoxes[randomIndex];
                return new int[] { selectedBox, boxToKeep };
            }
            else
            {
                return new int[] { selectedBox, portalGunBox };
            }
        }

        public double CalculateProbability(bool switching, int totalBoxes)
        {
            if (switching)
                return (double)(totalBoxes - 1) / totalBoxes;
            else
                return 1.0 / totalBoxes;
        }
    }
}