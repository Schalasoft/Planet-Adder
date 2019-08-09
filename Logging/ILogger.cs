using Database;
using System.Collections.Generic;

namespace PlanetAdder.Logging
{
    public interface ILogger
    {
        void Log(string message);
        void InvalidPlanetName();
        void Begin();
        void Planet(string planet);
        void PrePatch(List<Tuple<SpaceDestinationType, int, int>> planets);
        void PostPatch();
        void Complete();
    }
}
