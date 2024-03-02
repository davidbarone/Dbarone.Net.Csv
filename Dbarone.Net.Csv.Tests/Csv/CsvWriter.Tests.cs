using Xunit;
using System.IO;
using System.Collections;
using System.Reflection.Metadata.Ecma335;
using System.Collections.Generic;
using System.Linq;
using System;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Text;

namespace Dbarone.Net.Csv.Tests;

public class CsvWriterTests
{

    public IEnumerable<StringDictionary> GetSimpleStringDictionary()
    {
        StringDictionary sd = new StringDictionary();
        sd.Add("foo", "123");
        sd.Add("bar", "456");
        sd.Add("baz", "789");
        yield return sd;
    }

    [Theory]
    [InlineData(',', '"', "\r\n", "foo,bar,baz\r\n123,456,789\r\n")]
    [InlineData('|', '"', ";;", "foo|bar|baz;;123|456|789;;")]
    public void TestValidWrite(char fieldSeparator, char fieldEscape, string lineDelimiter, string expected)
    {
        var sd = GetSimpleStringDictionary();
        CsvConfiguration configuration = new CsvConfiguration();
        configuration.FieldEscape = fieldEscape;
        configuration.FieldSeparator = fieldSeparator;
        configuration.LineSeparator = lineDelimiter;
        configuration.Headers = new string[] { "foo", "bar", "baz" };

        MemoryStream ms = new MemoryStream();
        CsvWriter csv = new CsvWriter(ms, configuration);
        csv.Write(sd);

        var actual = Encoding.UTF8.GetString(ms.ToArray());
        Assert.Equal(expected, actual);
    }
}