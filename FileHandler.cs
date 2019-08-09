using Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace PlanetAdder
{
    public static class FileHandler
    {
        public static string GetDLLPath()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            path = path.Replace("file:\\", ""); // May contain file ref key, so remove it if it exists
            return path;
        }

        // Converts text names of planets from config file to their respective types using reflection
        private static SpaceDestinationType convertSpaceDestinationType(SpaceDestinationTypes spaceDestinationTypes, String destinationType)
        {
            SpaceDestinationType spaceDestinationType = null;

            try
            {
                spaceDestinationType = (SpaceDestinationType)Activator.CreateInstance(typeof(SpaceDestinationTypes).GetNestedType(destinationType));
            }
            catch(Exception e)
            {
                Logger.InvalidPlanetName();
                spaceDestinationType = spaceDestinationTypes.CarbonaceousAsteroid;
            }

            return spaceDestinationType;
        }

        // Load config file of planets to add, 1 per line (planet type, row, column)
        public static List<Tuple<SpaceDestinationType, int, int>> LoadConfig(string path)
        {
            // List to hold planets
            List<Tuple<SpaceDestinationType, int, int>> planets = new List<Tuple<SpaceDestinationType, int, int>>();

            // Create path to the config file
            string fileName = Path.Combine(path, "config.txt");

            // Read all the lines
            if (File.Exists(fileName))
            {
                string[] lines = File.ReadAllLines(fileName, Encoding.UTF8);

                // Get the list of destination types for converting the text names from the config
                SpaceDestinationTypes spaceDestinationTypes = Db.Get().SpaceDestinationTypes;

                // Copy in each planet to add, 1 per line in format (type, row)
                foreach (String line in lines)
                {
                    // Split around comma
                    string[] parts = line.Split(',');

                    // Convert the text and get the type
                    SpaceDestinationType spaceDestinationType = convertSpaceDestinationType(spaceDestinationTypes, parts[0]);

                    // Add the type and the row number
                    planets.Add(new Tuple<SpaceDestinationType, int, int>(spaceDestinationType, int.Parse(parts[1]), int.Parse(parts[2])));
                }
            }

            return planets;
        }

        // Renames config file, this is done once the mod has added planets to a save from this config
        public static void RenameConfigFile(string path)
        {
            // Build file paths
            string replaceFileName = Path.Combine(path, "config.txt.old");
            string fileName = Path.Combine(path, "config.txt");

            // If there is already an "config.txt.old" then remove it
            if (File.Exists(replaceFileName)) File.Delete(replaceFileName);

            // Move (copy) config file
            File.Move(fileName, replaceFileName);

            // Delete original config file
            if (File.Exists(fileName)) File.Delete(fileName);
        }
    }
}
