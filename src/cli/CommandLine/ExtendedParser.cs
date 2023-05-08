using System.Reflection;
using CommandLine;

using CommandLineSource = CommandLine;

namespace Timesheet.CommandLine;

public class ExtendedParser
{
    private readonly Parser _parser;
    
    public ParserSettings Settings => _parser.Settings;
    
    public ExtendedParser(): this((settings) => {})
    { }    
    
    public ExtendedParser(Action<ParserSettings> configuration)
    {
        _parser = new Parser(configuration);
    }
    
    public ParserResult<T> ParseArguments<T>(IEnumerable<string> args)
    {
		List<string> argsList = new();
		List<OptionAttribute> options = new();

		string[] argsSnapshot = args.ToArray();
        ParserResult<T> result = _parser.ParseArguments<T>(argsSnapshot);
		
		IEnumerable<PropertyInfo> properties = typeof(T)
			.GetProperties(BindingFlags.Public | BindingFlags.Instance)
			.Where((p) => p.GetCustomAttribute<OptionAttribute>() is not null);
		
		// IEnumerable<OptionAttribute> options = typeof(T)
		// 	.GetCustomAttributes(typeof(OptionAttribute), true)
		// 	.Cast<OptionAttribute>();
		
		foreach (PropertyInfo property in properties) {
			if (property.PropertyType == typeof(bool)) continue;
			
			OptionAttribute option = property.GetCustomAttribute<OptionAttribute>()!;
			
			int indexOfShort = Array.IndexOf(argsSnapshot, option.ShortName);
			int indexOfLong = Array.IndexOf(argsSnapshot, option.LongName);
			
			if (indexOfShort == -1 && indexOfLong == -1) continue;
			
			int indexOfValue = Math.Max(indexOfShort, indexOfLong) + 1;
			
			// if (indexOfValue >= argsSnapshot.Length) 
			// 	result.Errors.Add(new MissingOptionValueErrorAdaptor(new NameInfoAdapter(option.LongName, option.ShortName)));
			
			// if (indexOfValue >= argsSnapshot.Length) 
			// 	result.Errors.Add(new TokenError(argsSnapshot[^1], ErrorType.MissingValueError));
				
			// CommandLineSource.NameInfo
		}
		
		return result;
	}
    
    public ParserResult<T> ParseArguments<T>(Func<T> factory, IEnumerable<string> args)
    {
		string[] argsArray = args.ToArray();
        ParserResult<T> result = _parser.ParseArguments<T>(factory, argsArray);
		return result;
	}
    
    
    private static readonly ExtendedParser _default = new ExtendedParser();
    
    public static ExtendedParser Default => _default;
}

class ParserResultAdapter<T>
{
	public ParserResultType Tag { get; }
	CommandLineSource.TypeInfo TypeInfo { get; }
	public T Value { get; }
	public IEnumerable<Error> Errors { get; }
}

class MissingOptionValueErrorAdaptor : Error
{
	public NameInfoAdapter Name { get; }
	
	public MissingOptionValueErrorAdaptor(NameInfoAdapter name): base(ErrorType.MissingValueOptionError)
	{
		Name = name;
	}
}

class NameInfoAdapter
{
	public string ShortName { get; }
	
	public string LongName { get; }
	
	public string NameText { 
		get  {
			return 
				ShortName.Length > 0 && LongName.Length > 0
					? ShortName + ", " + LongName
                : ShortName.Length > 0
                    ? ShortName
                : LongName;
		}
	}
	
	public NameInfoAdapter(string shortName, string longName)
	{
		if (shortName == null) throw new ArgumentNullException("shortName");
        if (longName == null) throw new ArgumentNullException("longName");

		this.ShortName = shortName;
		this.LongName = longName;
	}
}