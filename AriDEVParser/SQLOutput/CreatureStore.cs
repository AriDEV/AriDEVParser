using AriDEVParser.Enums;
using AriDEVParser.Util;

namespace AriDEVParser.SQLOutput
{
    public sealed class CreatureStore
    {
        public string GetCommand(int entry, string name, string subName, string iconName,
            CreatureTypeFlag typeFlags, CreatureType type, CreatureFamily family, CreatureRank rank,
            int[] killCredit, int[] dispId, float mod1, float mod2, bool racialLeader, int[] qItem,
            int moveId)
        {
            var builder = new CommandBuilder("creature_template");

            builder.AddColumnValue("entry", entry);

            for (var i = 0; i < 3; i++)
                builder.AddColumnValue("difficulty_entry_" + (i + 1), 0);

            for (var i = 0; i < 2; i++)
                builder.AddColumnValue("KillCredit" + (i + 1), killCredit[i]);

            if (SQLOutput.Format == SqlFormat.Trinity)
            {
                for (var i = 0; i < 4; i++)
                    builder.AddColumnValue("modelid" + (i + 1), dispId[i]);
            }
            else
            {
                builder.AddColumnValue("modelid_A", dispId[0]);
                builder.AddColumnValue("modelid_A2", dispId[1]);
                builder.AddColumnValue("modelid_H", dispId[2]);
                builder.AddColumnValue("modelid_H2", dispId[3]);
            }

            builder.AddColumnValue("name", name);
            builder.AddColumnValue("subname", subName);
            builder.AddColumnValue("IconName", iconName);
            builder.AddColumnValue("gossip_menu_id", 0);
            builder.AddColumnValue("minlevel", 1);
            builder.AddColumnValue("maxlevel", 1);

            if (SQLOutput.Format != SqlFormat.Trinity)
            {
                builder.AddColumnValue("minhealth", 0);
                builder.AddColumnValue("maxhealth", 0);
                builder.AddColumnValue("minmana", 0);
                builder.AddColumnValue("maxmana", 0);
                builder.AddColumnValue("armor", 0);
            }
            else
                builder.AddColumnValue("exp", 0);

            builder.AddColumnValue("faction_A", 35);
            builder.AddColumnValue("faction_H", 35);
            builder.AddColumnValue("npcflag", 0);
            builder.AddColumnValue("speed_walk", 1.0f);
            builder.AddColumnValue("speed_run", 1.14286f);
            builder.AddColumnValue("scale", 0.0f);
            builder.AddColumnValue("rank", (int)rank);
            builder.AddColumnValue("mindmg", 0);
            builder.AddColumnValue("maxdmg", 0);
            builder.AddColumnValue("dmgschool", 0);
            builder.AddColumnValue("attackpower", 0);
            builder.AddColumnValue("dmg_multiplier", 1.0f);
            builder.AddColumnValue("baseattacktime", 0);
            builder.AddColumnValue("rangeattacktime", 0);
            builder.AddColumnValue("unit_class", 1);
            builder.AddColumnValue("unit_flags", 0);
            builder.AddColumnValue("dynamicflags", 0);
            builder.AddColumnValue("family", (int)family);
            builder.AddColumnValue("trainer_type", 0);
            builder.AddColumnValue("trainer_spell", 0);
            builder.AddColumnValue("trainer_class", 0);
            builder.AddColumnValue("trainer_race", 0);
            builder.AddColumnValue("minrangedmg", 0.0f);
            builder.AddColumnValue("maxrangedmg", 0.0f);
            builder.AddColumnValue("rangedattackpower", 0);
            builder.AddColumnValue("type", (int)type);
            builder.AddColumnValue("type_flags", (int)typeFlags);
            builder.AddColumnValue("lootid", 0);
            builder.AddColumnValue("pickpocketloot", 0);
            builder.AddColumnValue("skinloot", 0);

            for (var i = 0; i < 6; i++)
                builder.AddColumnValue("resistance" + (i + 1), 0);

            var spellI = 4;
            if (SQLOutput.Format == SqlFormat.Trinity)
                spellI = 8;

            for (var i = 0; i < spellI; i++)
                builder.AddColumnValue("spell" + (i + 1), 0);

            builder.AddColumnValue("PetSpellDataId", 0);

            if (SQLOutput.Format == SqlFormat.Trinity)
                builder.AddColumnValue("VehicleId", 0);

            builder.AddColumnValue("mingold", 0);
            builder.AddColumnValue("maxgold", 0);
            builder.AddColumnValue("AIName", string.Empty);
            builder.AddColumnValue("MovementType", 0);
            builder.AddColumnValue("InhabitType", 0);

            if (SQLOutput.Format == SqlFormat.Trinity)
            {
                builder.AddColumnValue("Health_mod", mod1);
                builder.AddColumnValue("Mana_mod", mod2);
                builder.AddColumnValue("Armor_mod", 1);
            }
            else
            {
                builder.AddColumnValue("unk16", mod1);
                builder.AddColumnValue("unk17", mod2);
            }

            builder.AddColumnValue("RacialLeader", racialLeader.ToByte());

            for (var i = 0; i < 6; i++)
                builder.AddColumnValue("questItem" + (i + 1), qItem[0]);

            builder.AddColumnValue("movementId", moveId);
            builder.AddColumnValue("RegenHealth", 1);
            builder.AddColumnValue("equipment_id", 0);
            builder.AddColumnValue("mechanic_immune_mask", 0);
            builder.AddColumnValue("flags_extra", 0);
            builder.AddColumnValue("ScriptName", string.Empty);

            return builder.BuildInsert(true);
        }
    }
}
