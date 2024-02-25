using Xunit;
using System.IO;
using System.Collections;
using System.Reflection.Metadata.Ecma335;
using System.Collections.Generic;
using System.Linq;

namespace Dbarone.Net.Csv.Tests;

public class CsvParserTests
{

    public static IEnumerable<object[]> CsvTestData => new List<object[]> {

        new object[]{
@"field_name1,field_name2,field_name3
aaa,bbb,ccc
zzz,yyy,xxx",
3,
2
}
    };

    [Theory, MemberData(nameof(CsvTestData))]
    public void TestValidCsvFiles(string csvFile, int expectedColumns, int expectedRecords)
    {
        // convert string to stream
        byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(csvFile);
        //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
        MemoryStream stream = new MemoryStream(byteArray);

        CsvParser csv = new CsvParser();
        var results = csv.Parse(stream).ToList();

        Assert.Equal(expectedColumns, results.First().Count);
        Assert.Equal(expectedRecords, results.Count());
    }
}