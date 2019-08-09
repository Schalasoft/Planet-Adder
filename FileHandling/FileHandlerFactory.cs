using PlanetAdder.Logging;

namespace PlanetAdder.FileHandling
{
    static class FileHandlerFactory
    {
        public static FileHandler CreateFileHandler(ILogger logger)
        {
            return new FileHandler(logger);
        }

        public static LoggingFileHandler CreateLoggingFileHandler()
        {
            return new LoggingFileHandler();
        }
    }
}
