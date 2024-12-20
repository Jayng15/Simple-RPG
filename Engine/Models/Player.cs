﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Engine.Models
{
    public class Player : LivingEntity
    {
        private string? _characterClass;
        private int _experiencePoints;
        public string? CharacterClass
        {
            get { return _characterClass; }
            set
            {
                _characterClass = value;
                OnPropertyChanged();
            }
        }
        public int ExperiencePoints
        {
            get { return _experiencePoints; }
            private set
            {
                _experiencePoints = value;
                OnPropertyChanged();
                SetLevelAndMaximumHitPoints();
            }
        }
        public ObservableCollection<QuestStatus>? Quests { get; }

        public Player()
        {
            Quests = new ObservableCollection<QuestStatus>();
            MaximumHitPoints = 10;
        }


        public bool HasAllTheseItems(List<ItemQuantity> items)
        {
            foreach (ItemQuantity item in items)
            {
                if (Inventory.Count(i => i.ItemTypeID == item.ItemID) < item.Quantity)
                {
                    return false;
                }
            }
            return true;
        }

        public event EventHandler? OnLeveledUp;

        public void AddExperience(int experiencePoints) {
            if (experiencePoints < 0) {
                throw new ArgumentException("Experience points must be positive");
            }
            ExperiencePoints += experiencePoints;
        }

        public void SetLevelAndMaximumHitPoints() {
            int originalLevel = Level;
            Level = (ExperiencePoints / 100) + 1;
            if (Level != originalLevel) {
                MaximumHitPoints = Level * 10;
                OnLeveledUp?.Invoke(this, System.EventArgs.Empty);
            }
        }

    }
}