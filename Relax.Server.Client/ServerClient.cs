using System.Net;
using System.Net.Sockets;
using Kalantyr.Web;
using Relax.Models.Commands;

namespace Relax.Server.Client
{
    public class ServerClient : IServerClient
    {
        private readonly IPEndPoint _serverEndPoint;

        public ServerClient(IPEndPoint serverEndPoint)
        {
            _serverEndPoint = serverEndPoint ?? throw new ArgumentNullException(nameof(serverEndPoint));
        }

        public async Task<ResultDto<bool>> ConnectAsync(uint characterId, string token, CancellationToken cancellationToken)
        {
            using var udpClient = new UdpClient();
            try
            {
                var cmd = new ConnectCommand(characterId, token);
                udpClient.Connect(_serverEndPoint);
                await udpClient.SendAsync(cmd.Serialize(), cancellationToken);
            }
            finally
            {
                udpClient.Close();
            }

            return new ResultDto<bool> { Result = true };
        }

        public async Task<ResultDto<bool>> DisconnectAsync(string token, CancellationToken cancellationToken)
        {
            using var udpClient = new UdpClient();
            try
            {
                var cmd = new DisconnectCommand(token);
                udpClient.Connect(_serverEndPoint);
                await udpClient.SendAsync(cmd.Serialize(), cancellationToken);
            }
            finally
            {
                udpClient.Close();
            }

            return new ResultDto<bool> { Result = true };
        }

        public async Task<ResultDto<uint[]>> GetOnlineCharacterIdsAsync(string token, CancellationToken cancellationToken)
        {
            return new ResultDto<uint[]> { Result = Array.Empty<uint>() };

            // TODO

            //_requestEnricher.Token = token;
            //return await Get<ResultDto<uint[]>>("/characters/onlineCharacterIds", cancellationToken);
        }
    }
}
