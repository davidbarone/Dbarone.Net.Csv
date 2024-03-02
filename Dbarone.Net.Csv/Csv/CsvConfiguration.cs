using System.Diagnostics.Contracts;
using System.Globalization;

namespace Dbarone.Net.Csv;

/// <summary>
/// Provides configuration for a CsvParser.
/// </summary>
public class CsvConfiguration
{
    /// <summary>
    /// Gets / sets the line separator string. Defaults to CRLF for non unix systems, LF for unix systems.
    /// </summary>
    public string LineSeparator { get; set; } = Environment.NewLine;

    /// <summary>
    /// Gets / sets the field separator. Defaults to comma character (%x2C) per rfc4180 specification.
    /// </summary>
    public char FieldSeparator { get; set; } = ',';

    /// <summary>
    /// Gets / sets an optional field escape character. Defaults to double-quote character (%x22) per rfc4180 specification
    /// </summary>
    public char FieldEscape { get; set; } = '"';

    /// <summary>
    /// Gets / sets whether the Csv file has a header line. The default is true. If the Csv file does not have headers
    /// then column names: 'Column1', 'Column2', 'Column3', ... are used.
    /// </summary>
    public bool HasHeader { get; set; } = true;

    /// <summary>
    /// Gets / sets the culture. Defaults to InvariantCulture.
    /// </summary>
    public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;

    /// <summary>
    /// Customer header specification.
    /// </summary>
    public string[]? Headers { get; set; } = null;

    /// <summary>
    /// Callback function for processing of invalid data rows.
    /// </summary>
    public InvalidRowDelegate? InvalidRowHandler { get; set; } = CsvConfiguration.DefaultInvalidRowHandler;

    /// <summary>
    /// <see cref="InvalidRowDelegate"> to ignore blank records.
    /// </summary>
    /// <param name="record">The record number.</param>
    /// <param name="headers">The header array.</param>
    /// <param name="tokens">The record token array.</param>
    /// <returns>Returns a modified token array, or null if the record is to be ignored.</returns>
    public static InvalidRowDelegate IgnoreBlankRowsHandler = (int record, string[] headers, ref string[]? tokens) =>
    {
        if (tokens!.Length == 1 && tokens[0] == "")
        {
            // ignore blank rows
            tokens = null;
            return true;
        }
        else
        {
            return false;
        }
    };

    /// <summary>
    /// Default invalid row handler.
    /// </summary>
    public static InvalidRowDelegate DefaultInvalidRowHandler = (int record, string[] headers, ref string[]? tokens) => false;
}