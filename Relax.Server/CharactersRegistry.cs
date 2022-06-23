using System.Diagnostics;
using System.Net;
using Kalantyr.Auth.Client;
using Relax.Characters.Client;
using Relax.Models.Commands;

namespace Relax.Server
{
    public class CharactersRegistry
    {
        private readonly IAppAuthClient _authClient;
        private readonly ICharactersReadonlyClient _charactersClient;
        private readonly ICollection<RegistryEntry> _entries = new List<RegistryEntry>();

        public IEnumerable<RegistryEntry> Registry => _entries;

        public CharactersRegistry(IAppAuthClient authClient, ICharactersReadonlyClient charactersClient)
        {
            _authClient = authClient ?? throw new ArgumentNullException(nameof(authClient));
            _charactersClient = charactersClient ?? throw new ArgumentNullException(nameof(charactersClient));
        }

        public async Task ConnectAsync(ConnectCommand connectCommand, IPEndPoint clientEndPoint)
        {
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            var getUserIdResult = await _authClient.GetUserIdAsync(connectCommand.UserToken, cancellationTokenSource.Token);
            if (getUserIdResult.Error != null)
                throw new Exception(getUserIdResult.Error.Code);

            var userId = getUserIdResult.Result;
            lock (_entries)
                if (_entries.Any(en => en.UserId == userId))
                    throw new Exception("User already connected");

            var getCharactersResult = await _charactersClient.GetMyCharactersIdsAsync(connectCommand.UserToken, cancellationTokenSource.Token);
            if (getCharactersResult.Error != null)
                throw new Exception(getCharactersResult.Error.Code);
            if (getCharactersResult.Result.All(charId => charId != connectCommand.CharacterId))
                throw new Exception("Invalid character ID ");

            lock(_entries)
                _entries.Add(new RegistryEntry(userId, connectCommand.CharacterId, clientEndPoint));

            Debug.WriteLine($"Пользователь {userId} подключился");
        }

        public void Disconnect(DisconnectCommand disconnectCommand, IPEndPoint clientEndPoint)
        {
            lock (_entries)
            {
                var entry = _entries.FirstOrDefault(e => e.EndPoint.Equals(clientEndPoint));
                Debug.WriteLine($"Пользователь {entry.UserId} отключился");
                _entries.Remove(entry);
            }
        }

        public class RegistryEntry
        {
            public uint UserId { get; }
            
            public uint CharacterId { get; }

            public IPEndPoint EndPoint { get; }

            public RegistryEntry(uint userId, uint characterId, IPEndPoint endPoint)
            {
                UserId = userId;
                CharacterId = characterId;
                EndPoint = endPoint;
            }
        }

        public uint? GetCharacterId(IPEndPoint clientEndPoint)
        {
            lock (_entries)
            {
                var entry = _entries.FirstOrDefault(re => re.EndPoint.Equals(clientEndPoint));
                return entry?.CharacterId;
            }
        }
    }
}
