using Database;
using PlanetAdder.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace PlanetAdder.FileHandling
{
    public class FileHandler
    {
        private ILogger logger = null;
        private string path = null;

        public string FilePath
        {
            get
            {
                return this.path;
            }
        }

        public FileHandler(ILogger logger)
        {
            this.logger = logger;

            // Initialize file path
            this.path = GetDLLPath();
        }

        public static string GetDLLPath()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            path = path.Replace("file:\\", ""); // May contain file ref key, so remove it if it exists
            return path;
        }

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
                    case "Wormhole":
                        spaceDestinationType = spaceDestinationTypes.Wormhole;
                        break;
                    case "SaltDwarf":
                        spaceDestinationType = spaceDestinationTypes.SaltDwarf;
                        break;
                    case "RustPlanet":
                        spaceDestinationType = spaceDestinationTypes.RustPlanet;
                        break;
                    case "ForestPlanet":
                        spaceDestinationType = spaceDestinationTypes.ForestPlanet;
                        break;
                    case "RedDwarf":
                        spaceDestinationType = spaceDestinationTypes.RedDwarf;
                        break;
                    case "GoldAsteroid":
                        spaceDestinationType = spaceDestinationTypes.GoldAsteroid;
                        break;
                    case "HydrogenGiant":
                        spaceDestinationType = spaceDestinationTypes.HydrogenGiant;
                        break;
                    case "OilyAsteroid":
                        spaceDestinationType = spaceDestinationTypes.OilyAsteroid;
                        break;
                    case "ShinyPlanet":
                        spaceDestinationType = spaceDestinationTypes.ShinyPlanet;
                        break;
                    case "ChlorinePlanet":
                        spaceDestinationType = spaceDestinationTypes.ChlorinePlanet;
                        break;
                    case "SaltDesertPlanet":
                        spaceDestinationType = spaceDestinationTypes.SaltDesertPlanet;
                        break;
                    case "Earth":
                        spaceDestinationType = spaceDestinationTypes.Earth;
                        break;
            }
            return spaceDestinationType;
        }

        // Load config file of planets to add, 1 per line (planet type, row, column)
        public List<Tuple<SpaceDestinationType, int, int>> LoadConfig()
        {
            // List to hold planets
            List<Tuple<SpaceDestinationType, int, int>> planets = new List<Tuple<SpaceDestinationType, int, int>>();

            // Create path to the config file
            string fileName = Path.Combine(this.path, "config.txt");

            // Read all the lines
            if (File.Exists(fileName))
            {
                string[] lines = File.ReadAllLines(fileName, Encoding.UTF8);
                
                // Get the list of destination types for converting the text names from the config
                SpaceDestinationTypes spaceDestinationTypes = Db.Get().SpaceDestinationTypes;

                // Copy in each planet to add, 1 per line in format (type, row)
                foreach (string line in lines)
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
        public void RenameConfigFile()
        {
            // Build file paths
            string replaceFileName = Path.Combine(this.path, "config.txt.old");
            string fileName = Path.Combine(this.path, "config.txt");

            // If there is already an "config.txt.old" then remove it
            if (File.Exists(replaceFileName))
            {
                File.Delete(replaceFileName);
            }

            // Move (copy) config file
            File.Move(fileName, replaceFileName);

            // Delete original config file
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
    }
}
