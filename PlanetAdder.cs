using System;
using System.Collections.Generic;
using Harmony;
using Database;
using PlanetAdder.FileHandling;
using PlanetAdder.Logging;

namespace PlanetAdder
{
    [HarmonyPatch(typeof(StarmapScreen), "LoadPlanets")]
    public static class LoadsPlanets_Patch
    {
        private static LoggingFileHandler loggingFileHandler = null;
        private static ILogger logger = null;
        private static FileHandler fileHandler = null;

        private static void InitializeClasses()
        {
            // Create Logging File Handler
            loggingFileHandler = FileHandlerFactory.CreateLoggingFileHandler();

            // Create Logger
            logger = null;

            if (loggingFileHandler.IsLoggingEnabledFlag())
            {
                logger = LoggerFactory.CreateLogger();
            }
            else // Create fake logger to disable any logging
            {
                logger = LoggerFactory.CreateFakeLogger();
            }

            // Create File Handler - should be done before logger is setup to determine if logging is enabled
            fileHandler = FileHandlerFactory.CreateFileHandler(logger);
        }

        // LoadPlanets method prefix
        private static void Prefix()
        {
            logger.Begin();

            // Load the configuration file and get the planets to add
            List<Tuple<SpaceDestinationType, int, int>> planets = fileHandler.LoadConfig();

            if (planets.Count > 0)
            {
                logger.PrePatch(planets);

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

                    logger.Planet(spaceDestination.type.ToString());

#if DEBUG
                    // Complete research on the added planet
                    Game.Instance.spacecraftManager.destinationAnalysisScores.Add(uniqueId, 100);
#endif
                }

                logger.PostPatch();

                // Rename config file now that the planets have been created
                fileHandler.RenameConfigFile();

                logger.Complete();
            }
        }
	}
}
