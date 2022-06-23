namespace Relax.Models.Commands
{
    public abstract class CommandBase
    {
        public byte[] Serialize()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);
            writer.Write(GetType().Name);
            Serialize(writer);
            writer.Flush();
            return stream.ToArray();
        }

        protected abstract void Serialize(BinaryWriter writer);

        public static CommandBase Deserialize(byte[] data)
        {
            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);
            var typeName = reader.ReadString();
            var command = CreateInstance(typeName);
            command.Deserialize(reader);
            return command;
        }

        private static CommandBase CreateInstance(string typeName)
        {
            switch (typeName) // дабы не использовать медленный reflection
            {
                case nameof(ConnectCommand):
                    return new ConnectCommand();

                case nameof(DisconnectCommand):
                    return new DisconnectCommand();

                case nameof(StartMoveCommand):
                    return new StartMoveCommand();

                case nameof(StopMoveCommand):
                    return new StopMoveCommand();

                case nameof(CharacterOnlineEvent):
                    return new CharacterOnlineEvent();

                case nameof(CharacterOfflineEvent):
                    return new CharacterOfflineEvent();

                default:
                    throw new NotImplementedException(typeName);
            }
        }

        protected abstract void Deserialize(BinaryReader reader);
    }
}
