using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using static System.Console;

namespace CommandLineDemo
{
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
            Option optionThatTakesInt = new Option(
                "--int-option",
                "An option whose argument is parsed as an int",
                new Argument<int>(defaultValue: 42));
            
            Option optionThatTakesBool = new Option(
                "--bool-option",
                "An option whose argument is parsed as a bool",
                new Argument<bool>());
            
            Option optionThatTakesFileInfo = new Option(
                "--file-option",
                "An option whose argument is parsed as a FileInfo",
                new Argument<FileInfo>());

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