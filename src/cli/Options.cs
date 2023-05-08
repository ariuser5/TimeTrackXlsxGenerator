using System.Text.Json;

class Options
{
	public static class Defaults
	{
		public const string OutputLocation = ".";
		public const string UserName = "User";
		public static (int row, int col) StartCell = (4, 2);
		public const int RowsSpace = 7;
		public const int WorkHours = 8;
		public const string DescriptionPlaceholder = "<Desc>";
		public const string DateFormat = "dd.MM.yyyy";
	}
	
	private DateMonth? _fixedReferencedDate;
	private DateMonth _referencedDate;
	private string _outputLocation;
	private string _userName;
	private (int row, int col) _startCell;
	private int _rowsSpace;
	private int _workHours;
	private string _descriptionPlaceholder;
	private string _dateFormat;
	private string? _worksheetName;

	public Options() {
		_referencedDate = DateMonth.Now();
		_outputLocation = Defaults.OutputLocation;
		_userName = Defaults.UserName;
		_startCell = Defaults.StartCell;
		_rowsSpace = Defaults.RowsSpace;
		_workHours = Defaults.WorkHours;
		_descriptionPlaceholder = Defaults.DescriptionPlaceholder;
		_dateFormat = Defaults.DateFormat;
		_worksheetName = null;
	}

	public DateMonth? FixedReferencedDate {
		get => _fixedReferencedDate ?? _referencedDate;
		set => _fixedReferencedDate = value;
	}

	public DateMonth ReferencedDate {
		get => _referencedDate;
		set => _referencedDate = value;
	}

	public string OutputLocation {
		get => _outputLocation;
		set => _outputLocation = value;
	}

	public string UserName {
		get => _userName;
		set => _userName = value;
	}
	
	public (int row, int col) StartCell {
		get => _startCell;
		set => _startCell = value;
	}
	
	public int RowsSpace {
		get => _rowsSpace;
		set => _rowsSpace = value;
	}
	
	public int WorkHours {
		get => _workHours;
		set => _workHours = value;
	}
	
	public string DescriptionPlaceholder {
		get => _descriptionPlaceholder;
		set => _descriptionPlaceholder = value;
	}
	
	public string DateFormat {
		get => _dateFormat;
		set => _dateFormat = value;
	}
	
	public string? WorksheetName {
		get => _worksheetName;
		set => _worksheetName = value;
	}
	
	
	public string Serialize()
	{
		return JsonSerializer.Serialize(this, ConfigFile.DefaultJsonSerializationOptions);
	}
	

	public static implicit operator TimesheetOptions(
		Options options
	) => new TimesheetOptions(
		options.ReferencedDate,
		options.UserName,
		options.StartCell,
		options.RowsSpace,
		options.WorkHours,
		options.DescriptionPlaceholder,
		options.DateFormat,
		options.WorksheetName
	);
}