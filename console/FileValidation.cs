using Rick_Morty_console_game.core;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Rick_Morty_console_game.console
{
    public class FileValidation
    {
        private readonly string pathToMods = Path.Combine(Directory.GetCurrentDirectory(), "mods");
        public Type? GetMortyType(string fileName)
        {
            string fileWithoutExt = Path.GetFileNameWithoutExtension(fileName);

            var dllType = GetTypeFromDll(fileWithoutExt);
            if (dllType != null) return dllType;


            var csType = GetTypeFromCs(fileWithoutExt);
            if (csType != null) return csType;

            return null;
        }
        private Type? GetTypeFromDll(string className)
        {
            string modsPath = Path.Combine(Directory.GetCurrentDirectory(), "mods");
            if (!Directory.Exists(modsPath)) return null;

            var dllFiles = Directory.GetFiles(modsPath, "*.dll");
            foreach (var dll in dllFiles)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(dll);
                    var type = assembly.GetTypes()
                        .FirstOrDefault(t => t.Name.Equals(className, StringComparison.OrdinalIgnoreCase)
                                        && typeof(IMorty).IsAssignableFrom(t));
                    if (type != null) return type;
                }
                catch {}
            }
            return null;
        }

        private Type? GetTypeFromCs(string fileWithoutExt)
        {
            string csPath = Path.Combine(pathToMods, fileWithoutExt + ".cs");
            if (!File.Exists(csPath)) return null;

            var assembly = Assembly.GetExecutingAssembly();
            var type = assembly.GetTypes()
                .FirstOrDefault(t => t.Name.Equals(fileWithoutExt, StringComparison.OrdinalIgnoreCase)
                                     && typeof(IMorty).IsAssignableFrom(t));
            return type;
        }
        public void GetModsFiles()
        {
            if (!Directory.Exists(pathToMods))
            {
                Console.WriteLine("Folder 'mods' not found!");
                return;
            }

            var files = Directory.GetFiles(pathToMods, "*.*")
                .Where(f => f.EndsWith(".cs") || f.EndsWith(".dll"))
                .Select(f => Path.GetFileNameWithoutExtension(f));

            if (!files.Any())
                Console.WriteLine("No mods found.");
            else
                Console.WriteLine("Available mods: " + string.Join(", ", files));
        }
    }
}
