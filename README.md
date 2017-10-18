# ArgumentParser

A modular, extendable, and (hopefully) easy to use command line argument parser.

## Installation

Add this ArgumentParsing library to your application.

## Usage

1. Instantiate whatever types of arguments your program will accept with the allowable argument name(s):  
	Ex.  
    ```
    var helpArgument = new Argument(new List<string> { "help", "h", "?" }) { IsRequired = false }; //The Argument class is for flags (no associated value)  
    ```
    
    ```	
    var fileNameArgument = new StringArgument(new List<string> { "fileName", "file" });  
    ```
    
    ```
    var fileTypeArgument = new CharArgument(new List<string> { "fileType", "type" });
    ```
2. Instatiate the ArgumentParser  
	Ex. 
    ```
    var argumentParser = new ArgumentParser();
    ```
    
3. Pass the raw command line arguments, as well as your instantiated arguments, into the Parse method of the ArgumentParser  
	Ex. 
    ```
    var allowedArguments = new List<IArgument> { helpArgument, fileNameArgument, fileTypeArgument}; 
    ```
    
    ```
    var result = argumentParser.Parse(args, allowedArguments);
    ```
    
4. The ArgumentParser will populate the ParsedSuccessfully, ParsedArgumentName, and, when applicable, the ParsedArgumentValue. You can use these values as you like when parsing has finished.  
	Ex.  
    ```
    if (helpArgument.ParsedSuccessfully || !result.ParsingSuccessful)  
    {  
        var usage = argumentParser.GetUsage(ProgramDescription, allowedArguments);  
        Console.WriteLine(usage);  
    }
    ```
5. See the TestConsoleApp for more examples.
 

## Contributing

1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request :D

Note: Don't forget your Unit Tests :).

## Credits

Kevin Neumann

## License
MIT License

Copyright (c) 2017 

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
