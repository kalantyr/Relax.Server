using System.Net;
using Relax.Models.Commands;

namespace Relax.Server.CommandExecutors
{
    internal class CommandExecutorFactory
    {
        private readonly CharactersRegistry _charactersRegistry;

        public CommandExecutorFactory(CharactersRegistry charactersRegistry)
        {
            _charactersRegistry = charactersRegistry ?? throw new ArgumentNullException(nameof(charactersRegistry));
        }

        public ICommandExecutor Create(CommandBase command, IPEndPoint clientEndPoint)
        {
            if (command is ConnectCommand connectCommand)
                return new ConnectCommandExecutor(connectCommand, clientEndPoint, _charactersRegistry);

            if (command is DisconnectCommand disconnectCommand)
                return new DisconnectCommandExecutor(disconnectCommand, clientEndPoint, _charactersRegistry);

            throw new NotImplementedException(command.GetType().FullName);
        }
    }
}
