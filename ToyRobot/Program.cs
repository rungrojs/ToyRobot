using ToyRobotProject.Constants;
using ToyRobotProject.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var map = new Map(5, 5);
        var toyRobot = new ToyRobot(map);
        toyRobot.StatusCallBack = (status) =>
        {
            Console.WriteLine(status);
        };

        string input;
        while ((input = Console.ReadLine() ?? string.Empty)?.ToUpper() != "QUIT")
        {
            ProcessCommand(toyRobot, input);
        }
        toyRobot.StatusCallBack = null;
    }

    private static void ProcessCommand(ToyRobot toyRobot, string? input)
    {
        if (!string.IsNullOrWhiteSpace(input))
        {
            var inputArray = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (Enum.TryParse(inputArray.First().ToUpper(), true, out Command command))
            {
                if (command == Command.PLACE)
                {
                    if (inputArray.Length == 2)
                    {
                        var location = PlaceLocation.Parse(inputArray.Last());
                        if (location != null)
                        {
                            toyRobot.Execute(command, location);
                        }
                    }
                }
                else
                {
                    toyRobot.Execute(command);
                }
            }
        }
    }
}