using System;

namespace AriDEVParser.Enums
{
    [Flags]
    public enum GroupUpdateFlag
    {
        None = 0x00000000,
        Status = 0x00000001,
        CurrentHealth = 0x00000002,
        MaxHealth = 0x00000004,
        PowerType = 0x00000008,
        CurrentPower = 0x00000010,
        MaxPower = 0x00000020,
        Level = 0x00000040,
        Zone = 0x00000080,
        Position = 0x00000100,
        Auras = 0x00000200,
        PetGuid = 0x00000400,
        PetName = 0x00000800,
        PetModelId = 0x00001000,
        PetCurrentHealth = 0x00002000,
        PetMaxHealth = 0x00004000,
        PetPowerType = 0x00008000,
        PetCurrentPower = 0x00010000,
        PetMaxPower = 0x00020000,
        PetAuras = 0x00040000,
        VehicleSeat = 0x00080000
    }
}
