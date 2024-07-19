namespace EquipmentCalculator;

public enum ItemUICategory
{
    //구분자용
    LeftSideStart,
    //
    Weapon,
    Head,
    Body,
    Hands,
    Legs,
    Feet,
    
    //구분자용
    LeftSideEnd,
    RightSideStart,
    //
    
    Earrings,
    Necklace,
    Bracelets,
    Ring,
    Ring1,
    
    //구분자용
    RightSideEnd,
    //
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

    #endregion
}