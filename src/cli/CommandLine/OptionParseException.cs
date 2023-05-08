using System.Reflection;
using CommandLine;

using CommandLineSource = CommandLine;

namespace Timesheet.CommandLine;

class OptionParseException : Exception
{
	public OptionParseException() : base() { }
	
	public OptionParseException(string? message) : base(message) { }
	
	public OptionParseException(string? message, Exception? innerException) : base(message, innerException) { }
	
	public static OptionParseException? From(Error error)
	{
		return error switch
		{
			HelpRequestedError helpRequestedError => null,
			VersionRequestedError versionRequestedError => null,
			HelpVerbRequestedError helpVerbRequestedError => null,
			SetValueExceptionError setValueExceptionError 
				=> new OptionParseException(
					message: $"Invalid value for option '{setValueExceptionError.NameInfo.NameText}'. " +
							 $"Exception: {setValueExceptionError.Exception.Message}.",
					innerException: setValueExceptionError.Exception),
			NamedError namedError => new OptionParseException($"{error} for option '{namedError.NameInfo.NameText}'."),
			TokenError tokenError => new OptionParseException($"{error} for token '{tokenError.Token}'."),
			_ => new OptionParseException(error.ToString())
		};
	}
	
	public static OptionParseException FromOptionProperty(object instance, string propertyName)
	{
		PropertyInfo? propertyInfo = instance.GetType().GetProperty(propertyName);
		
		if (propertyInfo is null)
			throw new ArgumentException(
				message: $"Property does not exist for object of type '{instance.GetType()}'.", 
				paramName: nameof(propertyName));
		
		OptionAttribute? optionAttribute = propertyInfo.GetCustomAttribute<OptionAttribute>(true);
		
		if (optionAttribute is null)
			throw new ArgumentException("Property is not an option.", nameof(propertyInfo));
		
		string nameText 
			= optionAttribute.ShortName is not null && optionAttribute.LongName is not null
				? $"'{optionAttribute.ShortName}' or '{optionAttribute.LongName}'"
			: optionAttribute.ShortName is not null
				? $"'{optionAttribute.ShortName}'"
			: optionAttribute.LongName is not null 
				? $"'{optionAttribute.LongName}'"
			: throw new ArgumentException("Option does not have a short or long name.");
		
		return new OptionParseException($"Invalid value for option {nameText}.");
	}
}