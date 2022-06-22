using Kalavarda.Primitives.Geometry;

namespace Relax.Models.Commands
{
    public class StartMoveCommand: CommandBase
    {
        public uint CharacterId { get; private set; }

        public PointF StartPosition { get; private set; }

        public MoveDirection Direction { get; private set; }

        internal StartMoveCommand()
        {
        }

        public StartMoveCommand(uint characterId, PointF startPosition, MoveDirection direction) : this()
        {
            CharacterId = characterId;
            StartPosition = startPosition;
            Direction = direction;
        }

        protected override void Serialize(BinaryWriter writer)
        {
            writer.Write(CharacterId);
            StartPosition.Serialize(writer);
            writer.Write((byte)Direction);
        }

        protected override void Deserialize(BinaryReader reader)
        {
            CharacterId = reader.ReadUInt32();
            StartPosition = PointF.Deserialize(reader);
            Direction = (MoveDirection)reader.ReadByte();
        }
    }

    public enum MoveDirection
    {
        Up,
        Right,
        Down,
        Left
    }

    public class StopMoveCommand : CommandBase
    {
        public uint CharacterId { get; private set; }

        public PointF StopPosition { get; private set; }

        internal StopMoveCommand()
        {
        }

        public StopMoveCommand(uint characterId, PointF stopPosition) : this()
        {
            CharacterId = characterId;
            StopPosition = stopPosition;
        }

        protected override void Serialize(BinaryWriter writer)
        {
            writer.Write(CharacterId);
            StopPosition.Serialize(writer);
        }

        protected override void Deserialize(BinaryReader reader)
        {
            CharacterId = reader.ReadUInt32();
            StopPosition = PointF.Deserialize(reader);
        }
    }

}
