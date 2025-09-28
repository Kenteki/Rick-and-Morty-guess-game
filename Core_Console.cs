using Rick_Morty_console_game.console;
using Rick_Morty_console_game.core;

class Core_Console
{
    static void Main(string[] args)
    {
        try { 
        var argumentValidation = new ArgumentValidation();
        if(!argumentValidation.CheckArguments(args)) return;
        int amountOfBoxes = Convert.ToInt32(args[0]);
        Engine engine = new Engine();
        engine.Game(argumentValidation.AmountBoxes, argumentValidation.MortyType!);
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
    }
}