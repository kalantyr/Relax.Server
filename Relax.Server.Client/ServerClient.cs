using System.Net;
using System.Net.Sockets;
using Kalantyr.Web;
using Relax.Models.Commands;

namespace Relax.Server.Client
{
    public class ServerClient : IServerClient, IDisposable
    {
        private readonly Socket _socket;
        private readonly UdpClient _udpListener;
        private bool _isDisposing;

        public ServerClient(IPEndPoint serverEndPoint, int localUdpPort)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            var localEndpoint = new IPEndPoint(IPAddress.Loopback, localUdpPort);
            _socket.Bind(localEndpoint);
            _socket.Connect(serverEndPoint);

            _udpListener = new UdpClient(new IPEndPoint(serverEndPoint.Address, localUdpPort + 1));
            new Thread(Listen).Start();
        }

        private void Listen()
        {
            while (!_isDisposing)
            {
                IPEndPoint serverEndPoint = null;
                var data = _udpListener.Receive(ref serverEndPoint);
                var command = CommandBase.Deserialize(data);
                CommandReceived?.Invoke(command);
            }
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

        public event Action<CommandBase> CommandReceived;

        public void Dispose()
        {
            _isDisposing = true;
            if (_socket is { Connected: true })
            {
                _socket.Disconnect(true);
                _socket.Dispose();
            }
        }
    }
}
