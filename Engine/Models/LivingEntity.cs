using System.Collections.ObjectModel;

namespace Engine.Models;

public abstract class LivingEntity : BaseNotificationClass
{
    private string _name;
    private int _maximumHitPoints;
    private int _currentHitPoints;
    private int _gold;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }
    public int Gold
    {
        get => _gold;
        set
        {
            _gold = value;
            OnPropertyChanged(nameof(Gold));
        }
    }
    public int MaximumHitPoints
    {
        get => _maximumHitPoints;
        set
        {
            _maximumHitPoints = value;
            OnPropertyChanged(nameof(MaximumHitPoints));
        }
    }
    public int CurrentHitPoints
    {
        get => _currentHitPoints;
        set
        {
            _currentHitPoints = value;
            OnPropertyChanged(nameof(CurrentHitPoints));
        }
    }

    public ObservableCollection<GameItem> Inventory { get; set; }
    public List<GameItem> Weapons => Inventory?.Where(i => i is Weapon).ToList();

    public ObservableCollection<GroupedInventoryItem> GroupedInventory { get; set; }

    protected LivingEntity()
    {
        Inventory = new ObservableCollection<GameItem>();
        GroupedInventory = new ObservableCollection<GroupedInventoryItem>();
    }
    public void AddItemToInventory(GameItem? item)
    {
        if (item == null)
        {
            return;
        }
        Inventory.Add(item);
        if (item.IsUnique)
        {
            GroupedInventory.Add(new GroupedInventoryItem(item, 1));
        }
        else
        {
            if (GroupedInventory.All(gi => gi.Item.ItemTypeID != item.ItemTypeID))
            {
                GroupedInventory.Add(new GroupedInventoryItem(item, 0));
            }
            GroupedInventory.First(gi => gi.Item.ItemTypeID == item.ItemTypeID).Quantity++;
        }
        OnPropertyChanged(nameof(Weapons));
    }

    public void RemoveItemFromInventory(GameItem? item)
    {
        if (item == null)
        {
            return;
       }

        Inventory.Remove(item);
        GroupedInventoryItem groupedInventoryItem =
            GroupedInventory.FirstOrDefault(
                gi => gi.Item.ItemTypeID == item.ItemTypeID);
        if (groupedInventoryItem != null)
        {
            if (groupedInventoryItem.Quantity == 1)
            {
                GroupedInventory.Remove(groupedInventoryItem);
            }
            else
            {
                groupedInventoryItem.Quantity--;
            }
        }
        OnPropertyChanged(nameof(Weapons));
    }

}