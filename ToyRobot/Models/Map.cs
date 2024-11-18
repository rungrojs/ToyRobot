namespace ToyRobotProject.Models
{
    public class Map
    {
        private readonly int _minPositionX;
        private readonly int _maxPositionX;
        private readonly int _minPositionY;
        private readonly int _maxPositionY;

        private Map() { }

        public Map(int width, int height)
        {
            _minPositionX = 0;
            _maxPositionX = width - 1;
            _minPositionY = 0;
            _maxPositionY = height - 1;
        }

        public bool IsValidLocation(int positionX, int positionY)
        {
            return positionX >= _minPositionX && positionX <= _maxPositionX
                && positionY >= _minPositionY && positionY <= _maxPositionY;
        }
    }
}