namespace EquipmentCalculator;

public static class StatCalculator
{
    public const int BaseCRT = 400;
    public const int BaseDIR = 400;
    public const int BaseDET = 390;

    public class Stat
    {
        public int CRT;
        public int DET;
        public int DIR;
        public int TEN;
        public int PIE;
        public int SKS;
        public int SPS;
        public int MainStat;

        public Stat()
        {
            
        }
        public Stat(int crt, int det, int dir)
        {
            CRT = crt;
            DET = det;
            DIR = dir;
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


    public static Stat CalculateBaseStat(EquipmentDataGroup data)
    {
        var baseStat = new Stat();
        foreach (var d in data.DataGroup)
        {
            baseStat += d;
        }

        return baseStat;
    }

    public static float CalculateExpectedDamage(this Stat stat) => ExpectedDamage(stat.CRT, stat.DIR, stat.DET);
    
    public static float CalculateCRTRate(int crtStat) => (float)(200 * (crtStat - 400) / 1900 + 50) / 1000;
    public static float CalculateCRTMultiply(int crtStat) => (float)(200 * (crtStat - 400) / 1900 + 1400) / 1000;
    public static float CalculateDIRRate(int dirStat) => (float)(550 * (dirStat - 400) / 1900) / 1000;
    public static float DirMultiPly => 1.25f;
    public static float CalculateDETMultiply(int detStat) => (float)(140 * (detStat - 390) / 1900 + 1000) / 1000;

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

    public static float ExpectedDamage(int crtStat, int dirStat, int detStat)
    {
        float crtRate = CalculateCRTRate(crtStat);
        float crtMul = CalculateCRTMultiply(crtStat);
        float dirRate = CalculateDIRRate(dirStat);
        float detMul = CalculateDETMultiply(detStat);

        return (detMul * crtMul * DirMultiPly) * (crtRate * dirRate) +
               (detMul * crtMul) * (crtRate * (1 - dirRate)) +
               (detMul * DirMultiPly) * ((1 - crtRate) * dirRate) + detMul * ((1 - crtRate) * (1 - dirRate));
    }
}