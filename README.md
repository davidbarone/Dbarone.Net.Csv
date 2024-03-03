# Dbarone.Net.Csv
Dbarone.Net.Csv is a simple CSV library that provides a CsvReader and CsvWriter class to read and write csv files. The parsing of csv is compliant to https://www.rfc-editor.org/rfc/rfc4180, and a full suite of unit tests is provided to validate the code.

## CsvReader
The CsvReader class provides a single `Read()` method to read csv files from any Stream object. The `Read()` method returns a sequence of StringDictionary objects

``` C#
    var input = @"field_name1,field_name2,field_name3
    aaa,bbb,ccc
    zzz,yyy,xxx";

    byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(input);
    MemoryStream stream = new MemoryStream(byteArray);
    CsvReader csv = new CsvReader(stream);
    results = csv.Read();
```

The `Read()` method will `yield` records. Note that if you want to read the entire csv file into memory, you will need to make a call such as `ToList()`.

## Configuration
By default, the CsvReader and CsvWriter class use the following defaults:

| Item            | Value                              |
| --------------- | ---------------------------------- |
| Field Separator | ,                                  |
| Field Escape    | "                                  |
| Line Separator  | LF on Linux, CRLF on other systems |

However, these settings can be configured in the `CsvConfiguration` object which is passed in the constructor to the `CsvReader` or `CsvWriter`. The following configuration is available:

| Item              | Description                                                                                                                        |
| ----------------- | ---------------------------------------------------------------------------------------------------------------------------------- |
| FieldSeparator    | Sets the field separator character. Defaults to ','                                                                                |
| FieldEscape       | Sets the field escape character. Defaults to '"'                                                                                   |
| LineSeparator     | Sets the line separator string. Defaults to LF on Linux, CRLF on other systems.                                                    |
| HasHeader         | Set to true if the csv file has headers. If no headers are present, headers 'Column1', 'Column2',... will be generated             |
| Headers           | Used to override the headers in the file or data.                                                                                  |
| Culture           | Sets the `CultureInfo` instance to define how values are formatted to / from strings                                               |
| InvalidRowHandler | You can define a custom callback function to process invalid data rows (including empty lines) that may be read from the csv file. |

## CsvWriter
Csv files can be created using the `CsvWriter` class. The `Write()` method will write a sequence of StringDictionary objects to a stream.

``` c#
    IEnumerable<StringDictionary> data = new IEnumerable<StringDictionary>();
    StringDictionary sd = new StringDictionary();
    sd.Add("foo", "123");
    sd.Add("bar", "456");
    sd.Add("baz", "789");
    data.Add(sd);

    MemoryStream ms = new MemoryStream();
    CsvWriter csv = new CsvWriter(ms, configuration);
    csv.Write(sd);
```

## Reading Csv Files to Strong-Typed objects
This library deals purely with parsing of CSV files. Csv files by nature are only text, and have no built-in information about types. This is why the `Read()` method returns a sequence of `StringDictionary` objects. It does not make any assumptions about the type of data being read. If you need to map the values into a class or other strong-typed structure, you can use the `Dbarone.Net.Mapper` library in conjunction with this library.

--- d.barone 3-Mar-2024 ---