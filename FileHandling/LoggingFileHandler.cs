using System;
using System.IO;

namespace PlanetAdder.FileHandling
{
    public class LoggingFileHandler
    {
        public bool IsLoggingEnabledFlag()
        {
            bool IsLoggingEnabled = false;
            string fileName = Path.Combine(FileHandler.GetDLLPath(), "EnableLogging.txt");

            try
            {
                using (TextReader reader = File.OpenText(fileName))
                {
                    Boolean.TryParse(reader.ReadLine(), out IsLoggingEnabled);

                    return IsLoggingEnabled;
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
