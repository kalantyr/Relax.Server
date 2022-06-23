using System.Net;
using Relax.Models.Commands;

namespace Relax.Server.CommandExecutors
{
    internal class CommandExecutorFactory
    {
        private readonly CharactersRegistry _charactersRegistry;
        private readonly IClientSender _clientSender;

        public CommandExecutorFactory(CharactersRegistry charactersRegistry, IClientSender clientSender)
        {
            _charactersRegistry = charactersRegistry ?? throw new ArgumentNullException(nameof(charactersRegistry));
            _clientSender = clientSender ?? throw new ArgumentNullException(nameof(clientSender));
        }

        public ICommandExecutor Create(CommandBase command, IPEndPoint clientEndPoint)
        {
            if (command is ConnectCommand connectCommand)
                return new ConnectCommandExecutor(connectCommand, clientEndPoint, _charactersRegistry, _clientSender);

            if (command is DisconnectCommand disconnectCommand)
                return new DisconnectCommandExecutor(disconnectCommand, clientEndPoint, _charactersRegistry, _clientSender);

            if (command is StartMoveCommand startMoveCommand)
                throw new NotImplementedException(nameof(StartMoveCommand));

            if (command is StopMoveCommand stopMoveCommand)
                throw new NotImplementedException(nameof(StopMoveCommand));

            throw new NotImplementedException(command.GetType().FullName);
        }
    }
}
