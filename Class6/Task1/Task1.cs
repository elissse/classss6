using Microsoft.Extensions.Logging;
namespace Task1;
class UnsupportedOperation : Exception
{
    public UnsupportedOperation(char op) : base($"Operation '{op}' is not supported")
    {
        Console.WriteLine($"Operation '{op}' is not supported");
        Task1.Logger.LogInformation("Stopped program due to UnsupportedOperation error");
    }

}
class DivisionByZero : Exception
{
    public DivisionByZero() : base($"Division by zero is caught")
    {
        Console.WriteLine($"Division by zero is caught");
        Task1.Logger.LogInformation("Stopped program due to DivisionByZero error");
    }
}

class NotEnoughNumbers : Exception
{
    public NotEnoughNumbers(int available, string line) : base($"More than {available} numbers are required in ${line}")
    {
        Console.WriteLine($"More than {available} numbers are required in ${line}");
        Task1.Logger.LogInformation("Stopped program due to NotEnoughNumbers error");
    }
}

 class NotEnoughOperations : Exception
{
    public NotEnoughOperations(int available, string line) : base(
        $"More than {available} operations are required in ${line}")
    {
        Console.WriteLine($"More than {available} operations are required in ${line}");
        Task1.Logger.LogInformation("Stopped program due to NotEnoughOperations error");
    }
}

 class WrongArgsFormat : Exception
{
    public WrongArgsFormat(string args) : base($"Program args are incorrect: {args}")
    {
        Console.WriteLine($"Program args are incorrect: {args}");
        Task1.Logger.LogInformation("Stopped program due to WrongArgsFormat error");
    }
}


 class WrongNumbersInput : Exception
{
    public WrongNumbersInput(string numbers) : base($"Failed to parse [{numbers}] to ints")
    {
        Console.WriteLine($"Failed to parse [{numbers}] to ints");
        Task1.Logger.LogInformation("Stopped program due to WrongNumbersInput error");
    }
}

 class FileDoesNotExist : Exception
{
    public FileDoesNotExist(string filePath) : base($"No such file: {filePath}")
    {
        Console.WriteLine($"No such file: {filePath}");
        Task1.Logger.LogInformation("Stopped program due to FileDoesNotExist error");
    }
}

 class FilesOfDifferentSize : Exception
{
    public FilesOfDifferentSize() : base("Input files contain different amount of lines")
    {
        Console.WriteLine("Input files contain different amount of lines");
        Task1.Logger.LogInformation("Stopped program due to FilesOfDifferentSize error");
    }
}

 class FileAlreadyExists : Exception
{
    public FileAlreadyExists(string filePath) : base($"File {filePath} already exists")
    {
        Console.WriteLine($"File {filePath} already exists");
        Task1.Logger.LogInformation("Stopped program due to FileAlreadyExists error");
    }
}

internal class FailedToCreateFile : Exception
{
    public FailedToCreateFile(string filePath) : base($"Failed to create file: {filePath}")
    {
        Console.WriteLine($"Failed to create file: {filePath}");
        Task1.Logger.LogInformation("Stopped program due to FailedToCreateFile error");
    }
}
public class Task1
{
    public static readonly ILogger<Task1> Logger =
        LoggerFactory.Create(builder => { builder.AddSimpleConsole(); }).CreateLogger<Task1>();

    internal static int ApplyOperation(char op, int arg1, int arg2)
    {
        switch (op)
        {
            case '*': return arg1 * arg2;
            case '/':
                {
                    if (arg2 == 0)
                        throw new DivisionByZero();
                    return arg1 / arg2;
                }
            default: throw new UnsupportedOperation(op);
        }
    }

    private static Func<List<int>, int> ApplySchema(string schema)
    {
        var ops = schema.ToCharArray();
        return args =>
        {
            var result = args[0];
            var i = 1;

            foreach (var op in ops)
                result = ApplyOperation(op, result, args[i++]);

            return result;
        };
    }

    internal static string FormatLhs(string schema, string[] numbers)
    {
        var outputString = $"{numbers[0]}";

        for (var i = 0; i < schema.Length; i++)
            outputString += $"{schema[i]}{numbers[i + 1]}";

        return outputString;
    }

    internal static string ProcessString(string schema, string input)
    {
        var magicalTransformation = ApplySchema(schema);
        var numbers = input.Split(",");

        if (numbers.Length < schema.Length + 1)
            throw new NotEnoughNumbers(numbers.Length, input);

        if (numbers.Length > schema.Length + 1)
            throw new NotEnoughOperations(schema.Length, schema);

        try
        {
            var n = numbers.Select(int.Parse).Count();
            if (n != numbers.Length)
            {
                throw new WrongNumbersInput(string.Join(" ", numbers));
            }
        }
        catch
        {
            throw new WrongNumbersInput(string.Join(" ", numbers));
        }

        var result = magicalTransformation(numbers.Select(int.Parse).ToList());
        return FormatLhs(schema, numbers) + $"={result}";
    }

    private static void ProcessFiles(string schemasFile, string dataFile, string outputFile)
    {
        if (!File.Exists(@schemasFile))
            throw new FileDoesNotExist(schemasFile);

        if (!File.Exists(@dataFile))
            throw new FileDoesNotExist(dataFile);

        var schemas = File.ReadAllLines(@schemasFile);
        var dataLines = File.ReadAllLines(@dataFile);

        if (schemas.Length != dataLines.Length)
            throw new FilesOfDifferentSize();

        if (File.Exists(@outputFile))
            throw new FileAlreadyExists(outputFile);

        var output = new List<string>();

        for (var i = 0; i < schemas.Length; i++)
            output.Add(ProcessString(schemas[i], dataLines[i]));

        try
        {
            var streamWriter = new StreamWriter(@outputFile);
            foreach (var line in output)
                streamWriter.WriteLine(line);
            streamWriter.Close();
        }
        catch (Exception)
        {
            throw new FailedToCreateFile(outputFile);
        }
    }

    private static List<string> ParseInput(string[] args)
    {
        if (args.Length != 3)
            throw new WrongArgsFormat(string.Join(" ", args));

        return new List<string> { args[0], args[1], args[2] };
    }
    public static void Main(string[] args)
    {
        Logger.LogInformation("program started");
        var parsedInput = ParseInput(args);
        Logger.LogInformation("program completed");
    }

}