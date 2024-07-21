namespace EquipmentCalculator;

public static class GCDTier
{
    public static Dictionary<int, int> GCDTierList(ClassJobCategory classCategory)
    {
        if(classCategory == ClassJobCategory.MNK)
        {
            return GCD2TierList;
        }

        return GCD25TierList;
    }
    
    private static Dictionary<int, int> GCD25TierList = new()
    {
        {250, 420}, 
        {249, 442}, 
        {248, 527}, 
        {247, 613}, 
        {246, 698}, 
        {245, 784}, 
        {244, 870}, 
        {243, 955}, 
        {242, 1041}, 
        {241, 1126}, 
        {240, 1212}, 
        {239, 1297}, 
        {238, 1383}, 
        {237, 1468}, 
        {236, 1554}, 
        {235, 1639}, 
        {234, 1725}, 
        {233, 1810}, 
        {232, 1896}, 
        {231, 1982}, 
        {230, 2067}, 
        {229, 2153}, 
        {228, 2238}, 
        {227, 2324}, 
        {226, 2409}, 
        {225, 2495}, 
        {224, 2580}, 
        {223, 2666}, 
        {222, 2751}, 
        {221, 2837}, 
        {220, 2922}, 
        {219, 3008}, 
        {218, 3094}, 
        {217, 3179}, 
        {216, 3265}, 
        {215, 3350}, 
        {214, 3436}, 
        {213, 3521}, 
        {212, 3607}, 
        {211, 3692}, 
        {210, 3778}, 
    };
    
    private static Dictionary<int, int> GCD2TierList = new()
    {
        {200, 420}, 
        {199, 442}, 
        {198, 549}, 
        {197, 656}, 
        {196, 763}, 
        {195, 870}, 
        {194, 976}, 
        {193, 1083}, 
        {192, 1190}, 
        {191, 1297}, 
        {190, 1404}, 
        {189, 1511}, 
        {188, 1618}, 
        {187, 1725}, 
    };
}