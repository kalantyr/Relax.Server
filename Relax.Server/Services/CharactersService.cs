using System.Collections.Concurrent;
using Kalantyr.Auth.Client;
using Kalantyr.Web;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Relax.Server.Services
{
    public class CharactersService: IHealthCheck
    {
        private readonly IAppAuthClient _authClient;
        private static readonly IDictionary<uint, uint> _onlineCharacters = new ConcurrentDictionary<uint, uint>();

        public CharactersService(IAppAuthClient authClient)
        {
            _authClient = authClient ?? throw new ArgumentNullException(nameof(authClient));
        }

        public async Task<ResultDto<bool>> ConnectAsync(uint characterId, string userToken, CancellationToken cancellationToken)
        {
            var getUserIdResult = await _authClient.GetUserIdAsync(userToken, cancellationToken);
            if (getUserIdResult.Error != null)
                return new ResultDto<bool> { Error = getUserIdResult.Error };

            cancellationToken.ThrowIfCancellationRequested();

            if (!_onlineCharacters.ContainsKey(getUserIdResult.Result))
                _onlineCharacters.Add(getUserIdResult.Result, characterId);

            return ResultDto<bool>.Ok;
        }

        public async Task<ResultDto<bool>> DisconnectAsync(string userToken, CancellationToken cancellationToken)
        {
            var getUserIdResult = await _authClient.GetUserIdAsync(userToken, cancellationToken);
            if (getUserIdResult.Error != null)
                return new ResultDto<bool> { Error = getUserIdResult.Error };

            cancellationToken.ThrowIfCancellationRequested();

            if (_onlineCharacters.ContainsKey(getUserIdResult.Result))
                _onlineCharacters.Remove(getUserIdResult.Result);

            return ResultDto<bool>.Ok;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
        {
            try
            {
                if (_authClient is IHealthCheck hc1)
                {
                    var result = await hc1.CheckHealthAsync(context, cancellationToken);
                    if (result.Status != HealthStatus.Healthy)
                        return result;
                }

                return HealthCheckResult.Healthy();
            }
            catch (Exception e)
            {
                return HealthCheckResult.Unhealthy(nameof(CharactersService), e);
            }
        }

        public async Task<ResultDto<uint[]>> GetOnlineCharacterIdsAsync(string userToken, CancellationToken cancellationToken)
        {
            var getUserIdResult = await _authClient.GetUserIdAsync(userToken, cancellationToken);
            if (getUserIdResult.Error != null)
                return new ResultDto<uint[]> { Error = getUserIdResult.Error };

            cancellationToken.ThrowIfCancellationRequested();

            return new ResultDto<uint[]>
            {
                Result = _onlineCharacters.Values.ToArray()
            };
        }
    }
}
