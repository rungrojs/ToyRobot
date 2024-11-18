using ToyRobotProject.Constants;
using ToyRobotProject.Models;

namespace ToyRobotProject.UnitTests
{
    public class ToyRobotTest
    {
        private const string _outputTag = "Output:";

        [Theory()]
        [InlineData(0, 0, Direction.NORTH, $"{_outputTag} 0,0,NORTH")]
        [InlineData(0, 0, Direction.SOUTH, $"{_outputTag} 0,0,SOUTH")]
        [InlineData(0, 0, Direction.EAST, $"{_outputTag} 0,0,EAST")]
        [InlineData(0, 0, Direction.WEST, $"{_outputTag} 0,0,WEST")]
        [InlineData(4, 4, Direction.NORTH, $"{_outputTag} 4,4,NORTH")]
        [InlineData(0, 4, Direction.NORTH, $"{_outputTag} 0,4,NORTH")]
        [InlineData(4, 0, Direction.NORTH, $"{_outputTag} 4,0,NORTH")]
        public void Execute_PlaceCommand_ShouldSetPositionAndDirection_WhenLocationIsValid(
            int positionX, int positionY, Direction direction, string expectedResult)
        {
            // Arrange
            var toyRobot = GetToyRobot();

            var location = new PlaceLocation
            {
                PositionX = positionX,
                PositionY = positionY,
                Direction = direction
            };

            // Act
            toyRobot.Execute(Command.PLACE, location);

            // Assert
            string? output = null;
            toyRobot.StatusCallBack = (status) => output = status;
            toyRobot.Execute(Command.REPORT);

            Assert.Equal(expectedResult, output);
        }

        [Theory()]
        [InlineData(-1, 0, Direction.NORTH, $"{_outputTag} Error, Please place robot on map to start")]
        [InlineData(-1, 0, Direction.SOUTH, $"{_outputTag} Error, Please place robot on map to start")]
        [InlineData(-1, 0, Direction.EAST, $"{_outputTag} Error, Please place robot on map to start")]
        [InlineData(-1, 0, Direction.WEST, $"{_outputTag} Error, Please place robot on map to start")]
        [InlineData(0, -1, Direction.NORTH, $"{_outputTag} Error, Please place robot on map to start")]
        [InlineData(0, 5, Direction.NORTH, $"{_outputTag} Error, Please place robot on map to start")]
        [InlineData(5, 0, Direction.NORTH, $"{_outputTag} Error, Please place robot on map to start")]
        public void Execute_PlaceCommand_ShouldSetPositionAndDirection_WhenLocationIsInvalid(
            int positionX, int positionY, Direction direction, string expectedResult)
        {
            // Arrange
            var toyRobot = GetToyRobot();

            var location = new PlaceLocation
            {
                PositionX = positionX,
                PositionY = positionY,
                Direction = direction
            };

            // Act
            toyRobot.Execute(Command.PLACE, location);

            // Assert
            string? output = null;
            toyRobot.StatusCallBack = (status) => output = status;
            toyRobot.Execute(Command.REPORT);

            Assert.Equal(expectedResult, output);
        }

        [Theory()]
        [MemberData(nameof(Execute_MoveCommand_ShouldUpdatePosition_WhenMoveIsValidData), MemberType = typeof(ToyRobotTest))]
        public void Execute_MoveCommand_ShouldUpdatePosition_WhenMoveIsValid(
            int positionX, int positionY, Direction direction, List<Command> commands, string expectedResult)
        {
            // Arrange
            var toyRobot = GetToyRobot();

            var location = new PlaceLocation
            {
                PositionX = positionX,
                PositionY = positionY,
                Direction = direction
            };


            // Act
            toyRobot.Execute(Command.PLACE, location);
            foreach (var command in commands)
            {
                toyRobot.Execute(command);
            }

            // Assert
            string? output = null;
            toyRobot.StatusCallBack = (status) => output = status;
            toyRobot.Execute(Command.REPORT);

            Assert.Equal(expectedResult, output);
        }

