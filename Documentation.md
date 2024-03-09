<a id='top'></a>
# Assembly: Dbarone.Net.Csv
## Contents
- [CsvConfiguration](#dbaronenetcsvcsvconfiguration)
  - [LineSeparator](#dbaronenetcsvcsvconfigurationlineseparator)
  - [FieldSeparator](#dbaronenetcsvcsvconfigurationfieldseparator)
  - [FieldEscape](#dbaronenetcsvcsvconfigurationfieldescape)
  - [HasHeader](#dbaronenetcsvcsvconfigurationhasheader)
  - [Culture](#dbaronenetcsvcsvconfigurationculture)
  - [Headers](#dbaronenetcsvcsvconfigurationheaders)
  - [ProcessRowErrorHandler](#dbaronenetcsvcsvconfigurationprocessrowerrorhandler)
  - [ProcessRowHandler](#dbaronenetcsvcsvconfigurationprocessrowhandler)
  - [RowProcessIgnoreBlank](#dbaronenetcsvcsvconfigurationrowprocessignoreblank)
  - [RowProcessError](#dbaronenetcsvcsvconfigurationrowprocesserror)
  - [RowProcessDefault](#dbaronenetcsvcsvconfigurationrowprocessdefault)
- [CsvReader](#dbaronenetcsvcsvreader)
  - [#ctor](#dbaronenetcsvcsvreader#ctor(systemiostream,dbaronenetcsvcsvconfiguration))
  - [#ctor](#dbaronenetcsvcsvreader#ctor(systemiostream))
  - [Read](#dbaronenetcsvcsvreaderread)
- [CsvWriter](#dbaronenetcsvcsvwriter)
  - [#ctor](#dbaronenetcsvcsvwriter#ctor(systemiostream,dbaronenetcsvcsvconfiguration))
  - [#ctor](#dbaronenetcsvcsvwriter#ctor(systemiostream))
  - [Write](#dbaronenetcsvcsvwriterwrite(systemcollectionsgenericienumerable{systemcollectionsgenericidictionary{systemstring,systemobject}}))
- [ProcessRowDelegate](#dbaronenetcsvprocessrowdelegate)
- [StringParser](#dbaronenetcsvstringparser)
  - [#ctor](#dbaronenetcsvstringparser#ctor(systemstring))
  - [Match](#dbaronenetcsvstringparsermatch(systemnullable{systemchar}))
  - [Read](#dbaronenetcsvstringparserread)
  - [Peek](#dbaronenetcsvstringparserpeek)
  - [Eof](#dbaronenetcsvstringparsereof)
- [Tokeniser](#dbaronenetcsvtokeniser)
  - [IsEOF](#dbaronenetcsvtokeniseriseof)
  - [LinesLastProcessed](#dbaronenetcsvtokeniserlineslastprocessed)
  - [#ctor](#dbaronenetcsvtokeniser#ctor(dbaronenetcsvcsvconfiguration))
  - [#ctor](#dbaronenetcsvtokeniser#ctor)
  - [GetLine](#dbaronenetcsvtokenisergetline(systemiostreamreader))
  - [Tokenise](#dbaronenetcsvtokenisertokenise(systemiostreamreader))
- [CsvException](#csvexception)
  - [#ctor](#csvexception#ctor(systemstring))



---
>## <a id='dbaronenetcsvcsvconfiguration'></a>type: CsvConfiguration
### Namespace:
`Dbarone.Net.Csv`
### Summary
 Provides configuration for a CsvParser. 

### Type Parameters:
None

>### <a id='dbaronenetcsvcsvconfigurationlineseparator'></a>property: LineSeparator
#### Summary
 Gets / sets the line separator string. Defaults to CRLF for non unix systems, LF for unix systems. 


<small>[Back to top](#top)</small>
>### <a id='dbaronenetcsvcsvconfigurationfieldseparator'></a>property: FieldSeparator
#### Summary
 Gets / sets the field separator. Defaults to comma character (%x2C) per rfc4180 specification. 


<small>[Back to top](#top)</small>
>### <a id='dbaronenetcsvcsvconfigurationfieldescape'></a>property: FieldEscape
#### Summary
 Gets / sets an optional field escape character. Defaults to double-quote character (%x22) per rfc4180 specification 


<small>[Back to top](#top)</small>
>### <a id='dbaronenetcsvcsvconfigurationhasheader'></a>property: HasHeader
#### Summary
 Gets / sets whether the Csv file has a header line. The default is true. If the Csv file does not have headers then column names: 'Column1', 'Column2', 'Column3', ... are used. 


<small>[Back to top](#top)</small>
>### <a id='dbaronenetcsvcsvconfigurationculture'></a>property: Culture
#### Summary
 Gets / sets the culture. Defaults to InvariantCulture. 


<small>[Back to top](#top)</small>
>### <a id='dbaronenetcsvcsvconfigurationheaders'></a>property: Headers
#### Summary
 Customer header specification. 


<small>[Back to top](#top)</small>
>### <a id='dbaronenetcsvcsvconfigurationprocessrowerrorhandler'></a>property: ProcessRowErrorHandler
#### Summary
 Callback function for processing of invalid data rows. 


<small>[Back to top](#top)</small>
>### <a id='dbaronenetcsvcsvconfigurationprocessrowhandler'></a>property: ProcessRowHandler
#### Summary
 Callback function for process a data row 


<small>[Back to top](#top)</small>
>### <a id='dbaronenetcsvcsvconfigurationrowprocessignoreblank'></a>field: RowProcessIgnoreBlank
#### Summary
 InvalidRowDelegate to ignore blank rows in the Csv file. 


<small>[Back to top](#top)</small>
>### <a id='dbaronenetcsvcsvconfigurationrowprocesserror'></a>field: RowProcessError
#### Summary
 Row handler that errors on each row. 


<small>[Back to top](#top)</small>
>### <a id='dbaronenetcsvcsvconfigurationrowprocessdefault'></a>field: RowProcessDefault
#### Summary
 Default row handler that does no proessing, but leaves the values as is. 


<small>[Back to top](#top)</small>

---
>## <a id='dbaronenetcsvcsvreader'></a>type: CsvReader
### Namespace:
`Dbarone.Net.Csv`
### Summary
 Reads a csv file per https://datatracker.ietf.org/doc/html/rfc4180. 

### Type Parameters:
None

>### <a id='dbaronenetcsvcsvreader#ctor(systemiostream,dbaronenetcsvcsvconfiguration)'></a>method: #ctor
#### Signature
``` c#
CsvReader.#ctor(System.IO.Stream stream, Dbarone.Net.Csv.CsvConfiguration configuration)
```
#### Summary
 Creates a new configured CsvReader instance from a stream and configueration. 

#### Type Parameters:
None

#### Parameters:
|Name | Description |
|-----|------|
|stream: |The csv stream to read.|
|configuration: |The csv configuration.|

#### Exceptions:
None
#### Examples:
None

<small>[Back to top](#top)</small>
>### <a id='dbaronenetcsvcsvreader#ctor(systemiostream)'></a>method: #ctor
#### Signature
``` c#
CsvReader.#ctor(System.IO.Stream stream)
```
#### Summary
 Creates a new configured CsvReader instance from a stream. 

#### Type Parameters:
None

#### Parameters:
|Name | Description |
|-----|------|
|stream: |The csv stream to read.|

#### Exceptions:
None
#### Examples:
None

<small>[Back to top](#top)</small>
>### <a id='dbaronenetcsvcsvreaderread'></a>method: Read
#### Signature
``` c#
CsvReader.Read()
```
#### Summary
 Reads a csv file. 

#### Type Parameters:
None

#### Parameters:
None

#### Exceptions:

Exception thrown: [T:CsvException](#T:CsvException): Throws an exception under various error conditions.

#### Examples:
None

<small>[Back to top](#top)</small>

---
>## <a id='dbaronenetcsvcsvwriter'></a>type: CsvWriter
### Namespace:
`Dbarone.Net.Csv`
### Summary
 Writes a csv file per https://datatracker.ietf.org/doc/html/rfc4180. 

### Type Parameters:
None

>### <a id='dbaronenetcsvcsvwriter#ctor(systemiostream,dbaronenetcsvcsvconfiguration)'></a>method: #ctor
#### Signature
``` c#
CsvWriter.#ctor(System.IO.Stream stream, Dbarone.Net.Csv.CsvConfiguration configuration)
```
#### Summary
 Creates a new configured CsvWriter instance from a stream and configuration. 

#### Type Parameters:
None

#### Parameters:
|Name | Description |
|-----|------|
|stream: |The csv stream to write to.|
|configuration: |The csv configuration.|

#### Exceptions:
None
#### Examples:
None

<small>[Back to top](#top)</small>
>### <a id='dbaronenetcsvcsvwriter#ctor(systemiostream)'></a>method: #ctor
#### Signature
``` c#
CsvWriter.#ctor(System.IO.Stream stream)
```
#### Summary
 Creates a new configured CsvWriter instance from a stream. 

#### Type Parameters:
None

#### Parameters:
|Name | Description |
|-----|------|
|stream: |The csv stream to write to.|

#### Exceptions:
None
#### Examples:
None

<small>[Back to top](#top)</small>
>### <a id='dbaronenetcsvcsvwriterwrite(systemcollectionsgenericienumerable{systemcollectionsgenericidictionary{systemstring,systemobject}})'></a>method: Write
#### Signature
``` c#
CsvWriter.Write(System.Collections.Generic.IEnumerable<System.Collections.Generic.IDictionary<System.String,System.Object>> data)
```
#### Summary
 Writes a csv file. 

#### Type Parameters:
None

#### Parameters:
|Name | Description |
|-----|------|
|data: |The data to write.|

#### Exceptions:

Exception thrown: [T:CsvException](#T:CsvException): Throws an exception under various error conditions.

#### Examples:
None

<small>[Back to top](#top)</small>

---
>## <a id='dbaronenetcsvprocessrowdelegate'></a>type: ProcessRowDelegate
### Namespace:
`Dbarone.Net.Csv`
### Summary
 Delegate for row processing. Allows the tokens to be checked or modified before being returned by the library. Set the tokens variable to null to ignore the row. 

### Type Parameters:
None


---
>## <a id='dbaronenetcsvstringparser'></a>type: StringParser
### Namespace:
`Dbarone.Net.Csv`
### Summary
 Provides parsing functions for a string similar to StreamReader. 

### Type Parameters:
None

>### <a id='dbaronenetcsvstringparser#ctor(systemstring)'></a>method: #ctor
#### Signature
``` c#
StringParser.#ctor(System.String str)
```
#### Summary
 Creates a new StringParser instance. 

#### Type Parameters:
None

#### Parameters:
|Name | Description |
|-----|------|
|str: |The input string.|

#### Exceptions:
None
#### Examples:
None

<small>[Back to top](#top)</small>
>### <a id='dbaronenetcsvstringparsermatch(systemnullable{systemchar})'></a>method: Match
#### Signature
``` c#
StringParser.Match(System.Nullable<System.Char> token)
```
#### Summary
 Matches a character. 

#### Type Parameters:
None

#### Parameters:
|Name | Description |
|-----|------|
|token: |The token char to match.|

#### Exceptions:
None
#### Examples:
None

<small>[Back to top](#top)</small>
>### <a id='dbaronenetcsvstringparserread'></a>method: Read
#### Signature
``` c#
StringParser.Read()
```
#### Summary
 Reads a character from the string, and advances the position. 

#### Type Parameters:
None

#### Parameters:
None

#### Exceptions:
None
#### Examples:
None

<small>[Back to top](#top)</small>
>### <a id='dbaronenetcsvstringparserpeek'></a>method: Peek
#### Signature
``` c#
StringParser.Peek()
```
#### Summary
 Peeks the next character without advancing the position. 

#### Type Parameters:
None

#### Parameters:
None

#### Exceptions:
None
#### Examples:
None

<small>[Back to top](#top)</small>
>### <a id='dbaronenetcsvstringparsereof'></a>property: Eof
#### Summary
 Returns true if EOF 


<small>[Back to top](#top)</small>

---
>## <a id='dbaronenetcsvtokeniser'></a>type: Tokeniser
### Namespace:
`Dbarone.Net.Csv`
### Summary
 Gets the csv tokens in a string. 

### Type Parameters:
None

>### <a id='dbaronenetcsvtokeniseriseof'></a>property: IsEOF
#### Summary
 Is set to true if the underlying stream is at end of stream. 


<small>[Back to top](#top)</small>
>### <a id='dbaronenetcsvtokeniserlineslastprocessed'></a>property: LinesLastProcessed
#### Summary
 Returns the number of lines processed by the immediately previous Tokenise() method call. 


<small>[Back to top](#top)</small>
>### <a id='dbaronenetcsvtokeniser#ctor(dbaronenetcsvcsvconfiguration)'></a>method: #ctor
#### Signature
``` c#
Tokeniser.#ctor(Dbarone.Net.Csv.CsvConfiguration configuration)
```
#### Summary
 Initialises a new Tokeniser instance from a configuration. 

#### Type Parameters:
None

#### Parameters:
|Name | Description |
|-----|------|
|configuration: |The configuration.|

#### Exceptions:
None
#### Examples:
None

<small>[Back to top](#top)</small>
>### <a id='dbaronenetcsvtokeniser#ctor'></a>method: #ctor
#### Signature
``` c#
Tokeniser.#ctor()
```
#### Summary
 Initialises a new Tokeniser instance. 

#### Type Parameters:
None

#### Parameters:
None

#### Exceptions:
None
#### Examples:
None

<small>[Back to top](#top)</small>
>### <a id='dbaronenetcsvtokenisergetline(systemiostreamreader)'></a>method: GetLine
#### Signature
``` c#
Tokeniser.GetLine(System.IO.StreamReader sr)
```
#### Summary
 Returns the next line, or final line if end of stream reached. Allow for non-standard line terminators to be specified. The Read() method of StringReader only recognises standard end of line markers line CRLF and LF. 

#### Type Parameters:
None

#### Parameters:
|Name | Description |
|-----|------|
|sr: |The StreamReader instance.|

#### Exceptions:
None
#### Examples:
None

<small>[Back to top](#top)</small>
>### <a id='dbaronenetcsvtokenisertokenise(systemiostreamreader)'></a>method: Tokenise
#### Signature
``` c#
Tokeniser.Tokenise(System.IO.StreamReader sr)
```
#### Summary
 Gets the tokens on the next record. Note that this may read more than 1 line of the file. 

#### Type Parameters:
None

#### Parameters:
|Name | Description |
|-----|------|
|sr: |The input stream reader.|

#### Exceptions:

Exception thrown: [T:CsvException](#T:CsvException): Throws an exception in certain error conditions.

#### Examples:
None

<small>[Back to top](#top)</small>

---
>## <a id='csvexception'></a>type: CsvException
### Namespace:
``
### Summary
 Exception class used for all exceptions in Dbarone.Net.Csv. 

### Type Parameters:
None

>### <a id='csvexception#ctor(systemstring)'></a>method: #ctor
#### Signature
``` c#
CsvException.#ctor(System.String message)
```
#### Summary
 Creates a new CsvException instance. 

#### Type Parameters:
None

#### Parameters:
|Name | Description |
|-----|------|
|message: |The error message.|

#### Exceptions:
None
#### Examples:
None

<small>[Back to top](#top)</small>
