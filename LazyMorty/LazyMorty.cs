using Rick_Morty_console_game.core;
using System;
using System.Linq;

namespace Rick_Morty_console_game.mods
{
    public class LazyMorty : IMorty
    {
        private readonly KeyManager _keyManager;
        private readonly Random _random = new Random();

        public string Name { get; } = "Lazy Morty";

        public LazyMorty(KeyManager keyManager, int maxBoxes)
        {
            _keyManager = keyManager;
        }

        public string GetHMAC(int round) => _keyManager.GetHMAC(round);

        public bool ShouldRemoveBoxes(int selectedBox, int portalGunBox)
        {
            bool willRemove = _random.NextDouble() < 0.7;
            if (!willRemove)
            {
                Console.WriteLine("Morty: Uh, you know what Rick? I'm feeling lazy today. Just pick your box and let's see what happens.");
            }
            return willRemove;
        }

        public int[] SelectBoxesToKeep(int selectedBox, int portalGunBox, int totalBoxes, KeyManager keyManager)
        {
            var availableBoxes = Enumerable.Range(0, totalBoxes)
                .Where(i => i != selectedBox && i != portalGunBox)
                .OrderBy(i => i)
                .ToArray();

            if (selectedBox == portalGunBox)
            {
                return new int[] { selectedBox, availableBoxes[0] };
            }
            else
            {
                return new int[] { selectedBox, portalGunBox };
            }
        }

        public double CalculateProbability(bool switching, int totalBoxes)
        {
            if (switching)
                return 0.7 * ((double)(totalBoxes - 1) / totalBoxes) + 0.3 * (1.0 / totalBoxes);
            else
                return 0.7 * (1.0 / totalBoxes) + 0.3 * (1.0 / totalBoxes);
        }
    }
}