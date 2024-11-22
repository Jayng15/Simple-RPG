using Engine.Models;

namespace Engine.Factories;

public static class ItemFactory
{
    private readonly static List<GameItem> _standardGameItems = new List<GameItem>();

    static ItemFactory()
    {
        BuildWeapon(1001, "Pointy Stick", 1, 1, 2);
        BuildWeapon(1002, "Rusty Sword", 5, 1, 3);
        BuildWeapon(1003, "OP Sword", 100, 50, 200);
        BuildMiscellaneousItem(9001, "Snake fang", 1);
        BuildMiscellaneousItem(9002, "Snakeskin", 2);
        BuildMiscellaneousItem(9003, "Rat tail", 1);
        BuildMiscellaneousItem(9004, "Rat fur", 2);
        BuildMiscellaneousItem(9005, "Spider fang", 1);
        BuildMiscellaneousItem(9006, "Spider silk", 2);
    }

    public static GameItem? CreateGameItem(int itemTypeID) =>
        _standardGameItems.FirstOrDefault(i => i.ItemTypeID == itemTypeID)?.Clone();

    private static void BuildMiscellaneousItem(int id, string name, int price)
    {
        _standardGameItems.Add(new GameItem(GameItem.ItemCategory.Miscellaneous, id, name, price));
    }

    private static void BuildWeapon(
        int id, string name, int price, int minimumDamage, int maximumDamage)
    {
        _standardGameItems.Add(new GameItem(
            GameItem.ItemCategory.Weapon, id, name, price, true, minimumDamage, maximumDamage));
    }

}