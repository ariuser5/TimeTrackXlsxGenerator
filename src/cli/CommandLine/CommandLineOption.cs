using CommandLine;

namespace Timesheet.CommandLine;

class CommandLineOption<T>
{
    public CommandLineOption(OptionAttribute meta)
    {
        Meta = meta;
        Value = default;
    }
    
    public CommandLineOption(OptionAttribute meta, T? value)
    {
        Meta = meta;
        Value = value;
    }
    
    public OptionAttribute Meta { get; }
    public T? Value { get; }
}
