using Database;
using System.Collections.Generic;

namespace PlanetAdder.Logging
{
    class FakeLogger : ILogger
    {
        public void Log(string message) { }
        public void InvalidPlanetName() { }
        public void Begin() { }
        public void Planet(string planet) { }
        public void PrePatch(List<Tuple<SpaceDestinationType, int, int>> planets) { }
        public void PostPatch() { }
        public void Complete() { }
    }
}
