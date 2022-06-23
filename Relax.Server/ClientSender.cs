using System.Net;
using System.Net.Sockets;
using Relax.Models.Commands;

namespace Relax.Server
{
    public interface IClientSender
    {
        void SendToClient(CommandBase command, IPEndPoint clientEndPoint);

        void SendToClient(CommandBase command, IEnumerable<IPEndPoint> clientEndPoints);
    }

    internal class ClientSender : IClientSender
    {
        public void SendToClient(CommandBase command, IPEndPoint clientEndPoint)
        {
            using var udpClient = new UdpClient(AddressFamily.InterNetwork);
            try
            {
                udpClient.Connect(new IPEndPoint(clientEndPoint.Address, clientEndPoint.Port + 1));
                udpClient.Send(command.Serialize());
            }
            finally
            {
                udpClient.Close();
            }
        }

        public void SendToClient(CommandBase command, IEnumerable<IPEndPoint> clientEndPoints)
        {
            // TODO: replace with JoinMulticastGroup

            using var udpClient = new UdpClient(AddressFamily.InterNetwork);
            try
            {
                foreach (var clientEndPoint in clientEndPoints)
                {
                    udpClient.Connect(new IPEndPoint(clientEndPoint.Address, clientEndPoint.Port + 1));
                    udpClient.Send(command.Serialize());
                }
            }
            finally
            {
                udpClient.Close();
            }
        }
    }
}
