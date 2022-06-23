using System.Net;
using System.Net.Sockets;
using Kalantyr.Web;
using Relax.Models.Commands;

namespace Relax.Server.Client
{
    public class ServerClient : IServerClient, IDisposable
    {
        private readonly Socket _socket;

        public ServerClient(IPEndPoint serverEndPoint, int localUdpPort)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            var localEndpoint = new IPEndPoint(IPAddress.Loopback, localUdpPort);
            _socket.Bind(localEndpoint);
            _socket.Connect(serverEndPoint);
        }

        public async Task<ResultDto<bool>> ConnectAsync(uint characterId, string token, CancellationToken cancellationToken)
        {
            var cmd = new ConnectCommand(characterId, token);
            await SendAsync(cmd, cancellationToken);
            return new ResultDto<bool> { Result = true };
        }

        public async Task<ResultDto<bool>> DisconnectAsync(string token, CancellationToken cancellationToken)
        {
            await SendAsync(new DisconnectCommand(), cancellationToken);
            return new ResultDto<bool> { Result = true };
        }

        public async Task<ResultDto<uint[]>> GetOnlineCharacterIdsAsync(string token, CancellationToken cancellationToken)
        {
            return new ResultDto<uint[]> { Result = Array.Empty<uint>() };

            // TODO

            //_requestEnricher.Token = token;
            //return await Get<ResultDto<uint[]>>("/characters/onlineCharacterIds", cancellationToken);
        }

        public async Task SendAsync(CommandBase command, CancellationToken cancellationToken)
        {
            var data = command.Serialize();

            var res = await _socket.SendAsync(data, SocketFlags.None, cancellationToken);
            if (res != data.Length)
                throw new Exception("Send error");
        }

        public void Dispose()
        {
            if (_socket is { Connected: true })
            {
                _socket.Disconnect(true);
                _socket.Dispose();
            }
        }
    }
}
