using System.Net;
using Relax.Models.Commands;

namespace Relax.Server.CommandExecutors
{
    internal abstract class CommandExecutorBase : ICommandExecutor
    {
        public abstract void Execute(CommandBase command);
    }
}
