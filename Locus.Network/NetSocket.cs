using System;
using System.Net;
using System.Net.Sockets;

namespace Locus.Network
{
    public class NetSocket : IDisposable
    {
        bool m_Disposed = false;
        Socket m_Socket;

        public int Port { get; protected set; }

        public bool CanRead
        {
            get { return m_Socket.Available > 0; }
        }

        public NetSocket()
        {

        }

        public void Start(int port = 0)
        {
            m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            var epToBind = new IPEndPoint(IPAddress.Any, port);

            try
            {
                const uint IOC_IN = 0x80000000;
                const uint IOC_VENDOR = 0x18000000;
                uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
                m_Socket.IOControl((int)SIO_UDP_CONNRESET, new[] { Convert.ToByte(false) }, null);
            }
            catch
            {
                NetLog.Warn("Failed to set control code for ignoring ICMP port unreachable.");
            }

            m_Socket.ReceiveBufferSize = 4194304;
            if (m_Socket.ReceiveBufferSize != 4194304) NetLog.Warn("ReceiveBufferSize restricted by OS.");
            m_Socket.SendBufferSize = 1048576;
            m_Socket.Blocking = false;

            do
            {
                try
                {
                    m_Socket.Bind(epToBind);
                }
                catch (SocketException e)
                {
                    if (e.SocketErrorCode == SocketError.AddressAlreadyInUse)
                    {
                        epToBind = new IPEndPoint(epToBind.Address, epToBind.Port + 1);
                        NetLog.Warn("Port in use. Incrementing and retrying...");
                    }
                    else
                    {
                        NetLog.Error(e.Message);
                    }
                }
            } while (!m_Socket.IsBound);

            Port = (m_Socket.LocalEndPoint as IPEndPoint).Port;
            NetLog.Log("Socket Bound at : " + m_Socket.LocalEndPoint);
        }


        public NetSendResult SendTo(byte[] buffer, int sendCount, NetEndPoint ep)
        {
            var bytesSent = m_Socket.SendTo(buffer, sendCount, SocketFlags.None, ep.EndPoint);
            return bytesSent > 0? NetSendResult.Success : NetSendResult.Failed;
        }


        public int ReceiveFrom(byte[] buffer, int sendCound, ref NetEndPoint ep)
        {
            var endPoint = (EndPoint)ep.EndPoint;
            return m_Socket.ReceiveFrom(buffer, buffer.Length, SocketFlags.None, ref endPoint);
        }


        public void Dispose()
        {
            if(!m_Disposed)
            {
                m_Disposed = true;
                if (m_Socket != null)
                {
                    m_Socket.Dispose();
                    m_Socket = null;
                }
            }
        }
    }
}
