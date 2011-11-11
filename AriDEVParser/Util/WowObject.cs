using System.Collections.Generic;
using AriDEVParser.Enums;

namespace AriDEVParser.Util
{
    public sealed class WowObject
    {
        public Guid Guid;

        public Vector3 Position;

        public ObjectType Type;

        public MovementInfo Movement;

        public WowObject(Guid guid, ObjectType type, MovementInfo moves)
        {
            Guid = guid;
            Type = type;
            Movement = moves;
        }
    }
}
