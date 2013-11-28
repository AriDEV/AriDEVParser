namespace AriDEVParser.SQLOutput
{
    public sealed class VendorItemStore
    {
        public string GetCommand(uint entry, int itemId, int maxCount, int extendedCost)
        {
            var builder = new CommandBuilder("npc_vendor");

            builder.AddColumnValue("entry", entry);
            builder.AddColumnValue("item", itemId);
            builder.AddColumnValue("maxcount", maxCount);
            builder.AddColumnValue("incrtime", 0);
            builder.AddColumnValue("ExtendedCost", extendedCost);

            return builder.BuildInsert(true);
        }
    }
}
