using Kalavarda.Primitives.Geometry;
using NUnit.Framework;
using Relax.Models.Commands;

namespace Relax.Server.Tests.Models.Commands
{
    public class Serialization_Test
    {
        [Test]
        public void ConnectCommand_Test()
        {
            var command = new ConnectCommand(321, "savsdfvgsdf");
            var data = command.Serialize();
            var command2 = (ConnectCommand)CommandBase.Deserialize(data);
            Assert.AreEqual(command.CharacterId, command2.CharacterId);
            Assert.AreEqual(command.UserToken, command2.UserToken);
        }

        [Test]
        public void DisonnectCommand_Test()
        {
            var command = new DisconnectCommand();
            var data = command.Serialize();
            var command2 = (DisconnectCommand)CommandBase.Deserialize(data);
        }

        [Test]
        public void StartMoveCommand_Test()
        {
            var command = new StartMoveCommand(1234, new PointF(12, 34), MoveDirection.Up);
            var data = command.Serialize();
            var command2 = (StartMoveCommand)CommandBase.Deserialize(data);
            Assert.AreEqual(command.CharacterId, command2.CharacterId);
            Assert.AreEqual(command.StartPosition.X, command2.StartPosition.X);
            Assert.AreEqual(command.Direction, command2.Direction);
        }

        [Test]
        public void StopMoveCommand_Test()
        {
            var command = new StopMoveCommand(1234, new PointF(12, 34));
            var data = command.Serialize();
            var command2 = (StopMoveCommand)CommandBase.Deserialize(data);
            Assert.AreEqual(command.CharacterId, command2.CharacterId);
            Assert.AreEqual(command.StopPosition.X, command2.StopPosition.X);
        }
    }
}
