/// <summary>
/// Exception class used for all exceptions in Dbarone.Net.Csv.
/// </summary>
public class CsvException : Exception
{
    public CsvException(string? message) : base(message) { }
}