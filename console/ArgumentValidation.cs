using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Console.WriteLine("No arguments! Use \"example\" as argument to see how to play.");
                return false;
            }

            switch (args[0].ToLower())
            {
                case "example":
                    Console.WriteLine("Example of command is: \n dotnet run 3 EvilMorty \n First argument is Amount of boxes \n Second argument is name of mod for \"Morthy Implementation\"");
                    Console.WriteLine("To see available mods type \"mods\"");
                    return false;

                case "mods":
                    Console.WriteLine("Available \"Morthy implementations\":");
                    fileValidation.GetModsFiles();
                    return false;

                case "github":
                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = "https://github.com/Kenteki/Rick-and-Morty-guess-game",
                            UseShellExecute = true
                        });

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    return false;

            }
            if (!int.TryParse(args[0], out int number) || number <= 2)
            {
                Console.WriteLine("Number of boxes must be an integer greater than 2!\nUse \"example\" as argument to see how to play.");
                return false;
            }
            if (args.Length == 1)
            {
                Console.WriteLine("Missing Morty class name!\nUse \"mods\" as argument to see how to play.");
                return false;
            }
            if (args.Length > 2)
            {
                Console.WriteLine("Too many arguments provided!\nUse \"example\" as argument to see how to play.");
                return false;
            }

            var mortyType = fileValidation.GetMortyType(args[1]);
            if (mortyType == null)
            {
                Console.WriteLine($"Morty class '{args[1]}' not found or does not implement IMorty.\n Use \"mods\" as argument to see available Morty class.");
                return false;
            }
            AmountBoxes = number;
            MortyType = mortyType;
            return true;
        }
    }

}