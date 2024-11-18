using ToyRobotProject.Constants;

namespace ToyRobotProject.Models
{
    public class PlaceLocation
    {
        public Direction Direction { get; set; } = Direction.EAST;
        public int PositionX { get; set; } = 0;
        public int PositionY { get; set; } = 0;

        static public PlaceLocation? Parse(string locationString)
        {
            var locationStringArray = locationString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (locationStringArray.Length == 3
                && int.TryParse(locationStringArray[0], out int positionX)
                && int.TryParse(locationStringArray[1], out int positionY)
                && Enum.TryParse(locationStringArray[2].ToUpper(), out Direction direction))
            {
                return new PlaceLocation
                {
                    Direction = direction,
                    PositionX = positionX,
                    PositionY = positionY
                };
            }
            return null;
        }
    }
}
