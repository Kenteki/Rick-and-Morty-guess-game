using System;

namespace Rick_Morty_console_game.core
{
    public interface IMorty
    {
        string Name { get; }
        string GetHMAC(int round);
        bool ShouldRemoveBoxes(int selectedBox, int portalGunBox);
        int[] SelectBoxesToKeep(int selectedBox, int portalGunBox, int totalBoxes, KeyManager keyManager);
        double CalculateProbability(bool switching, int totalBoxes);
    }
}