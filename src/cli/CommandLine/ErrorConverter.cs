using Timesheet.CommandLine.Exceptions;

using CommandLineSource = CommandLine;

namespace Timesheet.CommandLine;

public class ErrorConverter
{
    // public static OptionParseException Convert(Error error)
    // {
    //     return error switch
    //     {
    //         NamedError namedError => new OptionParseException(namedError.NameInfo.NameText),
    //         TokenError tokenError => new OptionParseException(tokenError.Token),
    //         _ => throw new NotImplementedException()
    //     };
    // }
}
