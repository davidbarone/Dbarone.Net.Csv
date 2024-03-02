namespace Dbarone.Net.Csv;

/// <summary>
/// Delegate for invalid row processing.
/// </summary>
/// <param name="record">The current record number.</param>
/// <param name="headers">The headers.</param>
/// <param name="tokens">The tokens</param>
/// <returns>Return a modified set of tokens. To ignore the record, return null. Throw an exception to </returns>
public delegate string[]? InvalidRowDelegate(int record, string[] headers, string[] tokens);