using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Obsidian
{
	/// <summary>
	/// Summary description for mcIRCForm.
	/// </summary>
	sealed public class mcMainForm	: System.Windows.Forms.Form
	{
		public mcPage CurrentPage;
		public mcOptions OptionsForm = new mcOptions();
		public mcNetworkEditor NetworkEditor;
		public mcAbout AboutForm;

		private System.Collections.SortedList Servers = new System.Collections.SortedList(5);
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem mnuFileExit;
		private System.Windows.Forms.MenuItem mnuCreateNewServer;
		private System.Windows.Forms.MenuItem mnuHelp;
		private System.Windows.Forms.MenuItem mnuAbout;
		private System.Windows.Forms.MenuItem mnuView;
		private System.Windows.Forms.MenuItem mnuNetworkEditor;
		private System.Windows.Forms.MenuItem mnuViewOptions;
		private System.Windows.Forms.Splitter MySplitter;
		public System.Windows.Forms.TreeView tvcWindows;
		private System.Windows.Forms.Timer tmrParseStuff;
		private System.Windows.Forms.ToolBar tbLauncher;
		private System.Windows.Forms.ToolBarButton tbbtnNewServer;
		private System.Windows.Forms.ImageList tbImages;
		private System.Windows.Forms.ToolBarButton tbbtnConnectDisconnect;
		private System.ComponentModel.IContainer components;

		public mcMainForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(mcMainForm));
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.mnuCreateNewServer = new System.Windows.Forms.MenuItem();
			this.mnuFileExit = new System.Windows.Forms.MenuItem();
			this.mnuView = new System.Windows.Forms.MenuItem();
			this.mnuNetworkEditor = new System.Windows.Forms.MenuItem();
			this.mnuViewOptions = new System.Windows.Forms.MenuItem();
			this.mnuHelp = new System.Windows.Forms.MenuItem();
			this.mnuAbout = new System.Windows.Forms.MenuItem();
			this.MySplitter = new System.Windows.Forms.Splitter();
			this.tvcWindows = new System.Windows.Forms.TreeView();
			this.tmrParseStuff = new System.Windows.Forms.Timer(this.components);
			this.tbLauncher = new System.Windows.Forms.ToolBar();
			this.tbbtnNewServer = new System.Windows.Forms.ToolBarButton();
			this.tbbtnConnectDisconnect = new System.Windows.Forms.ToolBarButton();
			this.tbImages = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1,
																					  this.mnuView,
																					  this.mnuHelp});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuCreateNewServer,
																					  this.mnuFileExit});
			this.menuItem1.Text = "&File";
			// 
			// mnuCreateNewServer
			// 
			this.mnuCreateNewServer.Index = 0;
			this.mnuCreateNewServer.Text = "&New Server";
			this.mnuCreateNewServer.Click += new System.EventHandler(this.mnuCreateNewServer_Click);
			// 
			// mnuFileExit
			// 
			this.mnuFileExit.Index = 1;
			this.mnuFileExit.Text = "&Exit";
			this.mnuFileExit.Click += new System.EventHandler(this.mnuFileExit_Click);
			// 
			// mnuView
			// 
			this.mnuView.Index = 1;
			this.mnuView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuNetworkEditor,
																					this.mnuViewOptions});
			this.mnuView.Text = "View";
			// 
			// mnuNetworkEditor
			// 
			this.mnuNetworkEditor.Index = 0;
			this.mnuNetworkEditor.Text = "&Network Editor";
			this.mnuNetworkEditor.Click += new System.EventHandler(this.mnuNetworkEditor_Click);
			// 
			// mnuViewOptions
			// 
			this.mnuViewOptions.Index = 1;
			this.mnuViewOptions.Text = "&Options";
			this.mnuViewOptions.Click += new System.EventHandler(this.mnuViewOptions_Click);
			// 
			// mnuHelp
			// 
			this.mnuHelp.Index = 2;
			this.mnuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuAbout});
			this.mnuHelp.Text = "&Help";
			// 
			// mnuAbout
			// 
			this.mnuAbout.Index = 0;
			this.mnuAbout.Text = "&About";
			this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
			// 
			// MySplitter
			// 
			this.MySplitter.Location = new System.Drawing.Point(144, 0);
			this.MySplitter.Name = "MySplitter";
			this.MySplitter.Size = new System.Drawing.Size(3, 260);
			this.MySplitter.TabIndex = 5;
			this.MySplitter.TabStop = false;
			// 
			// tvcWindows
			// 
			this.tvcWindows.BackColor = System.Drawing.Color.Black;
			this.tvcWindows.Dock = System.Windows.Forms.DockStyle.Left;
			this.tvcWindows.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.tvcWindows.ForeColor = System.Drawing.Color.White;
			this.tvcWindows.ImageIndex = -1;
			this.tvcWindows.ItemHeight = 16;
			this.tvcWindows.Location = new System.Drawing.Point(0, 0);
			this.tvcWindows.Name = "tvcWindows";
			this.tvcWindows.SelectedImageIndex = -1;
			this.tvcWindows.Size = new System.Drawing.Size(144, 260);
			this.tvcWindows.TabIndex = 4;
			this.tvcWindows.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvcWindows_AfterSelect);
			// 
			// tmrParseStuff
			// 
			this.tmrParseStuff.Enabled = true;
			this.tmrParseStuff.Tick += new System.EventHandler(this.tmrParseStuff_Tick);
			// 
			// tbLauncher
			// 
			this.tbLauncher.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.tbLauncher.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						  this.tbbtnNewServer,
																						  this.tbbtnConnectDisconnect});
			this.tbLauncher.DropDownArrows = true;
			this.tbLauncher.ImageList = this.tbImages;
			this.tbLauncher.Location = new System.Drawing.Point(147, 0);
			this.tbLauncher.Name = "tbLauncher";
			this.tbLauncher.ShowToolTips = true;
			this.tbLauncher.Size = new System.Drawing.Size(501, 28);
			this.tbLauncher.TabIndex = 6;
			this.tbLauncher.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
			this.tbLauncher.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbLauncher_ButtonClick);
			// 
			// tbbtnNewServer
			// 
			this.tbbtnNewServer.ImageIndex = 0;
			this.tbbtnNewServer.Tag = "NEW_SERVER";
			this.tbbtnNewServer.ToolTipText = "Opens a new server window without disconnecting from the current one";
			// 
			// tbbtnConnectDisconnect
			// 
			this.tbbtnConnectDisconnect.ImageIndex = 1;
			this.tbbtnConnectDisconnect.Tag = "CONNECT_DISCONNECT";
			this.tbbtnConnectDisconnect.ToolTipText = "Connects (or disconnects) the currently selected server instance";
			// 
			// tbImages
			// 
			this.tbImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit;
			this.tbImages.ImageSize = new System.Drawing.Size(16, 16);
			this.tbImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("tbImages.ImageStream")));
			this.tbImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// mcMainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(648, 260);
			this.Controls.Add(this.tbLauncher);
			this.Controls.Add(this.MySplitter);
			this.Controls.Add(this.tvcWindows);
			this.Menu = this.mainMenu1;
			this.Name = "mcMainForm";
			this.Text = "Obsidian";
			this.Closed += new System.EventHandler(this.Exit);
			this.ResumeLayout(false);

		}
		#endregion

		private void mnuFileExit_Click(object sender, System.EventArgs e)
		{
			this.Exit(null, null);
		}

		public mcServer AddServer()
		{
			mcServer Server = new mcServer();
			System.Random Rnd = new Random();

			/*
			 * generate a random key.
			 * this should break out eventually.
			 */
			for (;;)
			{
				//100 chars should be sufficient entropy ;)...
				Server.HashKey = Server.HashKey + Rnd.Next(500000).ToString();
				if (!Servers.Contains(Server.HashKey))
				{
					//we have generated a unique hashkey
					Server.ServerPage.MyNode.Tag = Server.HashKey;
					TreeNode lvi = new	TreeNode("My Status");
					lvi.Tag = Server.HashKey;
					tvcWindows.Nodes.Add(lvi);
					Server.ServerPage.MyNode = lvi;

					lvi = new TreeNode("My Channels");
					lvi.Tag = Server.HashKey;
					Server.ServerPage.MyNode.Nodes.Add(lvi);
					Server.ServerPage.ChannelsNode = lvi;

					lvi = new TreeNode("My Messages");
					lvi.Tag = Server.HashKey;
					Server.ServerPage.MyNode.Nodes.Add(lvi);
					Server.ServerPage.MessagesNode = lvi;

					lvi = new TreeNode("My Buddies");
					lvi.Tag = Server.HashKey;
					Server.ServerPage.MyNode.Nodes.Add(lvi);
					Server.ServerPage.BuddiesNode = lvi;

					Servers.Add(Server.HashKey, Server);

					this.tvcWindows.ExpandAll();

					//fix: select this Server as active.
					this.tvcWindows_AfterSelect(this, new TreeViewEventArgs(Server.ServerPage.MyNode, TreeViewAction.ByMouse));
					return Server;
				}
			}
		}

		public void DeleteServer(mcServer Server)
		{
			Servers.Remove(Server.HashKey);
		}

		public void DeleteServer(string Key)
		{
			Servers.Remove(Key);
		}

		private void tvcWindows_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			mcPage aPage;
			mcServer aServer = (mcServer)Servers.GetByIndex(Servers.IndexOfKey(e.Node.Tag));

			/*
			 * Locate the server instance.
			 * aServer will never be null, as if the above fails
			 * the client will crash. This should be OK though
			 * s we should NOT be trying to select a non-existant window ;)
			 */

			/* find our page */
			aPage = aServer.FindPage(e.Node.Text);
			if (aPage == null)
			{
				/*
				 * all this generally means is that they selected, for example,
				 * a "messages" or "channels" node.
				 */
				aPage = aServer.CurrentPage;
			}
			/* now, we should have a page - focus on it */
			aPage.DoFocus();
		}

		public void Exit(object obj,System.EventArgs e)
		{
			foreach (mcServer aServer in Servers.Values)
			{
				aServer.Disconnect("Departing.");
			}
			Application.Exit();
		}

		private void mnuCreateNewServer_Click(object sender, System.EventArgs e)
		{
			this.AddServer();
		}

		private void mnuAbout_Click(object sender, System.EventArgs e)
		{
			this.AboutForm = new mcAbout();
			this.AboutForm.Show();
			this.AboutForm.SynchroniseOpacity();
		}

		private void mnuNetworkEditor_Click(object sender, System.EventArgs e)
		{
			this.NetworkEditor = new mcNetworkEditor();
			this.NetworkEditor.Show();
			this.NetworkEditor.SynchroniseOpacity();
		}
		public void SynchroniseOpacity()
		{
			//this is really ugly :(
			this.Opacity = this.OptionsForm.Setting_Opacity;
			try
			{
				this.AboutForm.SynchroniseOpacity();
			}
			catch
			{}
			try
			{
				this.NetworkEditor.SynchroniseOpacity();
			}
			catch
			{}
		}

		private void mnuViewOptions_Click(object sender, System.EventArgs e)
		{
			this.OptionsForm = new mcOptions();
			this.OptionsForm.Show();
			this.OptionsForm.SynchroniseOpacity();
		}

		private void tmrParseStuff_Tick(object sender, System.EventArgs e)
		{
			/* bit of an ugly way to handle things, but it works ;) */
			foreach (mcServer aServer in Servers.Values)
			{
				if (aServer.Connected)
				{
					if (aServer.ServerSocket.sck == null)
					{
						/* Remote server disconnected.*/
						aServer.Disconnect("Remote server closed socket.");
						/* fix: exception on remote server close */
						continue;
					}
					if (aServer.ServerSocket.Available() > 0)
					{
						//Data incoming...
						mcInbound.Parse(aServer.ServerSocket.GetData(), aServer.ServerPage);
					}
				}
			}
		}

		private void tbLauncher_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			switch (e.Button.Tag.ToString())
			{
				/*
				 * i'm aware that this is a totally FUGLY way of
				 * doing things - got any better ideas? :p
				 */
				case "NEW_SERVER":
					this.AddServer();
					break;
				case "CONNECT_DISCONNECT":
					if (this.CurrentPage.Server.Connected)
					{
						this.CurrentPage.Server.Disconnect("Departing.");
					}
					else
					{
						this.CurrentPage.Server.Connect();
					}
					break;
			}
		}
	}
}
