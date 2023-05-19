namespace EquipmentCalculator;

public enum ItemUICategory
{
    Weapon,
    Head,
    Body,
    Hands,
    Legs,
    Feet,
    Earrings,
    Necklace,
    Bracelets,
    Ring,
}
public enum ClassJobCategory
{
    Error = -1,
    #region Tanker
    
    MRD,
    WAR,
    GLD,
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

    #endregion
}