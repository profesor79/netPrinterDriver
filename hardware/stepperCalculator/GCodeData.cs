using System.Collections.Generic;

namespace stepperCalculator
{
    public class GCodeData
    {
        public  Dictionary<string, double> Coordinates = new Dictionary<string, double>
        {
            {"X", -90000},
            {"Y", -90000},
            {"Z", -90000},
            {"E0", -90000},
            {"E1", -90000},
        };

        public GCodeData Clone()
        {
           var r = new GCodeData();
           foreach (var coordinate in Coordinates)
           {
               r.Coordinates[coordinate.Key] = coordinate.Value;
           }

           return r;
        }
    }
}
