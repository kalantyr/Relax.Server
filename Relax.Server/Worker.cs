using System.Net;
using System.Net.Sockets;
using Relax.Models.Commands;
using Relax.Server.CommandExecutors;

namespace Relax.Server
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly CharactersRegistry _charactersRegistry;
        private readonly UdpClient _udpClient;
        private readonly CommandExecutorFactory _commandExecutorFactory = new();

        public Worker(ILogger<Worker> logger, CharactersRegistry charactersRegistry)
        {
            _logger = logger;
            _charactersRegistry = charactersRegistry ?? throw new ArgumentNullException(nameof(charactersRegistry));
            _udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, 12345));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var receiveResult = await _udpClient.ReceiveAsync(stoppingToken);
                    var command = CommandBase.Deserialize(receiveResult.Buffer);

                    if (command is ConnectCommand connectCommand)
                    {
                        var connectTask = _charactersRegistry.ConnectAsync(connectCommand, receiveResult.RemoteEndPoint);
                        connectTask.Start();
                    }
                    else if (command is DisconnectCommand disconnectCommand)
                        _charactersRegistry.Disconnect(disconnectCommand);
                    else
                    {
                        var commandExecutor = _commandExecutorFactory.Create(command);
                        commandExecutor.Execute(command);
                    }
                }
            }
            finally
            {
                _udpClient.Close();
                _udpClient.Dispose();
            }
        }
    }
}