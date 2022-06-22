using Kalavarda.Primitives.Geometry;

namespace Relax.Models.Commands
{
    public class StartMoveCommand: CommandBase
    {
        public uint CharacterId { get; }

        public PointF StartPosition { get; }

        public MoveDirection Direction { get; }

        public StartMoveCommand(uint characterId, PointF startPosition, MoveDirection direction) : base(DateTimeOffset.Now)
        {
            CharacterId = characterId;
            StartPosition = startPosition;
            Direction = direction;
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
        public uint CharacterId { get; }

        public PointF StopPosition { get; }

        public StopMoveCommand(uint characterId, PointF stopPosition) : base(DateTimeOffset.Now)
        {
            CharacterId = characterId;
            StopPosition = stopPosition;
        }
    }

}
