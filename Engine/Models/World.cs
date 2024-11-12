namespace Engine.Models;

public class World
{
    private readonly List<Location> _locations = new List<Location>();

    internal void AddLocation(int xCoordinate, int yCoordinate,
            string name, string description, string imageName)
    {
        string ImageName = $"pack://application:,,,/Engine;component/Images/Locations/{imageName}";
        Location loc = new Location(xCoordinate, yCoordinate, name, description, ImageName);

        // {
        //     XCoordinate = xCoordinate,
        //     YCoordinate = yCoordinate,
        //     Name = name,
        //     Description = description,
        // };

        _locations.Add(loc);
    }

    public Location? LocationAt(int XCoordinate, int YCoordinate) =>
        _locations.FirstOrDefault(l => l.XCoordinate == XCoordinate 
            && l.YCoordinate == YCoordinate)?? null;
}
