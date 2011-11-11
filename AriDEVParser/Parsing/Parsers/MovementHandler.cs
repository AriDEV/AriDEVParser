using System;
using AriDEVParser.Util;
using AriDEVParser.Enums;
using Guid = AriDEVParser.Util.Guid;

namespace AriDEVParser.Parsing.Parsers
{
    public static class MovementHandler
    {
        public static Vector4 CurrentPosition;

        public static int CurrentMapId;

        public static int CurrentPhaseMask = 1;

        public static MovementInfo ReadMovementInfo(Packet packet, Guid guid)
        {
            bool HaveTransportData = false,
                 HaveTransportTime2 = false,
                 HaveTransportTime3 = false,
                 HavePitch = false,
                 HaveFallData = false,
                 HaveFallDirection = false,
                 HaveSplineElevation = false,
                 UnknownBool = false;
            var info = new MovementInfo();

            info.Flags = packet.ReadEnum<MoveFlag>("Movement Flags", 30);

            var flags2 = packet.ReadEnum<MoveFlagExtra>("Extra Movement Flags", 12);

            if (HaveTransportData = packet.ReadBit())
            {
                HaveTransportTime2 = packet.ReadBit();
                HaveTransportTime3 = packet.ReadBit();
            }

            HavePitch = packet.ReadBit();

            if (HaveFallData = packet.ReadBit())
                HaveFallDirection = packet.ReadBit();

            HaveSplineElevation = packet.ReadBit();
            UnknownBool = packet.ReadBit();

            packet.ReadGuid("GUID");
            packet.ReadInt32("Time");
            var pos = packet.ReadVector4("Position");
            info.Position = new Vector3(pos.X, pos.Y, pos.Z);
            info.Orientation = pos.O;

            if (HaveTransportData)//info.Flags.HasFlag(MoveFlag.OnTransport))
            {
                packet.ReadGuid("Transport GUID");
                packet.ReadVector4("Transport Position");
                packet.ReadByte("Transport Seat");
                packet.ReadInt32("Transport Time");

                if (HaveTransportTime2)//flags2.HasFlag(MoveFlagExtra.InterpolatedPlayerMovement))
                    packet.ReadInt32("Transport Time 2");

                if (HaveTransportTime3)
                    packet.ReadInt32("Transport Time 3");
            }

            if (HavePitch)//info.Flags.HasAnyFlag(MoveFlag.Swimming | MoveFlag.Flying) ||
                //flags2.HasFlag(MoveFlagExtra.AlwaysAllowPitching))
                packet.ReadSingle("Swim Pitch");

            if (HaveFallData) // info.Flags.HasFlag(MoveFlag.Falling)
            {
                packet.ReadInt32("Fall Time");
                packet.ReadSingle("Jump Velocity");
                if (HaveFallDirection)
                {
                    packet.ReadSingle("Jump Sin");
                    packet.ReadSingle("Jump Cos");
                    packet.ReadSingle("Jump XY Speed");
                }
            }

            if (HaveSplineElevation) //info.Flags.HasFlag(MoveFlag.SplineElevation))
                packet.ReadSingle("Spline Elevation");

            return info;
        }

        [Parser(Opcode.MSG_MOVE_HEARTBEAT)]
        [Parser(Opcode.MSG_MOVE_SET_PITCH)]
        [Parser(Opcode.MSG_MOVE_SET_FACING)]
        public static void ParseMovementPackets(Packet packet)
        {
            var guid = packet.ReadPackedGuid("Guid");
            ReadMovementInfo(packet, guid);
        }

        [Parser(Opcode.SMSG_MONSTER_MOVE)]
        [Parser(Opcode.SMSG_MONSTER_MOVE_TRANSPORT)]
        public static void ParseMonsterMovePackets(Packet packet)
        {
            var guid = packet.ReadPackedGuid("GUID");

            if (packet.GetOpcode() == (ushort)Opcode.SMSG_MONSTER_MOVE_TRANSPORT)
            {
                packet.ReadPackedGuid("Transport GUID");
                packet.ReadByte("Transport Seat");
            }

            packet.ReadByte("Unk Byte");//.ReadBoolean("Unk Boolean");

            var pos = packet.ReadVector3("Position");
            packet.ReadInt32("Move Ticks");

            var type = packet.ReadEnum<SplineType>("Spline Type");

            switch (type)
            {
                case SplineType.FacingSpot:
                    {
                        packet.ReadVector3("Facing Spot");
                        break;
                    }
                case SplineType.FacingTarget:
                    {
                        packet.ReadGuid("Facing GUID");
                        break;
                    }
                case SplineType.FacingAngle:
                    {
                        packet.ReadSingle("Facing Angle");
                        break;
                    }
                case SplineType.Stop:
                    {
                        return;
                    }
            }

            var flags = packet.ReadEnum<SplineFlag>("Spline Flags");

            if (flags.HasFlag(SplineFlag.AnimationTier))
            {
                packet.ReadEnum<MoveAnimationState>("Animation State");
                packet.ReadInt32("Unk Int32 1");
            }

            packet.ReadInt32("Move Time");

            if (flags.HasFlag(SplineFlag.Trajectory)) {
                packet.ReadSingle("Unk Single");
                packet.ReadInt32("Unk Int32 2");
            }

            var waypoints = packet.ReadInt32("Waypoints");

            var newpos = packet.ReadVector3("Waypoint 0");

            if (flags.HasFlag(SplineFlag.Flying) || flags.HasFlag(SplineFlag.CatmullRom)) {
                for (var i = 0; i < waypoints - 1; i++) {
                    packet.ReadVector3("Waypoint " + (i + 1));
                }
            }
            else {
                var mid = new Vector3();
                mid.X = (pos.X + newpos.X) * 0.5f;
                mid.Y = (pos.Y + newpos.Y) * 0.5f;
                mid.Z = (pos.Z + newpos.Z) * 0.5f;

                for (var i = 0; i < waypoints - 1; i++) {
                    var vec = packet.ReadPackedVector3();
                    vec.X += mid.X;
                    vec.Y += mid.Y;
                    vec.Z += mid.Z;

                    Console.WriteLine("Waypoint " + (i + 1) + ": " + vec);
                }
            }
        }
    }
}
