using Engine.Models;

namespace Engine.Factories;

internal static class WorldFactory
{
    internal static World CreateWorld()
    {
        World newWorld = new World();

        newWorld.AddLocation(
                0, -1, "Home", "This is your home.",
                    "Home.png");
        newWorld.AddLocation(
                -1, -1, "Farmer's House", "This is the house of the local farmer.",
                    "FarmHouse.png");
        newWorld.AddLocation(
                -2, -1, "Farmer's Field",
                    "There are rows of corn growing here, with giant rats hiding between them.",
                        "FarmFields.png");
        newWorld.LocationAt(-2, -1)?.AddMonster(2, 100);
        newWorld.AddLocation(
                0, 0, "TownSquare", "This is the town square.",
                    "TownSquare.png");
        newWorld.AddLocation(
                0, 1, "Herbalist's Hut", "The Local Herbalist's hut.",
                    "HerbalistsHut.png");
        newWorld.LocationAt(0, 1)?.QuestsAvailableHere.Add(QuestFactory.GetQuestByID(1));
        newWorld.AddLocation(
                0, 2, "Herb Garden", "There are medicinal herbs growing here.",
                    "HerbalistsGarden.png");
        newWorld.LocationAt(0, 2)?.AddMonster(1, 100);
        newWorld.AddLocation(
                -1, 0, "Trading Shop", "The Local Trader's Shop.",
                    "Trader.png");
        newWorld.AddLocation(
                1, 0, "Town Gate", "The gate that leads to the Spider Forest.",
                    "TownGate.png");
        newWorld.AddLocation(
                    2, 0, "Spider Forest", "There are giant venomous spiders here.",
                    "SpiderForest.png");
        newWorld.LocationAt(2, 0)?.AddMonster(3, 100);

        return newWorld;
    }
}