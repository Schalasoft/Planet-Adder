using System;
using System.Collections.Generic;
using Harmony;
using Database;

namespace PlanetAdder
{
    [HarmonyPatch(typeof(StarmapScreen), "LoadPlanets")]
    public static class LoadsPlanets_Patch
    {
        /*
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
        */

        // LoadPlanets method prefix
        private static void Prefix()
        {
            // Setup path to DLL for writing config
            string path = FileHandler.GetDLLPath();

            Logger.Begin();

            // Load the configuration file and get the planets to add
            List<Tuple<SpaceDestinationType, int, int>> planets = FileHandler.LoadConfig(path);

            if (planets.Count > 0)
            {
                Logger.PrePatch(planets);

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

                    Logger.Planet(spaceDestination.type.ToString());

#if DEBUG
                    // Complete research on the added planet
                    Game.Instance.spacecraftManager.destinationAnalysisScores.Add(uniqueId, 100);
#endif
                }

                Logger.PostPatch();

                // Rename config file now that the planets have been created
                FileHandler.RenameConfigFile(path);

                Logger.Complete();
            }
        }
	}
}
