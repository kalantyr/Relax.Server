using System.Net;
using Relax.Models.Commands;

namespace Relax.Server.CommandExecutors
{
    internal class ConnectCommandExecutor: ICommandExecutor
    {
        private readonly ConnectCommand _command;
        private readonly IPEndPoint _clientEndPoint;
        private readonly CharactersRegistry _charactersRegistry;
        private readonly IClientSender _clientSender;

        public ConnectCommandExecutor(ConnectCommand command, IPEndPoint clientEndPoint, CharactersRegistry charactersRegistry, IClientSender clientSender)
        {
            _command = command ?? throw new ArgumentNullException(nameof(command));
            _clientEndPoint = clientEndPoint ?? throw new ArgumentNullException(nameof(clientEndPoint));
            _charactersRegistry = charactersRegistry ?? throw new ArgumentNullException(nameof(charactersRegistry));
            _clientSender = clientSender ?? throw new ArgumentNullException(nameof(clientSender));
        }

        public void Execute()
        {
            // TODO: в параллельный поток

            _charactersRegistry.ConnectAsync(_command, _clientEndPoint)
                .Wait();

            var command = new CharacterOnlineEvent(_command.CharacterId);
            _clientSender.SendToClient(command, _charactersRegistry.Registry.Select(re => re.EndPoint));
        }
    }
}
