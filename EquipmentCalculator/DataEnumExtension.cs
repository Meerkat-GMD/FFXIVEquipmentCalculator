namespace EquipmentCalculator;

public static class DataEnumExtension
{
    public static bool IsJob(this ClassJobCategory cj)
    {
        switch (cj)
        {
            case ClassJobCategory.MRD:
            case ClassJobCategory.GLD:
            case ClassJobCategory.CNJ:
            case ClassJobCategory.ACN:
            case ClassJobCategory.PGL:
            case ClassJobCategory.LNC:
            case ClassJobCategory.ROG:
            case ClassJobCategory.ARC:
            case ClassJobCategory.THM:
                return true;
            default:
                return false;
        }
    }

    public static bool IsClass(this ClassJobCategory cj)
    {
        return !IsJob(cj);
    }
    
    
    /*
    public static ClassJobCategory JobToClass(this ClassJobCategory cj)
    {
        switch (cj)
        {
            case ClassJobCategory.MRD:
            case ClassJobCategory.WAR:
                return ClassJobCategory.WAR;
            case ClassJobCategory.GLD:
            case ClassJobCategory.PLD:
                return ClassJobCategory.PLD;
            case ClassJobCategory.CNJ:
            case ClassJobCategory.WHM:
                return ClassJobCategory.WHM;
            case ClassJobCategory.ACN:
            case ClassJobCategory.SCH:
                break;
            case ClassJobCategory.PGL:
            case ClassJobCategory.MNK:
                return ClassJobCategory.MNK;
            case ClassJobCategory.LNC:
            case ClassJobCategory.DRG:
                return ClassJobCategory.DRG;
            case ClassJobCategory.ROG:
            case ClassJobCategory.NIN:
                return ClassJobCategory.NIN;
            case ClassJobCategory.ARC:
            case ClassJobCategory.BRD:
                return ClassJobCategory.BRD;
            case ClassJobCategory.THM:
            case ClassJobCategory.BLM:
                return ClassJobCategory.BLM;
            case ClassJobCategory.Error:
                throw new ArgumentOutOfRangeException(nameof(cj), cj, null);
            default:
                return cj;
                
        }
    }
    */
}