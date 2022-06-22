using Kalantyr.Web;
using Kalantyr.Web.Impl;

namespace Relax.Server.Client
{
    public class ServerClient : HttpClientBase, IServerClient
    {
        private readonly TokenRequestEnricher _requestEnricher;

        public ServerClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory, new TokenRequestEnricher())
        {
            _requestEnricher = (TokenRequestEnricher)RequestEnricher;
        }

        public async Task<ResultDto<bool>> ConnectAsync(uint characterId, string token, CancellationToken cancellationToken)
        {
            _requestEnricher.Token = token;
            return await Post<ResultDto<bool>>("/characters/connect?characterId=" + characterId, null, cancellationToken);
        }

        public async Task<ResultDto<bool>> DisconnectAsync(string token, CancellationToken cancellationToken)
        {
            _requestEnricher.Token = token;
            return await Post<ResultDto<bool>>("/characters/disconnect", null, cancellationToken);
        }

        public async Task<ResultDto<uint[]>> GetOnlineCharacterIdsAsync(string token, CancellationToken cancellationToken)
        {
            _requestEnricher.Token = token;
            return await Get<ResultDto<uint[]>>("/characters/onlineCharacterIds", cancellationToken);
        }
    }
}
