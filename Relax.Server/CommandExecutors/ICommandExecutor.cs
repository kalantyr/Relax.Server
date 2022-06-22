using Relax.Models.Commands;

namespace Relax.Server.CommandExecutors;

internal interface ICommandExecutor
{
    void Execute(CommandBase command);
}