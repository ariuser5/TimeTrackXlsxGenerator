using CommandLine;

namespace Timesheet.CommandLine.Exceptions;

public class BadReferenceDateFormatError : TokenError
{
    public BadReferenceDateFormatError(string token): base(ErrorType.BadFormatTokenError, token)
	{ }
}