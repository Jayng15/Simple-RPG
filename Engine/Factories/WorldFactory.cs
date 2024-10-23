using Engine.Models;

namespace Engine.Factories;

internal static class WorldFactory
{
    internal static World CreateWorld()
    {
        World newWorld = new World();

        newWorld.AddLocation(
                0, -1, "Home", "This is your home.",
                    "pack://application:,,,/Engine;component/Images/Locations/Home.png");
        newWorld.AddLocation(
                -1, -1, "Farmer's House", "This is the house of the local farmer.",
                    "pack://application:,,,/Engine;component/Images/Locations/Farmhouse.png");
        newWorld.AddLocation(
                -2, -1, "Farmer's Field",
                    "There are rows of corn growing here, with giant rats hiding between them.",
                        "pack://application:,,,/Engine;component/Images/Locations/FarmFields.png");
        newWorld.AddLocation(
                0, 0, "TownSquare", "This is the town square.",
                    "pack://application:,,,/Engine;component/Images/Locations/TownSquare.png");
        newWorld.AddLocation(
                0, 1, "Herbalist's Hut", "The Local Herbalist's hut.",
                    "pack://application:,,,/Engine;component/Images/Locations/HerbalistsHut.png");
        newWorld.AddLocation(
                0, 2, "Herb Garden", "There are medicinal herbs growing here.",
                    "pack://application:,,,/Engine;component/Images/Locations/HerbalistsGarden.png");
        newWorld.AddLocation(
                -1, 0, "Trading Shop", "The Local Trader's Shop.",
                    "pack://application:,,,/Engine;component/Images/Locations/Trader.png");
        newWorld.AddLocation(
                1, 0, "Town Gate", "The gate that leads to the Spider Forest.",
                    "pack://application:,,,/Engine;component/Images/Locations/Trader.png");
        newWorld.AddLocation(
                    2, 0, "Spider Forest", "There are giant venomous spiders here.",
                    "pack://application:,,,/Engine;component/Images/Locations/Trader.png");

        return newWorld;
    }
}