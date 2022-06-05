using Kalantyr.Web;

namespace Relax.Server.Client
{
    public interface IServerClient
    {
        Task<ResultDto<bool>> ConnectAsync(uint characterId, string token, CancellationToken cancellationToken);
        
        Task<ResultDto<bool>> DisconnectAsync(string token, CancellationToken cancellationToken);
    }
}
