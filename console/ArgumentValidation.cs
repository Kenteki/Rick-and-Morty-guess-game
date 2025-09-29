using System;

namespace Rick_Morty_console_game.console
{
    public class ArgumentValidation
    {
        public int AmountBoxes { get; private set; } = 0;
        public Type? MortyType { get; private set; }

        public bool CheckArguments(string[] args)
        {
            var fileValidation = new FileValidation();

            if (args.Length == 0)
            {
                Console.WriteLine("No arguments provided!\nTry first argument : 3\nSecond argument: ClassicMorty");
                return false;
            }

            if (args.Length == 1)
            {
                Console.WriteLine("Missing Morty class name!\nTry first argument : 3\nSecond argument: ClassicMorty");
                return false;
            }

            if (args.Length > 2)
            {
                Console.WriteLine("Too many arguments provided!\nTry first argument : 3\nSecond argument: ClassicMorty");
                return false;
            }
            if (!int.TryParse(args[0], out int number) || number <= 2)
            {
                Console.WriteLine("Number of boxes must be an integer greater than 2!");
                return false;
            }

            var mortyType = fileValidation.GetMortyType(args[1]);
            if (mortyType == null)
            {
                Console.WriteLine("Morty class " + args[1] + " not found! Or does not implement IMorty.");
                var availableTypes = fileValidation.GetAvailableMortyType();
                if (availableTypes.Length > 0)
                {
                    Console.WriteLine("Available implementations: " + string.Join(", ", availableTypes));
                }
                else
                {
                    Console.WriteLine("No Morty implementations found!");
                }
                return false;
            }

            AmountBoxes = number;
            MortyType = mortyType;
            return true;
        }
    }
}