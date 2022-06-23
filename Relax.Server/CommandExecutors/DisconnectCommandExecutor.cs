using System.Net;
using Relax.Models.Commands;

namespace Relax.Server.CommandExecutors
{
    internal class DisconnectCommandExecutor : ICommandExecutor
    {
        private readonly DisconnectCommand _command;
        private readonly IPEndPoint _clientEndPoint;
        private readonly CharactersRegistry _charactersRegistry;
        private readonly IClientSender _clientSender;

        public DisconnectCommandExecutor(DisconnectCommand command, IPEndPoint clientEndPoint, CharactersRegistry charactersRegistry, IClientSender clientSender)
        {
            _command = command ?? throw new ArgumentNullException(nameof(command));
            _clientEndPoint = clientEndPoint ?? throw new ArgumentNullException(nameof(clientEndPoint));
            _charactersRegistry = charactersRegistry ?? throw new ArgumentNullException(nameof(charactersRegistry));
            _clientSender = clientSender ?? throw new ArgumentNullException(nameof(clientSender));
        }

        public void Execute()
        {
            var characterId = _charactersRegistry.GetCharacterId(_clientEndPoint);
            if (characterId == null)
                return;

            _charactersRegistry.Disconnect(_command, _clientEndPoint);

            var command = new CharacterOfflineEvent(characterId.Value);
            _clientSender.SendToClient(command, _charactersRegistry.Registry.Select(re => re.EndPoint));
        }
    }
}
