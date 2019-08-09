namespace PlanetAdder.Logging
{
    static class LoggerFactory
    {
        public static ILogger CreateLogger()
        {
            return new Logger();
        }

        public static ILogger CreateFakeLogger()
        {
            return new FakeLogger();
        }
    }
}
