namespace Dbarone.Net.Csv;

/// <summary>
/// Delegate for row processing. Allows the tokens to be checked or modified before being returned by the library. Set the tokens variable to null to ignore the row.
/// </summary>
/// <param name="record">The current record number.</param>
/// <param name="headers">The headers.</param>
/// <param name="values">The writeable array of values on the row. In a callback function, this array will be populated with the string values of the tokens
/// from the read csv record. The callback function is then able to modify these values, for example to cast or parse to the appropriate types and perform error-checking.</param>
/// <returns>An exception will be thrown if false is returned by the handler.</returns>
public delegate bool ProcessRowDelegate(int record, string[] headers, ref object[]? values);