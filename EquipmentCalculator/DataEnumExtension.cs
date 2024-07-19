namespace EquipmentCalculator;

public static class DataEnumExtension
{
    public static bool IsJob(this ClassJobCategory cj)
    {
        switch (cj)
        {
            case ClassJobCategory.MRD:
            case ClassJobCategory.GLA:
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

    public static bool IsAccessory(this ItemUICategory category)
    {
        switch (category)
        {
            case ItemUICategory.Earrings:
            case ItemUICategory.Bracelets:
            case ItemUICategory.Necklace:
            case ItemUICategory.Ring:
            case ItemUICategory.Ring1:
                return true;
            default:
                return false;
        }
    }

    public static int NormalEquipmentExchangeTokenNum(this ItemUICategory itemUiCategory)
    {
        switch (itemUiCategory)
        {
            case ItemUICategory.Weapon:
                return 7;
            case ItemUICategory.Head:
                return 2;
            case ItemUICategory.Body:
                return 4;
            case ItemUICategory.Hands:
                return 2;
            case ItemUICategory.Legs:
                return 4;
            case ItemUICategory.Feet:
                return 2;
            case ItemUICategory.Earrings:
                return 1;
            case ItemUICategory.Necklace:
                return 1;
            case ItemUICategory.Bracelets:
                return 1;
            case ItemUICategory.Ring:
                return 1;
            case ItemUICategory.Ring1:
                return 1;
            default:
                throw new ArgumentOutOfRangeException(nameof(itemUiCategory), itemUiCategory, null);
        }

        return 0;
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