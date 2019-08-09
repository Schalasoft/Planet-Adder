using Database;
using System.Collections.Generic;

namespace PlanetAdder
{
    static class Logger
    {
        // CDG Logger class should not do anything if logging disabled, also read enable logging from text file
        // Make property get/set? 
        public static bool loggingEnabled = true;

        public static void Log(string message)
        {
            if(loggingEnabled) Debug.Log(message);
        }

        public static void InvalidPlanetName()
        {
            Log(" === LoadsPlanets_Patch Prefix === Invalid Planet Name entered in configuration file, defaulting to 'CarbonaceousAsteroid' (please see 'config - Example.txt' for names)");
        }

        public static void Begin()
        {
            Log(" === LoadsPlanets_Patch Prefix === ");
        }

        public static void Planet(string planet)
        {
            Log(" === Planet Adder | LoadsPlanets_Patch Prefix === " + planet);
        }

        public static void PrePatch(List<Tuple<SpaceDestinationType, int, int>> planets)
        {
            Log(" === Planet Adder | LoadsPlanets_Patch Prefix === Planets Created:");

            foreach (Tuple<SpaceDestinationType, int, int> planet in planets)
                Log(" === LoadsPlanets_Patch Prefix === 1 - " + planet.first.Name);


            Log("");
            Log(" === Planet Adder | LoadsPlanets_Patch Prefix === Original Destinations:");
            Log(" === Planet Adder | LoadsPlanets_Patch Prefix === Count: " + Game.Instance.spacecraftManager.destinations.Count);

            foreach (SpaceDestination destination in Game.Instance.spacecraftManager.destinations)
            {
                Log(" === Planet Adder | LoadsPlanets_Patch Prefix === " + destination.type + "(" + destination.id + ")");
            }

            Log("");
            Log(" === Planet Adder | LoadsPlanets_Patch Prefix === " + "Destinations Created:");
        }

        public static void PostPatch()
        {
            Log("");
            Log(" === Planet Adder | LoadsPlanets_Patch Prefix === " + "Updated Destinations:");
            Log(" === Planet Adder | LoadsPlanets_Patch Prefix === Count: " + Game.Instance.spacecraftManager.destinations.Count);

            foreach (SpaceDestination destination in Game.Instance.spacecraftManager.destinations)
            {
                Log(" === Planet Adder | LoadsPlanets_Patch Prefix === " + destination.type
                    + "(id: " + destination.id + ") "
                    + "(distance: " + destination.distance + ") "
                    + "(Starting Orbit %: " + destination.startingOrbitPercentage + ")");
            }

            Log("");
            Log(" === Planet Adder | LoadsPlanets_Patch Prefix === " + "Rename Config File");
        }

        public static void Complete()
        {
            Log("");
            Log(" === Planet Adder | LoadsPlanets_Patch Prefix === 6 - " + "COMPLETE");
        }
    }
}
