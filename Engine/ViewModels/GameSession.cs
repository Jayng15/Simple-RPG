using System.ComponentModel;
using System.Dynamic;
using System.Security.Cryptography;
using System.Windows.Automation;
using Engine.EventArgs;
using Engine.Factories;
using Engine.Models;

namespace Engine.ViewModels
{
    public class GameSession : BaseNotificationClass
    {
        private Location _currentLocation;
        private Monster _currentMonster;
        private Player _currentPlayer;
        private Trader? _currentTrader;
        public Player CurrentPlayer {
            get => _currentPlayer;
            set {
                if(_currentPlayer != null) {
                    _currentPlayer.OnLeveledUp -= OnCurrentPlayerLeveledUp;
                    _currentPlayer.OnKilled -= OnCurrentPlayerKilled;
                }
                _currentPlayer = value;
                if (_currentPlayer != null) {
                    _currentPlayer.OnLeveledUp += OnCurrentPlayerLeveledUp;
                    _currentPlayer.OnKilled += OnCurrentPlayerKilled;
                }

            }
        }


        public EventHandler<GameMessageEventArgs>? OnMessageRaised;

        public Monster CurrentMonster
        {
            get { return _currentMonster; }
            set
            {
                _currentMonster = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasMonster));

                if (CurrentMonster != null)
                {
                    RaiseMessage("");
                    RaiseMessage($"You encounter a {CurrentMonster.Name}!");
                }
            }
        }
        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set
            {
                _currentLocation = value;

                CurrentTrader = CurrentLocation.TraderHere;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasLocationToNorth));
                OnPropertyChanged(nameof(HasLocationToWest));
                OnPropertyChanged(nameof(HasLocationToEast));
                OnPropertyChanged(nameof(HasLocationToSouth));

                GivePlayerQuestsAtLocation();
                CompleteQuestAtLocation();
                GetMonsterAtLocation();
            }
        }

        public Trader? CurrentTrader {
            get { return _currentTrader; }
            set {
                _currentTrader = value;
                
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasTrader));
            }
        }

        public World CurrentWorld { get; }

        public bool HasLocationToNorth =>
            CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1) != null;

        public bool HasLocationToWest =>
            CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate) != null;

        public bool HasLocationToEast =>
            CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate) != null;

        public bool HasLocationToSouth =>
            CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1) != null;

        public bool HasMonster => CurrentMonster != null;
        public bool HasTrader => CurrentTrader != null;
        public GameSession()
        {
            CurrentPlayer = new Player
            {
                Name = "Player",
                Gold = 10000,
                CharacterClass = "Fighter",
                CurrentHitPoints = 10,
            };
            CurrentPlayer.SetLevelAndMaximumHitPoints();

            if ( !CurrentPlayer.Weapons.Any() )
            {
                CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(1003));
            }

            CurrentWorld = WorldFactory.CreateWorld();

            CurrentLocation = CurrentWorld.LocationAt(0, 0);
        }

        public GameItem? CurrentWeapon { get; set; }

        public void MoveNorth()
        {
            if (HasLocationToNorth)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1);
            }
        }

        public void MoveWest()
        {
            if (HasLocationToWest)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate);
            }
        }

        public void MoveEast()
        {
            if (HasLocationToEast)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate);
            }
        }

        public void MoveSouth()
        {
            if (HasLocationToSouth)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate,
                                                            CurrentLocation.YCoordinate - 1);
            }
        }

        private void GivePlayerQuestsAtLocation()
        {
            foreach (Quest? quest in CurrentLocation.QuestsAvailableHere)
            {
                if (!CurrentPlayer.Quests.Any(pq => pq.PlayerQuest.ID == quest.ID))
                {
                    CurrentPlayer.Quests.Add(new QuestStatus(quest));
                    RaiseMessage("");
                    RaiseMessage($"You receive the '{quest.Name}' quest.");
                    RaiseMessage(quest.Description);
                    RaiseMessage("Return with:");
                    foreach (ItemQuantity itemQuantity in quest.ItemsToComplete)
                    {
                        RaiseMessage($"  {itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemID).Name}");
                    }
                    RaiseMessage("And you will receive:");
                    RaiseMessage($"  {quest.RewardExperiencePoints} experience points");
                    RaiseMessage($"  {quest.RewardGold} gold");
                    foreach (ItemQuantity itemQuantity in quest.RewardItems) {
                        RaiseMessage($"  {itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemID).Name}");
                    }
                    
                }
            }
        }

        private void CompleteQuestAtLocation() {
            foreach ( Quest quest in CurrentLocation.QuestsAvailableHere ) {
                QuestStatus? questToComplete = CurrentPlayer.Quests.FirstOrDefault(
                        p => p.PlayerQuest.ID == quest.ID && !p.IsCompleted); 

                if ( questToComplete != null ) {
                    if ( CurrentPlayer.HasAllTheseItems(quest.ItemsToComplete) ) {
                        // Remove the quest completion items from the player's inventory
                        foreach ( ItemQuantity itemQuantity in quest.ItemsToComplete ) {
                            for (int i = 0 ; i < itemQuantity.Quantity ; i++ ) {
                                CurrentPlayer.RemoveItemFromInventory(
                                    CurrentPlayer.Inventory.First(
                                        item => item.ItemTypeID == itemQuantity.ItemID));
                            }
                        }
                        RaiseMessage("");
                        RaiseMessage($"You completed the '{quest.Name}' quest.");
                        RaiseMessage("You receive:");
                        RaiseMessage($"  {quest.RewardExperiencePoints} experience points");
                        CurrentPlayer.AddExperience(quest.RewardExperiencePoints);
                        RaiseMessage($"  {quest.RewardGold} gold");
                        CurrentPlayer.Gold += quest.RewardGold;
                        
                        foreach ( ItemQuantity rewardItem in quest.RewardItems ) {
                            GameItem? item = ItemFactory.CreateGameItem(rewardItem.ItemID);
                            CurrentPlayer.AddItemToInventory(item);
                            RaiseMessage($"  You received: {rewardItem.Quantity} {item.Name}");
                        }

                        questToComplete.IsCompleted = true;
                        
                    }
                }
            }             

        }

        private void GetMonsterAtLocation()
        {
            CurrentMonster = CurrentLocation.GetMonster();
        }

        private void RaiseMessage(string message)
        {
            OnMessageRaised?.Invoke(this, new GameMessageEventArgs(message));
        }

        private void OnCurrentPlayerLeveledUp(object sender, System.EventArgs e) {
            RaiseMessage($"You are now level {CurrentPlayer.Level}!");
        }

        private void OnCurrentPlayerKilled(object sender, System.EventArgs e) {
            RaiseMessage("");
            RaiseMessage("You have been killed.");
            CurrentLocation = CurrentWorld.LocationAt(0, -1); // Player's home
            CurrentPlayer.CurrentHitPoints = CurrentPlayer.Level * 10;
        }

        public void AttackCurrentMonster()
        {
            if (CurrentWeapon == null)
            {
                RaiseMessage("You must select a weapon, to attack.");
                return;
            }

            //Determine to Damage Monster
            int damageToMonster = RandomNumberGenerator.GetInt32(
                CurrentWeapon.MinimumDamage, CurrentWeapon.MaximumDamage);

            if (damageToMonster == 0)
            {
                RaiseMessage($"You missed the attack.");
            }
            else
            {
                CurrentMonster.CurrentHitPoints -= damageToMonster;
                RaiseMessage($"You dealed {damageToMonster} damage to {CurrentMonster.Name}.");
            }

            //Check if monster is killed and give rewards
            if (CurrentMonster.CurrentHitPoints <= 0)
            {
                RaiseMessage("");
                RaiseMessage($"You defeated the {CurrentMonster.Name}.");

                // CurrentPlayer.ExperiencePoints += CurrentMonster.RewardExperiencePoints;
                CurrentPlayer.AddExperience(CurrentMonster.RewardExperiencePoints);
                RaiseMessage($"You receive {CurrentMonster.RewardExperiencePoints} experience points.");

                CurrentPlayer.Gold += CurrentMonster.Gold;
                RaiseMessage($"You receive {CurrentMonster.Gold} gold.");

                foreach (GameItem itemQuantity in CurrentMonster.Inventory)
                {
                    GameItem? item = ItemFactory.CreateGameItem(itemQuantity.ItemTypeID);
                    CurrentPlayer.AddItemToInventory(item);
                    RaiseMessage($"You receive one {item.Name}.");
                }

                //Get another monster
                GetMonsterAtLocation();
            }
            else
            {
                int damageToPlayer = RandomNumberGenerator.GetInt32(
                    CurrentMonster.MinimumDamage, CurrentMonster.MaximumDamage);

                if (damageToPlayer == 0)
                {
                    RaiseMessage("The monster missed the attack.");
                }
                else
                {
                    RaiseMessage($"The {CurrentMonster.Name} dealed {damageToPlayer} damage to you.");
                    CurrentPlayer.TakeDamage(damageToPlayer);
                }
            }
        }

    }
}