using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms; //for treenodes etc

namespace Obsidian
{
	/// <summary>
	/// Summary description for mcServer.
	/// </summary>
	sealed public class mcServer
	{
		public enum PageType
		{
			Channel,
			Message
		}
		sealed public class RPL_ISupport
		{
			//What features does my host Server support?
			public bool SILENCE = false;
			public bool SAFELIST = false;
			public bool WATCH = false;

			public int MAXCHANNELS = 10;
			public int NICKLEN = 12;
			public int CHANNELLEN = 32;
			public int TOPICLEN = 307;
			public int KICKLEN = 307;
			public int AWAYLEN = 307;
			public int MAXTARGETS = 20;

			public string PREFIX_Modes = "ohv";
			public string PREFIX_Characters = "@%+";
		}

		/* My properties. */
		public string MyNickname;
		public string MyUsername;
		public string MyRealname;


		/* Server properties. */
		public RPL_ISupport ISupport = new RPL_ISupport();
		public string HashKey; //<-- what is my "key" in the hash?
		public string ServerName;
		public int ServerPort;
		
		/* is this needed? */
		//private bool _connectionregistered;
		//public bool ConnectionRegistered //numeric 001 recieved?
		//{
		//	get
		//	{
		//		return this._connectionregistered;
		//	}
		//}

		private bool _connected;
		/* we don't want external functions to be able to mark us unconnected */
		public bool Connected
		{
			get
			{
				return this._connected;
			}
		}

		/* The "status window," if you want to call it that. (is never null) */
		public mcPage ServerPage;
		/* currently (or last) "active" page of this server */
		public mcPage CurrentPage;

		/* My collection of pages. */
		private System.Collections.SortedList Pages = new System.Collections.SortedList(5);

		/* todo: mark this private? */
		public NetworkThread.BufferedSocket ServerSocket;

		public mcServer()
		{
			ServerName = "";
			ServerPort = 6667;
			MyNickname = SystemInformation.UserName;
			MyUsername = MyNickname;
			MyRealname = MyNickname;
			ServerPage = new mcPage(this, "My Status", false);

			CurrentPage = ServerPage;
			ServerPage.DoFocus();
		}

		/* Page API */
		public mcPage FindPage(string PageName)
		{
			/* Check for special pages first. */
			//TODO: An idea, maybe we could make them like "My Status" "My Raw" and add them straight into the Pages array?
			if (PageName.ToLower().CompareTo(ServerPage.Text.ToLower()) == 0)
			{
				return ServerPage;
			}

			System.Collections.IEnumerator PageKeys = Pages.Keys.GetEnumerator();
			PageName = PageName.ToLower();
			while (PageKeys.MoveNext())
			{
				if (PageName.CompareTo(((string) PageKeys.Current).ToLower()) == 0)
				{
					return (mcPage)Pages[PageKeys.Current];
				}
			}
			return null;
		}

		public mcPage AddPage(string PageName, PageType TypeOfPage)
		{
			/* Add a page to the Server instance and return it. */
			mcPage aPage;
			System.Windows.Forms.TreeNode lvi = new	System.Windows.Forms.TreeNode(PageName);
			lvi.Tag = this.HashKey;

			switch (TypeOfPage)
			{
				case PageType.Channel:
					aPage = new mcPage(this, PageName, true);
					this.ServerPage.ChannelsNode.Nodes.Add(lvi);
					break;
				case PageType.Message:
					aPage = new mcPage(this, PageName, false);
					this.ServerPage.MessagesNode.Nodes.Add(lvi);
					break;
				default:
					/* unsupported page type?! */
					aPage = new mcPage(this, PageName, false);
					MessageBox.Show("mcServer.AddPage was given an invalid PageType for page " + aPage.Text);
					this.ServerPage.MyNode.Nodes.Add(lvi);
					break;
			}

			
			aPage.MyNode = lvi;
			Pages.Add(PageName, aPage);
			/* resize controls etc. -- is this needed here? */
			Obsidian.mainForm.ReinitGUI();

			/* reset focus on current page. */
			CurrentPage.DoFocus();

			/* We don't do duplicates checking (ie two #test's),
			 * this is assumed to have already been done by a 
			 * seperate proceedure (hint: FindPage)
			 */
			return aPage;
		}

		public void DeletePage(mcPage Page)
		{
			Pages.Remove(Page.Text);
			Page.Dispose();
		}

		public void CloseAllPages()
		{
			/* not to be used lightly..! */
			//todo: should this be disposing pages too?
			this.Pages.Clear();
		}

		/* Socket stuff. */
		public void IRCSend(string msg)
		{
			if(!Connected)
				return;
			ServerSocket.SendData(msg + "\r\n");
		}

		public void Connect()
		{
			/* Try to connect (this is slow :/) */
			//TODO: Asynchronous sockets.
			this.ServerPage.MessageInfo("Attempting to connect to " + this.ServerName + " on " + this.ServerPort.ToString());
#if NeverDefineThis
			try
			{
				ServerSocket.Connect(this.ServerName, this.ServerPort);
			}
			catch (Exception ex)
			{
				this.ServerPage.MessageInfo("Waah, couldn't connect: " + ex.ToString());
				return;
			}

			/* we connected? register ourselves .. */
			this._connected = true;
			this.IRCSend("NICK " + this.MyNickname);
			this.IRCSend("USER " + this.MyUsername + " * * :" + this.MyRealname);
#endif
			Obsidian.DoConnect(this.ServerName, this.ServerPort, new NetworkThread.ConnectCallback(this.ServerSocket_Connect));
		}

		private void ServerSocket_Connect(NetworkThread.BufferedSocket sck) 
		{
			if (sck.sck != null) 
			{
				/* SUCCESS!!! */
				this.ServerSocket = sck;
				this._connected = true;
				this.IRCSend("NICK " + this.MyNickname);
				this.IRCSend("USER " + this.MyUsername + " * * :" + this.MyRealname);
			}
			else 
			{
				/* FAILED :( */
				this.ServerPage.MessageInfo("Waah, couldn't connect: " + sck.sckError.ToString());
				return;
			}
		}

		public void Disconnect(string msg)
		{
			/* Disconnect this Server instance */
			if (ServerSocket != null && Connected)
			{
				try
				{
					this.IRCSend("QUIT :" + msg);
				}
				catch
				{
					/* ignore, the Server might've closed us. */
				}
				ServerSocket.Disconnect();
			}

			this._connected = false;
			this.ServerPage.MessageInfo("Disconnected (" + msg + ")");			
		}

		public void QuitNick(string Nick, string Reason)
		{
			/* remove nick from all pages */
			foreach (mcPage aPage in this.Pages.Values)
			{
				if (aPage.GetUserOnChannelByNick(Nick) != null)
				{
					aPage.MessageQuit(Nick, Reason);
					aPage.RemoveUserFromChannel(Nick);
				}
			}
		}
		public void ChangeNick(string OldNick, string NewNick)
		{
			/* Change OldNick to NewNick on all given pages. */
			foreach (mcPage aPage in this.Pages.Values)
			{
				TreeNode moo = aPage.GetUserOnChannelByNick(OldNick);
				if (moo == null)
					continue;	/* HOPEFULLY a self nickchange? Dunno.. */

				moo.Text = NewNick;
				moo.Tag = NewNick;
				aPage.MessageInfo(OldNick + " is now known as " + NewNick);
			}
		}
	}
}
