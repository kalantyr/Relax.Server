using Relax.Models.Commands;

namespace Relax.Server.CommandExecutors
{
    internal class CommandExecutorFactory
    {
        public ICommandExecutor Create(CommandBase command)
        {
            throw new NotImplementedException(command.GetType().FullName);
        }
    }
}
