# ArgumentParser

A modular, extendable, and (hopefully) easy to use command line argument parser.

## Installation

Add this ArgumentParsing library to your application.

## Usage

1. Instantiate whatever types of arguments your program will accept with the allowable argument name(s):
	Ex. `var helpArgument = new Argument(new List<string> { "help", "h", "?" }) { IsRequired = false }; //The Argument class is for flags (no associated value)
		var fileNameArgument = new StringArgument(new List<string> { "fileName", "file" });
		var fileTypeArgument = new CharArgument(new List<string> { "fileType", "type" });`
2. Instatiate the ArgumentParser
	Ex. `var argumentParser = new ArgumentParser();`
3. Pass the raw command line arguments, as well as your instantiated arguments, into the Parse method of the ArgumentParser
	Ex. `var allowedArguments = new List<IArgument> { helpArgument, fileNameArgument, fileTypeArgument};
		var result = argumentParser.Parse(args, allowedArguments);`
4. The ArgumentParser will populate the ParsedSuccessfully, ParsedArgumentName, and, when applicable, the ParsedArgumentValue. You can use these values as you like when parsing has finished. 
	Ex. `if (helpArgument.ParsedSuccessfully || !result.ParsingSuccessful)
            {
                var usage = argumentParser.GetUsage(ProgramDescription, allowedArguments);
                Console.WriteLine(usage);
            }`
5. See the TestConsoleApp for more examples.
 

## Contributing

1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am `Add some feature``
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request :D

Note: Don't forget your Unit Tests :).

## Credits

Kevin Neumann

## License

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.