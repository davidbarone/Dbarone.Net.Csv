namespace Dbarone.Net.Csv;

/// <summary>
/// Delegate for invalid row processing.
/// </summary>
/// <param name="record">The current record number.</param>
/// <param name="headers">The headers.</param>
/// <param name="tokens">The tokens. The tokens can be modified.</param>
/// <returns>An exception will be thrown if false is returned.</returns>
public delegate bool InvalidRowDelegate(int record, string[] headers, ref string[]? tokens);