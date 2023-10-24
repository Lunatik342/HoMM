using System;
using System.Collections.Generic;

namespace Battle.Units.StatsSystem
{
    //Based on https://forum.unity.com/threads/tutorial-character-stats-aka-attributes-system.504095/
    public class UnitStat
    {
        private readonly List<StatModifier> _statModifiers;

        public readonly int BaseValue;
        public int Value { get; private set; }
        public event Action<int, int> ValueChanged;

        public UnitStat(int baseValue)
        {
            BaseValue = baseValue;
            Value = baseValue;
            _statModifiers = new List<StatModifier>();
        }

        public void AddModifier(StatModifier mod)
        {
            _statModifiers.Add(mod);
            _statModifiers.Sort(CompareModifierOrder);
            SetDirty();
        }
 
        public bool RemoveModifier(StatModifier mod)
        {
            if (_statModifiers.Remove(mod))
            {
                SetDirty();
                return true;
            }
            return false;
        }

        public bool RemoveAllModifiersFromSource(object source)
        {
            bool didRemove = false;
 
            for (int i = _statModifiers.Count - 1; i >= 0; i--)
            {
                if (_statModifiers[i].Source == source)
                {
                    didRemove = true;
                    _statModifiers.RemoveAt(i);
                }
            }

            if (didRemove)
            {
                SetDirty();
            }
            
            return didRemove;
        }
 
        private int CalculateFinalValue()
        {
            float finalValue = BaseValue;
            float sumPercentAdd = 0;
 
            for (int i = 0; i < _statModifiers.Count; i++)
            {
                StatModifier mod = _statModifiers[i];
 
                if (mod.Type == StatModType.Flat)
                {
                    finalValue += mod.Value;
                }
                else if (mod.Type == StatModType.PercentAdd)
                {
                    sumPercentAdd += mod.Value;
 
                    if (i + 1 >= _statModifiers.Count || _statModifiers[i + 1].Type != StatModType.PercentAdd)
                    {
                        finalValue *= 1 + sumPercentAdd;
                        sumPercentAdd = 0;
                    }
                }
                else if (mod.Type == StatModType.PercentMult)
                {
                    finalValue *= 1 + mod.Value;
                }
            }
 
            return (int)Math.Round(finalValue);
        }

        private void SetDirty()
        {
            var previousValue = Value;
            Value = CalculateFinalValue();
            ValueChanged?.Invoke(previousValue, Value);
        }
 
        private int CompareModifierOrder(StatModifier a, StatModifier b)
        {
            if (a.Order < b.Order)
                return -1;
            else if (a.Order > b.Order)
                return 1;
            return 0;
        }
    }
}