        [Theory()]
        [MemberData(nameof(Execute_MoveCommand_ShouldUpdatePosition_WhenMoveIsValidData), MemberType = typeof(ToyRobotTest))]
        public void Execute_MoveCommand_ShouldUpdatePosition_WhenPlaceIsValidOnceAndMoveIsValid(
            int positionX, int positionY, Direction direction, List<Command> commands, string expectedResult)
        {
            // Arrange
            var toyRobot = GetToyRobot();
            var invalidLocation = new PlaceLocation
            {
                PositionX = -1,
                PositionY = 0,
                Direction = Direction.NORTH
            };

            var location = new PlaceLocation
            {
                PositionX = positionX,
                PositionY = positionY,
                Direction = direction
            };

            // Act
            toyRobot.Execute(Command.PLACE, invalidLocation);
            toyRobot.Execute(Command.MOVE);
            toyRobot.Execute(Command.MOVE);

            toyRobot.Execute(Command.PLACE, location);
            foreach (var command in commands)
            {
                toyRobot.Execute(command);
            }

            // Assert
            string? output = null;
            toyRobot.StatusCallBack = (status) => output = status;
            toyRobot.Execute(Command.REPORT);

            Assert.Equal(expectedResult, output);
        }

        [Fact]
        public void Execute_RotationCommands_ShouldChangeDirectionCorrectly()
        {
            // Arrange
            var toyRobot = GetToyRobot();

            var location = new PlaceLocation
            {
                PositionX = 0,
                PositionY = 0,
                Direction = Direction.NORTH
            };

            toyRobot.Execute(Command.PLACE, location);

            // Act
            toyRobot.Execute(Command.LEFT);
            string? outputLeft = null;
            toyRobot.StatusCallBack = (status) => outputLeft = status;
            toyRobot.Execute(Command.REPORT);

            toyRobot.Execute(Command.RIGHT);
            string? outputRight = null;
            toyRobot.StatusCallBack = (status) => outputRight = status;
            toyRobot.Execute(Command.REPORT);

            // Assert
            Assert.Equal($"{_outputTag} 0,0,WEST", outputLeft);
            Assert.Equal($"{_outputTag} 0,0,NORTH", outputRight);
        }

        [Fact]
        public void Execute_ReportCommand_ShouldReturnError_WhenRobotNotPlaced()
        {
            // Arrange
            var toyRobot = GetToyRobot();

            // Act
            string? output = null;
            toyRobot.StatusCallBack = (status) => output = status;
            toyRobot.Execute(Command.REPORT);

            // Assert
            Assert.Equal($"{_outputTag} Error, Please place robot on map to start", output);
        }

        [Fact]
        public void Execute_ReportCommand_ShouldReturnError_WhenRobotIncorrectlyPlaced()
        {
            // Arrange
            var toyRobot = GetToyRobot();
            var location = new PlaceLocation
            {
                PositionX = -1,
                PositionY = 0,
                Direction = Direction.NORTH
            };

            // Act
            string? output = null;
            toyRobot.StatusCallBack = (status) => output = status;
            toyRobot.Execute(Command.REPORT);

            // Assert
            Assert.Equal($"{_outputTag} Error, Please place robot on map to start", output);
        }

        public static IEnumerable<object[]> Execute_MoveCommand_ShouldUpdatePosition_WhenMoveIsValidData()
        {
            yield return new object[] { 0, 0, Direction.NORTH,
                new List<Command>() { Command.MOVE },
                $"{_outputTag} 0,1,NORTH" };
            yield return new object[] { 0, 0, Direction.NORTH,
                new List<Command>() { Command.MOVE, Command.MOVE, Command.MOVE, Command.MOVE },
                $"{_outputTag} 0,4,NORTH" };
            yield return new object[] { 0, 0, Direction.NORTH,
                new List<Command>() { Command.MOVE, Command.MOVE, Command.MOVE, Command.MOVE, Command.MOVE },
                $"{_outputTag} 0,4,NORTH" };
            yield return new object[] { 0, 0, Direction.NORTH,
                new List<Command>() { Command.LEFT, Command.RIGHT, Command.MOVE, Command.MOVE, Command.MOVE, Command.RIGHT },
                $"{_outputTag} 0,3,EAST" };
            yield return new object[] { 0, 0, Direction.WEST,
                new List<Command>() { Command.MOVE, Command.MOVE, Command.RIGHT, Command.MOVE, Command.MOVE },
                $"{_outputTag} 0,2,NORTH" };
            yield return new object[] { 0, 0, Direction.EAST,
                new List<Command>() { Command.MOVE, Command.RIGHT, Command.MOVE, Command.RIGHT, Command.MOVE },
                $"{_outputTag} 0,0,WEST" };
        }

        private ToyRobot GetToyRobot()
        {
            var map = new Map(5, 5);
            return new ToyRobot(map);
        }
    }
}