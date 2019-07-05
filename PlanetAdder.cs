using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using Database;
using System.Reflection;
using System.IO;

namespace PlanetAdder
{
    [HarmonyPatch(typeof(StarmapScreen), "LoadPlanets")]
    public static class LoadsPlanets_Patch
    {
        static bool debugEnabled = true;

        // local and steam paths are used for local development
        static string localPath = "C:\\Users\\ciand\\OneDrive\\Documents\\Klei\\OxygenNotIncluded\\mods\\dev\\Planet Adder\\";
        //static string steamPath = "C:\\Users\\ciand\\OneDrive\\Documents\\Klei\\OxygenNotIncluded\\mods\\Steam\\1749025321\\";
        static string path = "";

        // Converts text names of planets to their types
        private static SpaceDestinationType convertSpaceDestinationType(SpaceDestinationTypes spaceDestinationTypes, String destinationType)
        {
            SpaceDestinationType spaceDestinationType = null;

                switch(destinationType)
                {
                    case "Satellite":
                        spaceDestinationType = spaceDestinationTypes.Satellite;
                        break;
                    case "MetallicAsteroid":
                        spaceDestinationType = spaceDestinationTypes.MetallicAsteroid;
                        break;
                    case "RockyAsteroid":
                        spaceDestinationType = spaceDestinationTypes.RockyAsteroid;
                        break;
                    case "CarbonaceousAsteroid":
                        spaceDestinationType = spaceDestinationTypes.CarbonaceousAsteroid;
                        break;
                    case "IcyDwarf":
                        spaceDestinationType = spaceDestinationTypes.IcyDwarf;
                        break;
                    case "OrganicDwarf":
                        spaceDestinationType = spaceDestinationTypes.OrganicDwarf;
                        break;
                    case "TerraPlanet":
                        spaceDestinationType = spaceDestinationTypes.TerraPlanet;
                        break;
                    case "VolcanoPlanet":
                        spaceDestinationType = spaceDestinationTypes.VolcanoPlanet;
                        break;
                    case "GasGiant":
                        spaceDestinationType = spaceDestinationTypes.GasGiant;
                        break;
                    case "IceGiant":
                        spaceDestinationType = spaceDestinationTypes.IceGiant;
                        break;
                    case "DustyDwarf":
                        spaceDestinationType = spaceDestinationTypes.DustyMoon;
                        break;
                }

            return spaceDestinationType;
        }

        // Local debug for DLL
        public static void Test()
        {
            loadConfig();
            renameConfigFile();
        }

        // Load config file of planets to add, 1 per line (planet type, row, column)
        private static List<Tuple<SpaceDestinationType, int, int>> loadConfig()
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
        private static void renameConfigFile()
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

        private static void Prefix()
        {
            // Setup path to DLL for writing config
            path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            path = path.Replace("file:\\", ""); // May contain file ref key, so remove it if it exists

            if(debugEnabled) Debug.Log(" === LoadsPlanets_Patch Prefix === ");

            // Load the configuration file and get the planets to add
            List<Tuple<SpaceDestinationType, int, int>> planets = loadConfig();

            if (planets.Count > 0)
            {
                if (debugEnabled)
                {
                    Debug.Log(" === Planet Adder | LoadsPlanets_Patch Prefix === 1 - Planets Created:");

                    foreach (Tuple<SpaceDestinationType, int, int> planet in planets)
                        Debug.Log(" === LoadsPlanets_Patch Prefix === 1 - " + planet.first.Name);


                    Debug.Log("");
                    Debug.Log(" === Planet Adder | LoadsPlanets_Patch Prefix === 2 - " + "Original Destinations:");
                    Debug.Log(" === Planet Adder | LoadsPlanets_Patch Prefix === 2 - Count: " + Game.Instance.spacecraftManager.destinations.Count);

                    foreach (SpaceDestination destination in Game.Instance.spacecraftManager.destinations)
                    {
                        Debug.Log(" === Planet Adder | LoadsPlanets_Patch Prefix === 2 - " + destination.type + "(" + destination.id + ")");
                    }

                    Debug.Log("");
                    Debug.Log(" === Planet Adder | LoadsPlanets_Patch Prefix === 3 - " + "Destinations Created:");
                }

                // Add planets to destinations
                foreach (Tuple<SpaceDestinationType, int, int> planet in planets)
                {
                    // Planets are numbered from 0
                    int uniqueId = Game.Instance.spacecraftManager.destinations.Count;

                    // Create the destination using the type and row from the config
                    SpaceDestination spaceDestination = new SpaceDestination(uniqueId, planet.first.Id, planet.second);

                    // Set the starting orbit percentage (position in the row)
                    spaceDestination.startingOrbitPercentage = (float)Decimal.Divide(planet.third, 10);

                    // Add the destination
                    Game.Instance.spacecraftManager.destinations.Add(spaceDestination);

                    if (debugEnabled) Debug.Log(" === Planet Adder | LoadsPlanets_Patch Prefix === 3 - " + spaceDestination.type.ToString());

                    // CDG DEBUG - Auto show the planet
                    //Game.Instance.spacecraftManager.destinationAnalysisScores.Add(uniqueId, 100);
                }

                if (debugEnabled)
                {
                    Debug.Log("");
                    Debug.Log(" === Planet Adder | LoadsPlanets_Patch Prefix === 4 - " + "Updated Destinations:");
                    Debug.Log(" === Planet Adder | LoadsPlanets_Patch Prefix === 4 - Count: " + Game.Instance.spacecraftManager.destinations.Count);

                    foreach (SpaceDestination destination in Game.Instance.spacecraftManager.destinations)
                    {
                        Debug.Log(" === Planet Adder | LoadsPlanets_Patch Prefix === 4 - " + destination.type
                            + "(id:" + destination.id + ") "
                            + "(distance:" + destination.distance + ") "
                            + "(Starting Orbit %:" + destination.startingOrbitPercentage + ")");
                    }

                    Debug.Log("");
                    Debug.Log(" === Planet Adder | LoadsPlanets_Patch Prefix === 5 - " + "Rename Config File");
                }

                // Rename config file now that the planets have been created
                renameConfigFile();

                if (debugEnabled)
                {
                    Debug.Log("");
                    Debug.Log(" === Planet Adder | LoadsPlanets_Patch Prefix === 6 - " + "COMPLETE");
                }
            }
        }
	}
}
