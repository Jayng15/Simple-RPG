using System.Security.Cryptography;
using Engine.Models;

namespace Engine.Factories;

public class MonsterFactory {
    public static Monster GetMonster(int id) {
        switch(id) {
            case 1: 
                Monster snake = new Monster("Snake", "Snake.png", 4, 4, 5, 1);
                AddLootItem(snake, 9001, 25);
                AddLootItem(snake, 9002, 75);
                return snake;
            case 2:
                Monster rat = new Monster("Rat", "Rat.png", 5, 5, 5, 1);
                AddLootItem(rat, 9003, 25);
                AddLootItem(rat, 9004, 75);
                return rat;
            case 3:
                Monster giantSpider = new Monster(
                    "Giant Spider", "GiantSpider.png", 10, 10, 10, 3);
                AddLootItem(giantSpider, 9005, 25);
                AddLootItem(giantSpider, 9006, 75);
                return giantSpider;
            default:
                throw new ArgumentException($"{id} is an invalid monster id.");
        }
    }

    private static void AddLootItem(Monster monster, int itemID, int percentage) {
        if (RandomNumberGenerator.GetInt32(1, 101) <= percentage) {
            monster.Inventory?.Add(new ItemQuantity(itemID, 1));
        }
    } 
}