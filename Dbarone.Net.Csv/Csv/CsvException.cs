namespace Dbarone.Net.Csv;

/// <summary>
/// Exception class used for all exceptions in Dbarone.Net.Csv.
/// </summary>
public class CsvException : Exception
{
    /// <summary>
    /// Creates a new CsvException instance.
    /// </summary>
    /// <param name="message">The error message.</param>
    public CsvException(string? message) : base(message) { }
}