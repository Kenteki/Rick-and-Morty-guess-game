using Rick_Morty_console_game.core;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;


namespace Rick_Morty_console_game.console
{
    public class FileValidation
    {
        private readonly string pathToMods = Path.Combine(AppContext.BaseDirectory, "mods");

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
            string modsPath = pathToMods;
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
                catch { }
            }
            return null;
        }

        private Type? GetTypeFromCs(string fileWithoutExt)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var type = assembly.GetTypes()
                .FirstOrDefault(t => t.Name.Equals(fileWithoutExt, StringComparison.OrdinalIgnoreCase)
                                     && typeof(IMorty).IsAssignableFrom(t));
            return type;
        }

        public string[] GetAvailableMortyType()
        {
            var availableTypes = new List<string>();
            string modsPath = pathToMods;
            if (Directory.Exists(modsPath))
            {
                var dllFiles = Directory.GetFiles(modsPath, "*.dll");
                foreach (var dll in dllFiles)
                {
                    try
                    {
                        var assembly = Assembly.LoadFrom(dll);
                        var types = assembly.GetTypes()
                            .Where(t => typeof(IMorty).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                            .Select(t => t.Name);
                        availableTypes.AddRange(types);
                    }
                    catch { }
                }
            }
            var currentAssembly = Assembly.GetExecutingAssembly();
            var compiledTypes = currentAssembly.GetTypes()
                .Where(t => typeof(IMorty).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .Select(t => t.Name);
            availableTypes.AddRange(compiledTypes);

            return availableTypes.Distinct().ToArray();
        }
    }
}