using ToyRobotProject.Constants;

namespace ToyRobotProject.Models
{
    public class ToyRobot
    {
        private readonly Map? _map;
        
        private int? _positionX { get; set; } = null;
        
        private int? _positionY { get; set; } = null;
        
        private Direction _direction { get; set; }

        public delegate void StatusCallback(string status);

        public StatusCallback? StatusCallBack { get; set; } = null;

        private ToyRobot() { }

        public ToyRobot(Map map)
        {
            _map = map;
        }

        public void Execute(Command command, PlaceLocation? locationModel = null)
        {
            switch (command)
            {
                case Command.PLACE:
                    ExecutePlace(command, locationModel);
                    break;
                case Command.LEFT:
                case Command.RIGHT:
                    ExecuteRotation(command);
                    break;
                case Command.MOVE:
                    ExecuteMove();
                    break;
                case Command.REPORT:
                    ExecuteReport();
                    break;
            }
        }

        #region private method

        private void ExecuteReport()
        {
            if (_map != null && _positionX.HasValue && _positionY.HasValue)
            {
                StatusCallBack?.Invoke($"Output: {_positionX},{_positionY},{_direction}");
            }
            else
            {
                StatusCallBack?.Invoke($"Output: Error, Please place robot on map to start");
            }
        }

        private void ExecuteMove()
        {
            if (_map != null && _positionX.HasValue && _positionY.HasValue)
            {
                var newPositionX = _positionX.Value + (_direction switch
                {
                    Direction.WEST => -1,
                    Direction.EAST => 1,
                    _ => 0
                });
                var newPositionY = _positionY.Value + (_direction switch
                {
                    Direction.NORTH => 1,
                    Direction.SOUTH => -1,
                    _ => 0
                });
                if (_map.IsValidLocation(newPositionX, newPositionY))
                {
                    _positionX = newPositionX;
                    _positionY = newPositionY;
                }
            }
        }

        private void ExecuteRotation(Command command)
        {
            var newDirection = (int)_direction
                + (int)(command switch
                {
                    Command.LEFT => -1,
                    Command.RIGHT => 1,
                    _ => 0
                });
            newDirection = (newDirection + 4) % 4;
            _direction = (Direction)newDirection;
        }

        private void ExecutePlace(Command command, PlaceLocation? locationModel)
        {
            if (locationModel != null && _map != null)
            {
                if (_map.IsValidLocation(locationModel.PositionX, locationModel.PositionY))
                {
                    _positionX = locationModel.PositionX;
                    _positionY = locationModel.PositionY;
                    _direction = locationModel.Direction;
                }
            }
        }

        #endregion
    }
}
