namespace stepperCalculator
{
    public interface IPrinterCommand
    {
        // this interface helps us to have
        // list of commands that shall be executed
        CommandType  CommandType {  get; }
        object  CommandData { get; set; }
    }
}
