using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Obsidian
{
	/// <summary>
	/// Where w00t writes his c# implementation of tcpsocket, and falls arse over tit.
	/// </summary>
	public class mcSocket
	{
		private Socket aSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		//private AsyncCallback AcceptCB = new AsyncCallback() ;
		//										Dim ReadCB As AsyncCallback = New AsyncCallback(AddressOf m_Read)
		//																			  Public SendCB As AsyncCallback = New AsyncCallback(AddressOf m_Send)
		//																													   Dim ConnectCB As AsyncCallback = New AsyncCallback(AddressOf m_Connect)
		//																																								Dim DnsCB As AsyncCallback = New AsyncCallback(AddressOf m_Dns)
		public mcSocket()
		{
			/*
			 * Socket aSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			 * Umm, remember to use PollRead etc ;) otherwise you'll be in for a shock :)
			 */
			aSock.Blocking = true;
		}

		public void Bind(string host, int port)
		{
			IPAddress ip;

			try
			{
				ip = IPAddress.Parse(host);
			}
			catch
			{
				ip = Dns.Resolve(host).AddressList[0];
			}
			//ignore this warning.
			aSock.Bind(new IPEndPoint(ip, port));
		}

		public void Connect(string host, int port)
		{
			IPAddress ip;

			try
			{
				ip = IPAddress.Parse(host);
			}
			catch
			{
				ip = Dns.Resolve(host).AddressList[0];
			}
			aSock.Connect(new IPEndPoint(ip, port));
		}

		public void Listen()
		{
			//TODO: Optional variable BackLog to listen to
			//how do we do optional vars in C#?
			aSock.Listen(50);
		}
		
		public mcSocket Accept()
		{
			mcSocket t = new mcSocket();
			t.aSock.Close();
			t.aSock = this.aSock.Accept();
			return t;
		}
		
		public void Shutdown(System.Net.Sockets.SocketShutdown method)
		{
			aSock.Shutdown(method);
		}
		
		public void Close()
		{
			aSock.Close();
		}

		public void Send(string data)
		{
			if (data == null || data == "")
			{
				return;
			}
			if (aSock == null)
			{
				System.Diagnostics.Debug.Assert(false);
			}
			byte[] b = Encoding.ASCII.GetBytes(data);
			aSock.Send(b);
		}

		public string Recv()
		{
			byte[] b = new byte[aSock.Available];
			aSock.Receive(b);
			return Encoding.ASCII.GetString(b);
		}

		public bool PollRead()
		{
			//TODO: Make timeout optional?
			return (aSock.Poll(100,SelectMode.SelectRead));
		}

		public bool PollWrite()
		{
			//TODO: Make timeout optional?
			return (aSock.Poll(100,SelectMode.SelectWrite));
		}

		public bool PollError()
		{
			//TODO: Make timeout optional?
			return (aSock.Poll(100,SelectMode.SelectError));
		}

		public int Available()
		{
			return aSock.Available;
		}

		public int GetError()
		{
			if (!PollError())
			{
				return 0;
			}
			return (int)aSock.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Error);
		}

		//dunno if this will work...
		public string RemoteIP
		{
			get
			{
				return aSock.RemoteEndPoint.ToString();
			}
		}

		//dunno if this will work...
		public string RemoteHost
		{
			get
			{
				try
				{
					return Dns.Resolve(aSock.RemoteEndPoint.ToString()).HostName;
				}
				catch
				{
					return aSock.RemoteEndPoint.ToString();
				}
			}
		}

		/* let's try do asynch dns.. */

	}
}
