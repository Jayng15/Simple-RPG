using System.Collections.ObjectModel;
using System.Windows.Markup;

namespace Engine.Models;

public class Monster : LivingEntity
{
    public string? ImageName { get; }
    public int MinimumDamage { get; } 
    public int MaximumDamage { get; }
    public int RewardExperiencePoints { get; }
    public Monster(
        string name, string imageName,
            int maximumHitPoints, int hitPoints,
                int minimumDamage, int maximumDamage,
                    int rewardExperiencePoints, int rewardGold)
    {
        Name = name;
        ImageName = $"pack://application:,,,/Engine;component/Images/Monsters/{imageName}";
        MaximumHitPoints = maximumHitPoints;
        CurrentHitPoints = hitPoints;
        MinimumDamage = minimumDamage;
        MaximumDamage = maximumDamage;
        RewardExperiencePoints = rewardExperiencePoints;
        Gold = rewardGold;
    }
}