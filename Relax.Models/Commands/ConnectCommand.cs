namespace Relax.Models.Commands
{
    public class ConnectCommand: CommandBase
    {
        public uint CharacterId { get; private set; }
        
        public string UserToken { get; private set; }

        internal ConnectCommand()
        {
        }

        public ConnectCommand(uint characterId, string userToken): this()
        {
            CharacterId = characterId;
            UserToken = userToken ?? throw new ArgumentNullException(nameof(userToken));
        }

        protected override void Serialize(BinaryWriter writer)
        {
            writer.Write(CharacterId);
            writer.Write(UserToken);
        }

        protected override void Deserialize(BinaryReader reader)
        {
            CharacterId = reader.ReadUInt32();
            UserToken = reader.ReadString();
        }
    }

    public class DisconnectCommand: CommandBase
    {
        protected override void Serialize(BinaryWriter writer)
        {
        }

        protected override void Deserialize(BinaryReader reader)
        {
        }
    }
}
