namespace Engine.Models;

public class World
{
    private List<Location> _locations = new List<Location>();

    internal void AddLocation(int XCoordinate, int YCoordinate,
            string Name, string Description, string ImageName)
    {
        Location loc = new Location
        {
            XCoordinate = XCoordinate,
            YCoordinate = YCoordinate,
            Name = Name,
            Description = Description,
            ImageName = ImageName
        };

        _locations.Add(loc);
    }

    public Location? LocationAt(int XCoordinate, int YCoordinate) =>
        _locations.FirstOrDefault(l => l.XCoordinate == XCoordinate 
            && l.YCoordinate == YCoordinate)?? null;
}
