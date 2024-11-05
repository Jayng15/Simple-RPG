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

    protected LivingEntity()
    {
        Inventory = new ObservableCollection<GameItem>();
    }
    public void AddItemToInventory(GameItem? item)
    {
        if (item == null)
        {
            return;
        }
        Inventory.Add(item);
        OnPropertyChanged(nameof(Weapons));
    }

    public void RemoveItemFromInventory(GameItem? item)
    {
        if (item == null)
        {
            return;
        }

        Inventory.Remove(item);
        OnPropertyChanged(nameof(Weapons));
    }

}