using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Locus.Network
{
    public class NetEndPoint
    {
        IPEndPoint m_EndPoint;

        public NetEndPoint(IPEndPoint ep)
        {
            m_EndPoint = ep;
        }

        public IPEndPoint EndPoint
        {
            get { return m_EndPoint; }
        }

        public bool IsIPV6
        {
            get { return m_EndPoint.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6; }
        }

        public override bool Equals(object obj)
        {
            return m_EndPoint.Equals(obj);
        }

        public override string ToString()
        {
            return m_EndPoint.ToString();
        }

        public override int GetHashCode()
        {
            return m_EndPoint.GetHashCode();
        }
    }
}
