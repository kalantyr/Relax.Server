using System.Net;
using Relax.Models.Commands;

namespace Relax.Server.CommandExecutors
{
    internal class ConnectCommandExecutor: ICommandExecutor
    {
        private readonly ConnectCommand _command;
        private readonly IPEndPoint _clientEndPoint;
        private readonly CharactersRegistry _charactersRegistry;

        public ConnectCommandExecutor(ConnectCommand command, IPEndPoint clientEndPoint, CharactersRegistry charactersRegistry)
        {
            _command = command ?? throw new ArgumentNullException(nameof(command));
            _clientEndPoint = clientEndPoint ?? throw new ArgumentNullException(nameof(clientEndPoint));
            _charactersRegistry = charactersRegistry ?? throw new ArgumentNullException(nameof(charactersRegistry));
        }

        public void Execute()
        {
            // TODO: в параллельный поток

            _charactersRegistry.ConnectAsync(_command, _clientEndPoint)
                .Wait();

            /*
            using var udpClient = new UdpClient(AddressFamily.InterNetwork);
            try
            {
                udpClient.Connect(_clientEndPoint);
                udpClient.Send(new byte[] { 1, 2, 3, 4, 5 });
            }
            finally
            {
                udpClient.Close();
            }
            */
        }
    }
}
