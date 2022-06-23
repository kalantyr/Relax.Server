using Kalantyr.Web;
using Relax.Models.Commands;

namespace Relax.Server.Client
{
    public interface IServerClient
    {
        Task<ResultDto<bool>> ConnectAsync(uint characterId, string token, CancellationToken cancellationToken);
        
        Task<ResultDto<bool>> DisconnectAsync(string token, CancellationToken cancellationToken);

        Task<ResultDto<uint[]>> GetOnlineCharacterIdsAsync(string token, CancellationToken cancellationToken);
        
        Task SendAsync(CommandBase command, CancellationToken cancellationToken);
    }
}
