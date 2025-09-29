using Rick_Morty_console_game.core;
using System.Linq;
using System;

namespace Rick_Morty_console_game.mods
{
    public class EvilMorty : IMorty
    {
        private readonly KeyManager _keyManager;
        private readonly int _maxBoxes;

        public string Name { get; } = "Evil Morty";

        public EvilMorty(KeyManager keyManager, int maxBoxes)
        {
            _keyManager = keyManager;
            _maxBoxes = maxBoxes;
        }

        public string GetHMAC(int round) => _keyManager.GetHMAC(round);

        public bool ShouldRemoveBoxes(int selectedBox, int portalGunBox)
        {
            Console.WriteLine("Morty: You know what Rick? I'm not gonna make this easy for you. No box removal today!");
            return false;
        }

        public int[] SelectBoxesToKeep(int selectedBox, int portalGunBox, int totalBoxes, KeyManager keyManager)
        {
            return Enumerable.Range(0, totalBoxes).ToArray();
        }

        public double CalculateProbability(bool switching, int totalBoxes)
        {
            return 1.0 / totalBoxes;
        }
    }
}