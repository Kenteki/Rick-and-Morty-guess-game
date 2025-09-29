using Rick_Morty_console_game.console;
using Rick_Morty_console_game.core;
using System;

class Core_Console
{
    static void Main(string[] args)
    {
        try
        {
            var argumentValidation = new ArgumentValidation();
            if (!argumentValidation.CheckArguments(args)) return;

            Engine engine = new Engine();
            engine.Game(argumentValidation.AmountBoxes, argumentValidation.MortyType!);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}