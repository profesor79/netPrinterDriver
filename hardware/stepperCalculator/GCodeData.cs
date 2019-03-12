using System.Collections.Generic;

namespace stepperCalculator
{
    public class GCodeData
    {
        public  Dictionary<PrinterAxis, double> Coordinates = new Dictionary<PrinterAxis, double>
        {
            {PrinterAxis.X, -90000},
            {PrinterAxis.Y, -90000},
            {PrinterAxis.Z, -90000},
            {PrinterAxis.E, -90000},

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
