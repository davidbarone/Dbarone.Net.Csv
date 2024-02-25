using Xunit;
using System.IO;

namespace Dbarone.Net.Csv.Tests;

public class TokeniserTests
{
    [Theory]
    [InlineData("a", 1, 1)]
    public void TestValidRecords(string input, int expectedFieldCount, int expectedLinesRead)
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
        }
        finally
        {
            sr.Close();
        }
    }
}