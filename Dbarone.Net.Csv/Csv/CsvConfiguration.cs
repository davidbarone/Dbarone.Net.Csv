using System.Diagnostics.Contracts;
using System.Globalization;
using System.Net.NetworkInformation;

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
    public ProcessRowDelegate ProcessRowErrorHandler { get; set; } = CsvConfiguration.RowProcessError;

    /// <summary>
    /// Callback function for process a data row
    /// </summary>
    public ProcessRowDelegate ProcessRowHandler { get; set; } = CsvConfiguration.RowProcessDefault;

    /// <summary>
    /// InvalidRowDelegate to ignore blank rows in the Csv file.
    /// </summary>
    public static ProcessRowDelegate RowProcessIgnoreBlank = (int record, string[] headers, ref object[]? values) =>
    {
        if (values!.Length == 1 && (string)values[0] == "")
        {
            // ignore blank rows
            values = null;
            return true;
        }
        else
        {
            return false;
        }
    };

    /// <summary>
    /// Row handler that errors on each row.
    /// </summary>
    public static ProcessRowDelegate RowProcessError = (int record, string[] headers, ref object[]? values) => false;

    /// <summary>
    /// Default row handler that does no proessing, but leaves the values as is.
    /// </summary>
    public static ProcessRowDelegate RowProcessDefault = (int record, string[] headers, ref object[]? values) => true;
}