namespace Relax.Models.Commands
{
    public class CharacterOnlineEvent: CommandBase
    {
        public uint CharacterId { get; private set; }

        internal CharacterOnlineEvent()
        {
        }

        public CharacterOnlineEvent(uint characterId): this()
        {
            CharacterId = characterId;
        }

        protected override void Serialize(BinaryWriter writer)
        {
            writer.Write(CharacterId);
        }

        protected override void Deserialize(BinaryReader reader)
        {
            CharacterId = reader.ReadUInt32();
        }
    }

    public class CharacterOfflineEvent : CommandBase
    {
        public uint CharacterId { get; private set; }

        internal CharacterOfflineEvent()
        {
        }

        public CharacterOfflineEvent(uint characterId) : this()
        {
            CharacterId = characterId;
        }

        protected override void Serialize(BinaryWriter writer)
        {
            writer.Write(CharacterId);
        }

        protected override void Deserialize(BinaryReader reader)
        {
            CharacterId = reader.ReadUInt32();
        }
    }

}
