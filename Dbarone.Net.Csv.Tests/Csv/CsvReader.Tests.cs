using Xunit;
using System.IO;
using System.Collections;
using System.Reflection.Metadata.Ecma335;
using System.Collections.Generic;
using System.Linq;

namespace Dbarone.Net.Csv.Tests;

public class CsvReaderTests
{

    public static IEnumerable<object[]> Datasets => new List<object[]> {
            new object[]{
                "..\\..\\..\\Datasets\\iris.data",
                false,
                150,
                new string[] {"sepal_length", "sepal_width", "petal_length", "petal_width", "species" }
            },

            new object[]{
                "..\\..\\..\\Datasets\\sakila\\sakila-csv\\film.csv",
                true,
                1000
            }

    };

    public static IEnumerable<object[]> CsvTestData => new List<object[]> {

        // Simple CSV
        new object[]{
@"field_name1,field_name2,field_name3
aaa,bbb,ccc
zzz,yyy,xxx",
3,
2
},

        // 1 record spans 2 lines
        new object[]{
@"field_name1,field_name2,field_name3
aaa,bbb,""ccc
ccc""
zzz,yyy,xxx",
3,
2
},

        // Blank line(s) at end
        new object[]{
@"field_name1,field_name2,field_name3
aaa,bbb,ccc
zzz,yyy,xxx

",
3,
2
},

    };

    [Theory, MemberData(nameof(CsvTestData))]
    public void TestValidCsvFiles(string csvFile, int expectedColumns, int expectedRecords)
    {
        // convert string to stream
        byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(csvFile);
        //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
        MemoryStream stream = new MemoryStream(byteArray);

        CsvReader csv = new CsvReader(stream);
        var results = csv.Read().ToList();

        Assert.Equal(expectedColumns, results.First().Count);
        Assert.Equal(expectedRecords, results.Count());
    }


    [Theory, MemberData(nameof(Datasets))]
    public void TestDatasets(string file, bool hasHeader, int expectedRecords, string[]? headers = null)
    {
        using (FileStream fs = File.Open(file, FileMode.Open))
        {
            CsvConfiguration configuration = new CsvConfiguration();

            if (!hasHeader)
            {
                configuration.HasHeader = hasHeader;
            }
            configuration.Headers = headers;

            CsvReader csv = new CsvReader(fs, configuration);
            var results = csv.Read().ToList();
            Assert.Equal(expectedRecords, results.Count());
        }
    }
}