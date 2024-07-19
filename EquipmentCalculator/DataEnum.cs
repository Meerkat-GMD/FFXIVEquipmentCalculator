namespace EquipmentCalculator;

public enum ItemUICategory
{
    Weapon = 0,
    Head = 1,
    Body = 2,
    Hands = 3,
    Legs = 4,
    Feet = 5,
    
    Earrings = 6,
    Necklace = 7,
    Bracelets = 8,
    Ring = 9,
    Ring1 = 10,
}
public enum ClassJobCategory
{
    Error = -1,
    #region Tanker
    
    MRD,
    WAR,
    GLA,
    PLD,
    DRK,
    GNB,
    
    #endregion
    
    #region Healer
    
    CNJ,
    WHM,
    ACN,
    SCH,
    AST,
    SGE,

    #endregion
    
    #region Melee
    
    PGL,
    MNK,
    LNC,
    DRG,
    ROG,
    NIN,
    SAM,
    RPR,
    VPR,

    #endregion
    
    #region Ranger
    
    ARC,
    BRD,
    MCH,
    DNC,

    #endregion
    
    #region Caster

    THM,
    BLM,
    SMN,
    RDM,
    BLU,
    PCT,

    #endregion
}

public enum StatCategory
{
    CRT,
    DET,
    DIR,
    TEN,
    PIE,
    SKS,
    SPS
}