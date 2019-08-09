using System;
using System.IO;

namespace PlanetAdder.FileHandling
{
    public class LoggingFileHandler
    {
        public bool IsLoggingEnabledFlag()
        {
            string fileName = Path.Combine(FileHandler.GetDLLPath(), "EnableLogging.txt");

            try
            {
                using (TextReader reader = File.OpenText(fileName))
                {
                    string line = reader.ReadLine();

                    return (line == "1");
                }
            }
            catch (Exception ex)
            {
                if (ex is FileNotFoundException || ex is NullReferenceException)
                {
                    return false;
                }

                throw;
            }
        }
    }
}
