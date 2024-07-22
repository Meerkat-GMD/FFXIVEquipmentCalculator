namespace EquipmentCalculator;

public class Stat
{
    public int CRT;
    public int DET;
    public int DIR;
    public int TEN;
    public int PIE;
    public int SKS;
    public int SPS;
    public int WeaponDamage;
    public int MainStat;

    public Stat()
    {
            
    }
    public Stat(int crt, int det, int dir, int ten)
    {
        CRT = crt;
        DET = det;
        DIR = dir;
        TEN = ten;
    }
        

    public static Stat operator +(Stat s, EquipmentData d)
    {
        s.CRT += d.CRT;
        s.DET += d.DET;
        s.DIR += d.DIR;
        s.TEN += d.TEN;
        s.PIE += d.PIE;
        s.SKS += d.SKS;
        s.SPS += d.SPS;
        s.MainStat += d.MainStat;
        return s;
    }
        
    public static Stat operator -(Stat s, EquipmentData d)
    {
        s.CRT -= d.CRT;
        s.DET -= d.DET;
        s.DIR -= d.DIR;
        s.TEN -= d.TEN;
        s.PIE -= d.PIE;
        s.SKS -= d.SKS;
        s.SPS -= d.SPS;
        s.MainStat -= d.MainStat;
        return s;
    }
}

public static class StatCalculator
{
    public const int BaseCRT = 420;
    public const int BaseDIR = 420;
    public const int BaseDET = 440;
    public const int BaseSpeed = 420;
    public const int BaseTEN = 420;
    public static Stat CalculateBaseStat(EquipmentDataGroup data)
    {
        var baseStat = new Stat();
        foreach (var d in data.DataGroup)
        {
            baseStat += d;
        }

        return baseStat;
    }

    public static float CalculateExpectedDamage(this Stat stat, ClassJobCategory classJobCategory) => ExpectedDamage(stat, classJobCategory);

    public static float CalculateMainStat(ClassJobCategory classJobCategory, int mainStat) => classJobCategory.IsTank()
        ? Math.Max(0, (MathF.Floor(190 * (float)(mainStat - 440) / 440) + 100) / 100)
        : Math.Max(0, (MathF.Floor(237 * (float)(mainStat - 440) / 440) + 100) / 100);
    public static float CalculateCRTRate(int crtStat) => ((float)(200 * (crtStat - 420) / 2780) + 50) / 1000;
    public static float CalculateCRTMultiply(int crtStat) => ((float)(200 * (crtStat - 420) / 2780) + 1400) / 1000;
    public static float CalculateDIRRate(int dirStat) => (float)(550 * (dirStat - 420) / 2780) / 1000;
    public static float DirMultiPly => 1.25f;
    public static float CalculateDETMultiply(int detStat) => (1000 + (float)(140 * (detStat - 440) / 2780)) / 1000;
    public static float CalculateGCD(int ssStat) => (float)25 * (1000 + (130 * (420 - ssStat) / 2780)) / 10000;

    public static float CalculateTENMultiply(int tenStat) => (1000 + (float)(112 * (tenStat - 420) / 2780)) / 1000;
    public static float DeterminationExpectedDamage(int detStat) => CalculateDETMultiply(detStat);
    
    public static float CriticalExpectedDamage(int crtStat)
    {
        float crtRate = CalculateCRTRate(crtStat);
        float crtMul = CalculateCRTMultiply(crtStat);
        return (crtRate * crtMul + (1 - crtRate)) / 1.02f;
    }

    public static float DirectExpectedDamage(int dirStat)
    {
        float dirRate = CalculateDIRRate(dirStat);
        return dirRate * 1.25f + (1 - dirRate);
    }

    public static float ExpectedDamage(Stat stat, ClassJobCategory classJobCategory)
    {
        float weaponMul = stat.WeaponDamage; // 별도의 식이 잇지만 weapondamage 1당 1씩 늘어나길래 그냥 사용
        float mainStatMul = CalculateMainStat(classJobCategory, stat.MainStat);
        float crtRate = CalculateCRTRate(stat.CRT);
        float crtMul = CalculateCRTMultiply(stat.CRT);
        float dirRate = CalculateDIRRate(stat.DIR);
        float detandTenMul = CalculateDETMultiply(stat.DET) * CalculateTENMultiply(stat.TEN);

        return ((detandTenMul * crtMul * DirMultiPly) * (crtRate * dirRate) +
               (detandTenMul * crtMul) * (crtRate * (1 - dirRate)) +
               (detandTenMul * DirMultiPly) * ((1 - crtRate) * dirRate) + 
               detandTenMul * ((1 - crtRate) * (1 - dirRate))) * mainStatMul * weaponMul;
    }
}