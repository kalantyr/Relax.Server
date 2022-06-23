using System.Net;
using System.Net.Sockets;
using Relax.Models.Commands;
using Relax.Server.CommandExecutors;

namespace Relax.Server
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly UdpClient _udpClient;
        private readonly CommandExecutorFactory _commandExecutorFactory;

        public Worker(ILogger<Worker> logger, CharactersRegistry charactersRegistry)
        {
            _logger = logger;
            _udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, 12345));
            _commandExecutorFactory = new CommandExecutorFactory(charactersRegistry);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var receiveResult = await _udpClient.ReceiveAsync(stoppingToken);
                    var command = CommandBase.Deserialize(receiveResult.Buffer);
                    var commandExecutor = _commandExecutorFactory.Create(command, receiveResult.RemoteEndPoint);
                    commandExecutor.Execute();
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