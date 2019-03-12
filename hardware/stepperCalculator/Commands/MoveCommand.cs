using System.Collections.Generic;

namespace stepperCalculator
{
    public class MoveCommand : IPrinterCommand
    {
        public MoveCommand(Dictionary<PrinterAxis, Movement> move)
        {
            CommandType = CommandType.Motion;
            CommandData = move;
        }

        public CommandType CommandType { get; }
        public object CommandData { get; set; }
    }
}