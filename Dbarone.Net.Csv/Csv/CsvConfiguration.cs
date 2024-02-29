using System.Diagnostics.Contracts;
using System.Globalization;

namespace Dbarone.Net.Csv;

/// <summary>
/// Provides configuration for a CsvParser.
/// </summary>
public class CsvConfiguration
{
    /// <summary>
    /// Gets / sets the line delimiter. Defaults to CRLF for non unix systems, LF for unix systems.
    /// </summary>
    public string LineDelimiter { get; set; } = Environment.NewLine;

    /// <summary>
    /// Gets / sets the field separator. Defaults to comma character (%x2C) per rfc4180 specification.
    /// </summary>
    public char FieldSeparator { get; set; } = ',';

    /// <summary>
    /// Gets / sets an optional field escape character. Defaults to double-quote character (%x22) per rfc4180 specification
    /// </summary>
    public char? FieldEscapeCharacter { get; set; } = '"';

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
}