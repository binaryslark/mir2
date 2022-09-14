using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

public sealed class Stats : IEquatable<Stats>
{
    public SortedDictionary<Stat, int> Values { get; set; } = new SortedDictionary<Stat, int>();
    public int Count => Values.Sum(pair => Math.Abs(pair.Value));

    public int this[Stat stat]
    {
        get
        {
            return !Values.TryGetValue(stat, out int result) ? 0 : result;
        }
        set
        {
            if (value == 0)
            {
                if (Values.ContainsKey(stat))
                {
                    Values.Remove(stat);
                }

                return;
            }

            Values[stat] = value;
        }
    }

    public Stats() { }

    public Stats(Stats stats)
    {
        foreach (KeyValuePair<Stat, int> pair in stats.Values)
            this[pair.Key] += pair.Value;
    }

    public Stats(BinaryReader reader, int version = int.MaxValue, int customVersion = int.MaxValue)
    {
        int count = reader.ReadInt32();

        for (int i = 0; i < count; i++)
            Values[(Stat)reader.ReadByte()] = reader.ReadInt32();
    }

    public void Add(Stats stats)
    {
        foreach (KeyValuePair<Stat, int> pair in stats.Values)
            this[pair.Key] += pair.Value;
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(Values.Count);

        foreach (KeyValuePair<Stat, int> pair in Values)
        {
            writer.Write((byte)pair.Key);
            writer.Write(pair.Value);
        }
    }

    public void Clear()
    {
        Values.Clear();
    }

    public bool Equals(Stats other)
    {
        if (Values.Count != other.Values.Count) return false;

        foreach (KeyValuePair<Stat, int> value in Values)
            if (other[value.Key] != value.Value) return false;

        return true;
    }
}

public enum StatFormula : byte
{
    Health,
    Mana,
    Weight,
    Stat
}

public enum Stat : byte
{
    MinAC                      = 0, // 最小防御力
    MaxAC                      = 1, // 最大防御力
    MinMAC                     = 2, // 最小魔法防御力
    MaxMAC                     = 3, // 最大魔法防御力
    MinDC                      = 4, // 最小攻击力
    MaxDC                      = 5, // 最大攻击力
    MinMC                      = 6, // 最小魔法攻击力
    MaxMC                      = 7, // 最大攻击力
    MinSC                      = 8, // 最小道术
    MaxSC                      = 9, // 最大道术

    Accuracy                   = 10, // 准确
    Agility                    = 11, // 敏捷
    HP                         = 12, // 血量
    MP                         = 13, // 蓝量
    AttackSpeed                = 14, // 攻速
    Luck                       = 15, // 幸运
    BagWeight                  = 16, // 背包负重
    HandWeight                 = 17, // 腕力
    WearWeight                 = 18, // 穿戴负重
    Reflect                    = 19,
    Strong                     = 20,
    Holy                       = 21,
    Freezing                   = 22,
    PoisonAttack               = 23,

    MagicResist                = 30,
    PoisonResist               = 31,
    HealthRecovery             = 32,
    SpellRecovery              = 33,
    PoisonRecovery             = 34, //TODO - Should this be in seconds or milliseconds??
    CriticalRate               = 35,
    CriticalDamage             = 36,

    MaxACRatePercent           = 40,
    MaxMACRatePercent          = 41,
    MaxDCRatePercent           = 42,
    MaxMCRatePercent           = 43,
    MaxSCRatePercent           = 44,
    AttackSpeedRatePercent     = 45,
    HPRatePercent              = 46,
    MPRatePercent              = 47,
    HPDrainRatePercent         = 48,

    ExpRatePercent             = 100,
    ItemDropRatePercent        = 101,
    GoldDropRatePercent        = 102,
    MineRatePercent            = 103,
    GemRatePercent             = 104,
    FishRatePercent            = 105,
    CraftRatePercent           = 106,
    SkillGainMultiplier        = 107,
    AttackBonus                = 108,

    LoverExpRatePercent        = 120,
    MentorDamageRatePercent    = 121,
    MentorExpRatePercent       = 123,
    DamageReductionPercent     = 124,
    EnergyShieldPercent        = 125,
    EnergyShieldHPGain         = 126,
    ManaPenaltyPercent         = 127,
    TeleportManaPenaltyPercent = 128,
    Hero                       = 129,

    Unknown                    = 255
}