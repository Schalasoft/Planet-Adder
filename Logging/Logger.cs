using Database;
using System.Collections.Generic;

namespace PlanetAdder.Logging
{
    class Logger : ILogger
    {
        public void Log(string message)
        {
           Debug.Log(message);
        }

        public void InvalidPlanetName()
        {
            Log(" === LoadsPlanets_Patch Prefix === Invalid Planet Name entered in configuration file, defaulting to 'CarbonaceousAsteroid' (please see 'config - Example.txt' for names)");
        }

        public void Begin()
        {
            Log(" === LoadsPlanets_Patch Prefix === ");
        }

        public void Planet(string planet)
        {
            Log(" === Planet Adder | LoadsPlanets_Patch Prefix === " + planet);
        }

        public void PrePatch(List<Tuple<SpaceDestinationType, int, int>> planets)
        {
            Log(" === Planet Adder | LoadsPlanets_Patch Prefix === Planets Created:");

            foreach (Tuple<SpaceDestinationType, int, int> planet in planets)
            {
                Log(" === LoadsPlanets_Patch Prefix === 1 - " + planet.first.Name);
            }


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

        public void PostPatch()
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

        public void Complete()
        {
            Log("");
            Log(" === Planet Adder | LoadsPlanets_Patch Prefix === 6 - " + "COMPLETE");
        }
    }
}
