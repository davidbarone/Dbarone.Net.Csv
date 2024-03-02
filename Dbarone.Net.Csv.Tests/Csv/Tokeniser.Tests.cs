using Xunit;
using System.IO;
using Xunit.Sdk;

namespace Dbarone.Net.Csv.Tests;

public class TokeniserTests
{
    [Theory]
    [InlineData("aaa;;bbb", ";;", "aaa", "bbb")]
    [InlineData("aaa;;bbb;;ccc", ";;", "aaa", "bbb")]
    public void TestGetLine(string input, string lineSeparator, string expected1, string? expected2)
    {
        CsvConfiguration configuration = new CsvConfiguration
        {
            LineSeparator = lineSeparator
        };
        byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(input);
        MemoryStream stream = new MemoryStream(byteArray);
        StreamReader sr = new StreamReader(stream);

        Tokeniser tokeniser = new Tokeniser(configuration);
        if (!string.IsNullOrEmpty(expected1))
        {
            var actual1 = tokeniser.GetLine(sr);
            Assert.Equal(expected1, actual1);
        }
        if (!string.IsNullOrEmpty(expected2))
        {
            var actual2 = tokeniser.GetLine(sr);
            Assert.Equal(expected2, actual2);
        }
    }

    [Fact]
    public void TestNullString()
    {
        Tokeniser tokeniser = new Tokeniser();
        var ex = Assert.Throws<CsvException>(() => tokeniser.Tokenise(null));
        Assert.Equal("StreamReader is null!", ex.Message);
    }

    [Theory]
    [InlineData("a", 1, 1, null, null)]
    [InlineData("a,b,c", 3, 1, null, null)]
    [InlineData("a,\"b\",c", 3, 1, null, null)] // escape using double-quotes.
    [InlineData("a,\"b\"\"\",c", 3, 1, 1, "b\"")] // including quote in escaped value. 2nd column should be [b"].
    [InlineData("a,b, c ", 3, 1, 2, " c ")] // preserve any whitespace for the record.
    [InlineData("a,,c", 3, 1, 1, "")] // 2nd field is empty string.
    [InlineData(",,", 3, 1, 2, "")] // 3 empty strings.
    [InlineData(",,\n\n", 3, 1, 2, "\n\n")] // 3 empty strings with blank line at end with linux line endings.
    [InlineData("a,b,\"hello\r\nworld\"", 3, 2, 2, "hello\r\nworld")] // 3rd field spans 2 lines.
    public void TestValidRecords(string input, int expectedFieldCount, int expectedLinesRead, int? checkField, string? checkValue)
    {
        // convert string to stream
        byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(input);
        //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
        MemoryStream stream = new MemoryStream(byteArray);
        StreamReader sr = new StreamReader(stream);

        Tokeniser tokeniser = new Tokeniser();
        try
        {
            var tokens = tokeniser.Tokenise(sr);

            Assert.Equal(expectedFieldCount, tokens.Length);
            Assert.Equal(expectedLinesRead, tokeniser.LinesLastProcessed);
            if (checkField.HasValue)
            {
                Assert.Equal(checkValue, tokens[checkField.Value]);
            }
        }
        finally
        {
            sr.Close();
        }
    }

    [Theory]
    [InlineData("", "Unexpected EOF!")]    // empty string
    [InlineData("a,b,\"c", "Unexpected EOF!")]    // No terminating quote character after 'c'
    [InlineData("a,b\"B,c", "Unexpected field escape character found!")]    // Text escape character found midway through field
    public void TestInvalidRecords(string input, string expectedMessage)
    {
        // convert string to stream
        byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(input);
        //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
        MemoryStream stream = new MemoryStream(byteArray);
        StreamReader sr = new StreamReader(stream);

        Tokeniser tokeniser = new Tokeniser();
        try
        {
            var ex = Assert.Throws<CsvException>(() => tokeniser.Tokenise(sr));
            Assert.Equal(expectedMessage, ex.Message);
        }
        finally
        {
            sr.Close();
        }
    }
}