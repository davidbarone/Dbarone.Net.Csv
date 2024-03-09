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

    public IEnumerable<IDictionary<string, object>> GetMultilineRecord(string lineSeparator)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("foo", $"123{lineSeparator}aaa");
        dict.Add("bar", "456");
        dict.Add("baz", "789");
        yield return dict;
    }

    public IEnumerable<IDictionary<string, object>> GetOneRecord()
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("foo", "123");
        dict.Add("bar", "456");
        dict.Add("baz", "789");
        yield return dict;
    }

    public IEnumerable<IDictionary<string, object>> GetTwoRecords()
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("foo", "123");
        dict.Add("bar", "456");
        dict.Add("baz", "789");
        yield return dict;

        dict = new Dictionary<string, object>();
        dict.Add("foo", "aaa");
        dict.Add("bar", "bbb");
        dict.Add("baz", "ccc");
        yield return dict;
    }

    [Theory]
    [InlineData(',', '"', "\r\n", "foo,bar,baz\r\n123,456,789\r\n")]
    [InlineData('|', '"', ";;", "foo|bar|baz;;123|456|789;;")]
    public void TestValidWrite1(char fieldSeparator, char fieldEscape, string lineDelimiter, string expected)
    {
        var sd = GetOneRecord();
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

    [Theory]
    [InlineData(',', '"', "\r\n", "foo,bar,baz\r\n123,456,789\r\naaa,bbb,ccc\r\n")]
    [InlineData('|', '"', ";;", "foo|bar|baz;;123|456|789;;aaa|bbb|ccc;;")]
    public void TestValidWrite2(char fieldSeparator, char fieldEscape, string lineDelimiter, string expected)
    {
        var sd = GetTwoRecords();
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

    [Theory]
    [InlineData(',', '"', "\r\n", "foo,bar,baz\r\n\"123\r\naaa\",456,789\r\n")]
    [InlineData('|', '"', ";;", "foo|bar|baz;;\"123;;aaa\"|456|789;;")]
    public void TestMultiLineWrite(char fieldSeparator, char fieldEscape, string lineDelimiter, string expected)
    {
        var sd = GetMultilineRecord(lineDelimiter);
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