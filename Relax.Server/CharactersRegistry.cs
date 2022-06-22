using System.Net;
using Kalantyr.Auth.Client;
using Relax.Models.Commands;

namespace Relax.Server
{
    public class CharactersRegistry
    {
        private readonly IAppAuthClient _authClient;
        private readonly ICollection<RegistryEntry> _entries = new List<RegistryEntry>();

        public CharactersRegistry(IAppAuthClient authClient)
        {
            _authClient = authClient ?? throw new ArgumentNullException(nameof(authClient));
        }

        public async Task ConnectAsync(ConnectCommand connectCommand, IPEndPoint clientEndPoint)
        {
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            var getUserIdResult = await _authClient.GetUserIdAsync(connectCommand.UserToken, cancellationTokenSource.Token);
            if (getUserIdResult.Error != null)
                throw new Exception(getUserIdResult.Error.Code);

            var userId = getUserIdResult.Result;
            if (_entries.Any(en => en.UserId == userId))
                throw new Exception("Пользователь уже подключен");

            throw new NotImplementedException();
        }

        public void Disconnect(DisconnectCommand disconnectCommand)
        {
            throw new NotImplementedException();
        }

        public class RegistryEntry
        {
            public uint UserId { get; }

            public IPEndPoint EndPoint { get; }

            public RegistryEntry(uint userId, IPEndPoint endPoint)
            {
                UserId = userId;
                EndPoint = endPoint;
            }
        }
    }
}
