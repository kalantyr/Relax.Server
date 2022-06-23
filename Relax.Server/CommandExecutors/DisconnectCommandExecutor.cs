using System.Net;
using Relax.Models.Commands;

namespace Relax.Server.CommandExecutors
{
    internal class DisconnectCommandExecutor : ICommandExecutor
    {
        private readonly DisconnectCommand _command;
        private readonly IPEndPoint _clientEndPoint;
        private readonly CharactersRegistry _charactersRegistry;

        public DisconnectCommandExecutor(DisconnectCommand command, IPEndPoint clientEndPoint, CharactersRegistry charactersRegistry)
        {
            _command = command ?? throw new ArgumentNullException(nameof(command));
            _clientEndPoint = clientEndPoint ?? throw new ArgumentNullException(nameof(clientEndPoint));
            _charactersRegistry = charactersRegistry ?? throw new ArgumentNullException(nameof(charactersRegistry));
        }

        public void Execute()
        {
            _charactersRegistry.Disconnect(_command, _clientEndPoint);
        }
    }
}
