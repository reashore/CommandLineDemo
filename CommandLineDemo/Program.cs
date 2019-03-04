using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using static System.Console;

namespace CommandLineDemo
{
    // https://github.com/dotnet/command-line-api/wiki/Your-first-app-with-System.CommandLine
    public class Program
    {
        public static int Main(string[] args)
        {
            // dotnet run -- --int-option 123
            // dotnet run -- --bool-option false
            // dotnet run -- --file-option null
            int exitCode = CommandLine.ParseCommandLine(args);

            return exitCode;
        }
    }
    
    public static class CommandLine
    {
        public static int ParseCommandLine(string[] args)
        {
            string alias = "--int-option";
            string description = "An option whose argument is parsed as an int";
            Argument argument = new Argument<int>(defaultValue: 42);
            Option optionThatTakesInt = new Option(alias, description, argument);
            
            alias = "--bool-option";
            description = "An option whose argument is parsed as a bool";
            argument = new Argument<bool>();
            Option optionThatTakesBool = new Option(alias, description, argument);

            alias = "--file-option";
            description = "An option whose argument is parsed as a FileInfo";
            argument = new Argument<FileInfo>();
            Option optionThatTakesFileInfo = new Option(alias, description, argument);

            var rootCommand = new RootCommand {Description = "My sample app"};
            rootCommand.AddOption(optionThatTakesInt);
            rootCommand.AddOption(optionThatTakesBool);
            rootCommand.AddOption(optionThatTakesFileInfo);

            rootCommand.Handler = CommandHandler.Create<int, bool, FileInfo>((intOption, boolOption, fileOption) =>
            {
                WriteLine($"The value for --int-option is: {intOption}");
                WriteLine($"The value for --bool-option is: {boolOption}");
                WriteLine($"The value for --file-option is: {fileOption?.FullName ?? "null"}");
            });

            return rootCommand.InvokeAsync(args).Result;
        }
    }
}