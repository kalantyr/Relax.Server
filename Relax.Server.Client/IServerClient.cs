using Kalantyr.Web;
using Relax.Models.Commands;

namespace Relax.Server.Client
{
    public interface IServerClient
    {
        Task<ResultDto<uint[]>> GetOnlineCharacterIdsAsync(string token, CancellationToken cancellationToken);
        
        Task SendAsync(CommandBase command, CancellationToken cancellationToken);

        event Action<CommandBase> CommandReceived;
    }
}
