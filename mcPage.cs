using System;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Obsidian
{
	/// <summary>
	/// Summary description for mcPage.
	/// </summary>
	sealed public class mcPage : System.Windows.Forms.UserControl
	{
		private string p_text;
		new public string Text
		{
			get
			{
				return p_text;
			}
			set
			{
				this.MyNode.Text = value;
				p_text = value;
			}
		}
		public string Topic
		{
			get
			{
				return txtTopic.Text;
			}
			set
			{
				this.txtTopic.Text = value;
			}
		}
		public bool IsChannel;
		public string HashKey; //how can i be identified?

		/* who owns me? */
		public mcServer Server;
		public TreeNode MyNode = new TreeNode();
		public TreeNode ChannelsNode = new TreeNode();
		public TreeNode MessagesNode = new TreeNode();
		public TreeNode BuddiesNode = new TreeNode();

		/* XXX - mark private ASAP */
		public System.Windows.Forms.TreeView tvcUsers;
		private System.Windows.Forms.Button cmdClosePage;
		private System.Windows.Forms.TextBox txtToSend;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.RichTextBox txtData;
		private System.Windows.Forms.ContextMenu ctmNicklist;
		private System.Windows.Forms.MenuItem mnuNicklistWhois;
		private System.Windows.Forms.TextBox txtTopic;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public mcPage(mcServer Server, string Title, bool IsChannel) 
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			this.Server = Server;
			this.Text = Title;

			if (!IsChannel)
			{
				this.txtData.Dock = DockStyle.Fill;
				this.panel1.Top = 0;
				this.panel1.Height = this.Height - this.txtToSend.Height;
				this.tvcUsers.Visible = false;
				this.txtTopic.Visible = false;
				this.Topic = null;
			}

			if (Obsidian.mainForm != null)
				Obsidian.mainForm.Controls.Add(this);

			this.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.SetIndent(StringWidth(TimeStamp()+"<> ", this.txtData.Font));
		}


		public mcPage()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			this.Text = "#chatspike";

			this.Server = new mcServer();
			this.Server.CurrentPage = this;
			this.Server.ServerPage = this;

			this.txtData.Dock = DockStyle.Fill;
			this.panel1.Top = 0;
			this.panel1.Height = this.Height - this.txtToSend.Height;
			this.tvcUsers.Visible = false;
			this.txtTopic.Visible = false;
			this.Topic = null;

			this.txtToSend.Visible = false;
			this.cmdClosePage.Visible = false;

			this.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.SetIndent(StringWidth(TimeStamp()+"<> ", this.txtData.Font));
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
		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>

		private void InitializeComponent()
		{
			this.cmdClosePage = new System.Windows.Forms.Button();
			this.txtToSend = new System.Windows.Forms.TextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tvcUsers = new System.Windows.Forms.TreeView();
			this.ctmNicklist = new System.Windows.Forms.ContextMenu();
			this.mnuNicklistWhois = new System.Windows.Forms.MenuItem();
			this.txtData = new System.Windows.Forms.RichTextBox();
			this.txtTopic = new System.Windows.Forms.TextBox();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// cmdClosePage
			// 
			this.cmdClosePage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdClosePage.Location = new System.Drawing.Point(400, 223);
			this.cmdClosePage.Name = "cmdClosePage";
			this.cmdClosePage.Size = new System.Drawing.Size(48, 17);
			this.cmdClosePage.TabIndex = 3;
			this.cmdClosePage.Text = "X";
			this.cmdClosePage.Click += new System.EventHandler(this.cmdClosePage_Click);
			// 
			// txtToSend
			// 
			this.txtToSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtToSend.BackColor = System.Drawing.Color.Black;
			this.txtToSend.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtToSend.ForeColor = System.Drawing.Color.White;
			this.txtToSend.Location = new System.Drawing.Point(0, 222);
			this.txtToSend.Multiline = true;
			this.txtToSend.Name = "txtToSend";
			this.txtToSend.Size = new System.Drawing.Size(472, 20);
			this.txtToSend.TabIndex = 5;
			this.txtToSend.Text = "";
			this.txtToSend.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtToSend_KeyPress);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.tvcUsers);
			this.panel1.Controls.Add(this.txtData);
			this.panel1.Location = new System.Drawing.Point(0, 22);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(448, 197);
			this.panel1.TabIndex = 9;
			// 
			// tvcUsers
			// 
			this.tvcUsers.BackColor = System.Drawing.Color.Black;
			this.tvcUsers.ContextMenu = this.ctmNicklist;
			this.tvcUsers.Dock = System.Windows.Forms.DockStyle.Right;
			this.tvcUsers.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.tvcUsers.ForeColor = System.Drawing.Color.White;
			this.tvcUsers.ImageIndex = -1;
			this.tvcUsers.Location = new System.Drawing.Point(344, 0);
			this.tvcUsers.Name = "tvcUsers";
			this.tvcUsers.SelectedImageIndex = -1;
			this.tvcUsers.Size = new System.Drawing.Size(104, 197);
			this.tvcUsers.Sorted = true;
			this.tvcUsers.TabIndex = 1;
			// 
			// ctmNicklist
			// 
			this.ctmNicklist.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.mnuNicklistWhois});
			// 
			// mnuNicklistWhois
			// 
			this.mnuNicklistWhois.Index = 0;
			this.mnuNicklistWhois.Text = "&Whois";
			this.mnuNicklistWhois.Click += new System.EventHandler(this.mnuNicklistWhois_Click);
			// 
			// txtData
			// 
			this.txtData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtData.AutoSize = true;
			this.txtData.BackColor = System.Drawing.Color.Black;
			this.txtData.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtData.ForeColor = System.Drawing.Color.White;
			this.txtData.Location = new System.Drawing.Point(0, 0);
			this.txtData.MaxLength = 2048;
			this.txtData.Name = "txtData";
			this.txtData.ReadOnly = true;
			this.txtData.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
			this.txtData.Size = new System.Drawing.Size(344, 200);
			this.txtData.TabIndex = 99;
			this.txtData.Text = "";
			this.txtData.TextChanged += new System.EventHandler(txtData_TextChanged);
			this.txtData.MouseUp += new System.Windows.Forms.MouseEventHandler(this.txtData_MouseUp);
			// 
			// txtTopic
			// 
			this.txtTopic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtTopic.BackColor = System.Drawing.Color.Black;
			this.txtTopic.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtTopic.ForeColor = System.Drawing.Color.White;
			this.txtTopic.Location = new System.Drawing.Point(0, 0);
			this.txtTopic.Name = "txtTopic";
			this.txtTopic.Size = new System.Drawing.Size(448, 21);
			this.txtTopic.TabIndex = 11;
			this.txtTopic.Text = "";
			this.txtTopic.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTopic_KeyPress);
			// 
			// mcPage
			// 
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.cmdClosePage);
			this.Controls.Add(this.txtToSend);
			this.Controls.Add(this.txtTopic);
			this.Name = "mcPage";
			this.Size = new System.Drawing.Size(448, 240);
			this.Tag = "ePAGE";
			this.TextChanged += new System.EventHandler(this.PageTextChanged);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion


		/* Actual code below here */
		/*
		 *  avoid using .Append externally, I'm still
		 *  considering making it private.
		 */
		public bool Append(string data) 
		{
			if(txtData == null)
				return false;

					txtData.AppendText(data);

			if(!Server.CurrentPage.Equals(this))
				this.ColourNode(Color.Red);
			return true;
		}
		
		public bool Save(string file) 
		{
			if(txtData == null || file == null || file.Length < 2)
				return false;

			txtData.SaveFile(file);
			return true;
		}

		private bool SetColor(System.Drawing.Color color) 
		{
			txtData.SelectionColor = color;
			return true;
		}

		private bool ResetColor() 
		{
			txtData.SelectionColor = System.Drawing.Color.White;
			return true;
		}

		private bool SetIndent(int pixels) 
		{
			if(pixels < 0 || pixels > txtData.Width)
				return false;
			txtData.SelectionHangingIndent = pixels;
			return true;
		}
		
		private void PageTextChanged(object sender, System.EventArgs e)
		{
			MyNode.Text = this.Text;
		}

		//public so windowlist can reset us.
		private void ScrollDown()
		{
			if (txtData != null && txtData.Text.Length > 0) 
			{
				txtData.SelectionStart = txtData.Text.Length - 1;
				txtData.Focus();
				this.txtToSend.Focus();
			}
		}

		private void txtData_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			System.Windows.Forms.Clipboard.SetDataObject(txtData.SelectedText, true);
			this.txtToSend.Focus();
		}

		private void txtData_TextChanged(object sender, System.EventArgs e)
		{
			if(txtData.TextLength < 1)
				return;
			if(txtData.Text[txtData.TextLength-1] == '\n')
				ScrollDown();
		}

		private void cmdClosePage_Click(object sender, System.EventArgs e)
		{
			if (this.Server.ServerPage.Equals(this))
			{
				/* todo: this needs to be configurable */
				this.Server.Disconnect("Departing.");
				Obsidian.mainForm.DeleteServer(this.Server);
				this.Server.CloseAllPages();
				this.Server.ServerPage.ClosePage();
				this.Server = null;
				return;
			}
			if (this.IsChannel)
				this.Server.IRCSend("PART " + this.Text);
			this.ClosePage();
		}
		public void ClosePage()
		{
			MyNode.Remove();
			Server.DeletePage(this);
		}

		private void txtToSend_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if ((int)e.KeyChar == 13)
			{
				/* Enter was pressed-- send it! */
				string mycmd = txtToSend.Text;
				txtToSend.Text = null;
				mcCommands.MainParser(this, mycmd);
				return;
			}
		}
		private void txtTopic_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if ((int)e.KeyChar == 13)
			{
				/* Enter was pressed-- change the topic */
				if (this.IsChannel)
				{
					this.Server.IRCSend("TOPIC " + this.Text + " :" + txtTopic.Text);
				}
				else
				{
					this.Topic = null;
				}
				return;
			}
		}

		private int StringWidth(string msg, Font font) 
		{
			Form aForm = new Form();
			Graphics g = aForm.CreateGraphics();
			SizeF size = new SizeF();
			size = g.MeasureString(msg, font);
			g.Dispose();
			aForm.Dispose();
			return (int)size.Width;
		}

		private string TimeStamp() 
		{
			System.DateTime timestamp = new System.DateTime();
			string timestr;

			timestamp = System.DateTime.Now;
			timestr = "[";
			timestr +=  (timestamp.Hour > 9)? timestamp.Hour.ToString() : "0"+timestamp.Hour;
			timestr += ":";
			timestr += (timestamp.Minute > 9)? timestamp.Minute.ToString() : "0"+timestamp.Minute;
			timestr += ":";
			timestr += (timestamp.Second > 9)? timestamp.Second.ToString() : "0"+timestamp.Second;
			timestr += "] ";
			return timestr;
		}

		public void MessageDisplay(string msg) 
		{
			if(msg != null && msg.Length > 0)
				this.Append(TimeStamp()+msg+"\r\n");
		}

		public bool MessageUser(string nick, string msg) 
		{
			if(nick.Length < 1 || msg == null || msg.Length < 1)
				return false;


			this.Append(TimeStamp());
			this.SetColor(System.Drawing.Color.Blue);
			this.Append("<"); 
			this.ResetColor();
			this.Append(nick); 
			this.SetColor(System.Drawing.Color.Blue);
			this.Append("> "); 
			this.ResetColor();
			this.Append(msg+"\r\n");
			this.SetIndent(0);

			return true;
		}

		public bool MessagePrivate(string nick, string msg) 
		{
			if (nick.Length < 1 || msg == null || msg.Length < 1)
				return false;
			
			this.Append(TimeStamp());
			this.SetColor(System.Drawing.Color.Green);
			this.Append(">"); 
			this.ResetColor();
			this.Append(nick); 
			this.SetColor(System.Drawing.Color.Green);
			this.Append("< "); 
			this.ResetColor();
			this.Append(msg+"\r\n"); 
	
			return true;
		}

		public bool MessageNotice(string nick, string msg) 
		{
			System.Drawing.Color pink = System.Drawing.Color.FromArgb(238, 34, 238);
			if (nick.Length < 1 || msg == null || msg.Length < 1)
				return false;

			this.Append(TimeStamp()); 
			this.SetColor(System.Drawing.Color.Blue);
			this.Append("-"); 
			this.SetColor(pink);
			this.Append(nick); 
			this.SetColor(System.Drawing.Color.Blue);
			this.Append("- "); 
			this.ResetColor();
			this.Append(msg+"\r\n"); 
	
			return true;
		}

		public bool MessageAction(string nick, string msg) 
		{
			if (nick.Length < 1 || msg == null || msg.Length < 1)
				return false;

			this.Append(TimeStamp());
			this.SetColor(System.Drawing.Color.Cyan);
			this.Append("*");
			this.ResetColor();
			this.Append(" "+nick+" "+msg+"\r\n");

			return true;
		}

		public bool MessageInfo(string msg) 
		{
			if (msg == null || msg.Length < 1)
				return false;

			this.Append(TimeStamp());
			this.Append("-");
			this.SetColor(System.Drawing.Color.DarkCyan);
			this.Append("-");
			this.SetColor(System.Drawing.Color.Cyan);
			this.Append("-");
			this.ResetColor();
			this.Append(" " + msg + "\r\n");

			return true;
		}

		public bool MessageJoin(string nick, string info) 
		{
			if (nick == null || info == null || nick.Length < 1 || info.Length < 1)
				return false;

			this.Append(TimeStamp());
			this.Append("-");
			this.SetColor(System.Drawing.Color.DarkCyan);
			this.Append("-");
			this.SetColor(System.Drawing.Color.Cyan);
			this.Append("> ");
			this.SetColor(System.Drawing.Color.White);
			this.Append(nick);
			this.SetColor(System.Drawing.Color.DarkGray);
			this.Append(" (");
			this.SetColor(System.Drawing.Color.DarkCyan);
			this.Append(info);
			this.SetColor(System.Drawing.Color.DarkGray);
			this.Append(")");
			this.ResetColor();
			this.Append(" has joined "+this.Text+"\r\n"); 


			if (nick == this.Server.MyNickname)
				return true;

			this.AddUserToChannel(nick, info);
			return true;
		}

		public bool MessagePart(string nick, string info, string reason) 
		{
			if (nick == null || info == null || nick.Length < 1 || info.Length < 1)
				return false;

			this.Append(TimeStamp());
			this.Append("<");
			this.SetColor(System.Drawing.Color.DarkCyan);
			this.Append("-");
			this.SetColor(System.Drawing.Color.Cyan);
			this.Append("- ");
			this.SetColor(System.Drawing.Color.White);
			this.Append(nick);
			this.SetColor(System.Drawing.Color.DarkGray);
			this.Append(" (");
			this.SetColor(System.Drawing.Color.DarkCyan);
			this.Append(info);
			this.SetColor(System.Drawing.Color.DarkGray);
			this.Append(")");
			this.ResetColor();
			this.Append(" has left "+this.Text); 
			
			if(reason == null || reason.Length < 1) 
			{
				this.Append("\r\n");
			} 
			else 
			{
				this.SetColor(System.Drawing.Color.DarkGray);
				this.Append(" (");
				this.ResetColor();
				this.Append(reason);
				this.SetColor(System.Drawing.Color.DarkGray);
				this.Append(")\r\n");
				this.ResetColor();
			}

			this.RemoveUserFromChannel(nick);
			return true;
		}

		public bool MessageKick(string target, string opnick, string reason) 
		{
			if (target == null || opnick == null)
				return false;

			this.Append(TimeStamp());
			this.Append("<");
			this.SetColor(System.Drawing.Color.DarkCyan);
			this.Append("-");
			this.SetColor(System.Drawing.Color.Cyan);
			this.Append("- ");
			this.SetColor(System.Drawing.Color.White);
			this.Append(target);
			this.ResetColor();
			this.Append(" has been kicked from "+this.Text+" by "+opnick);

			if(reason == null || reason.Length < 1)
			{
				this.Append("\r\n");
			}
			else
			{
				this.SetColor(System.Drawing.Color.DarkGray);
				this.Append(" (");
				this.ResetColor();
				this.Append(reason);
				this.SetColor(System.Drawing.Color.DarkGray);
				this.Append(")\r\n");
				this.ResetColor();
			}

			this.RemoveUserFromChannel(target);
			return true;
		}

		public bool MessageQuit(string qnick, string reason) 
		{
			if( qnick == null || qnick.Length < 1)
				return false;

			this.Append(TimeStamp());
			this.Append("<");
			this.SetColor(Color.DarkCyan);
			this.Append("-");
			this.SetColor(Color.Cyan);
			this.Append("- ");
			this.SetColor(Color.White);
			this.Append(qnick);
			this.ResetColor();
			this.Append(" has Quit");

			if(reason == null || reason.Length < 1) 
			{
				this.Append("\r\n");
			} 
			else 
			{
				this.SetColor(Color.DarkGray);
				this.Append(" (");
				this.ResetColor();
				this.Append(reason);
				this.SetColor(Color.DarkGray);
				this.Append(")\r\n");
				this.ResetColor();
			}
			return true;
		}

		public bool MessageTopic(string topic) 
		{
			if (topic == null)
				return false;

			this.txtTopic.Text = topic;
			if(this.Equals(this.Server.CurrentPage))
				this.Server.CurrentPage.txtTopic.Text = this.txtTopic.Text;

			this.Append(TimeStamp());
			this.Append("-");
			this.SetColor(System.Drawing.Color.DarkCyan);
			this.Append("-");
			this.SetColor(System.Drawing.Color.Cyan);
			this.Append("- ");
			this.ResetColor();
			this.Append("Topic for ");
			this.SetColor(System.Drawing.Color.Cyan);
			this.Append(this.Text);
			this.ResetColor();
			this.Append(" is ");
			this.SetColor(System.Drawing.Color.Cyan);
			this.Append(topic);
			this.ResetColor();
			this.Append("\r\n");

			return true;
		}

		public bool MessageTopicTime(string user, string rawtime) 
		{
			//TODO: There is a known issue to do with this.
			//Basically, the date doesn't work! :p
			System.DateTime time = new System.DateTime(System.Int32.Parse(rawtime));
			this.Append(TimeStamp());
			this.Append("-");
			this.SetColor(System.Drawing.Color.DarkCyan);
			this.Append("-");
			this.SetColor(System.Drawing.Color.Cyan);
			this.Append("- ");
			this.ResetColor();
			this.Append("Topic for ");
			this.SetColor(System.Drawing.Color.Cyan);
			this.Append(this.Text);
			this.ResetColor();
			this.Append(" set by ");
			this.SetColor(System.Drawing.Color.Cyan);
			this.Append(user);
			this.ResetColor();
			this.Append(" at ");
			this.SetColor(System.Drawing.Color.Cyan);
			this.Append(time.ToString("ddd, MMM d HH:mm:ss"));
			this.Append("DEBUG: " + time.ToString());
			this.ResetColor();
			this.Append("\r\n");

			return true;
		}

		private void mnuNicklistWhois_Click(object sender, System.EventArgs e)
		{
			this.Server.IRCSend("WHOIS " + this.tvcUsers.SelectedNode.Tag);
		}

		private void ColourNode(Color ink) 
		{
			MyNode.ForeColor = ink;
		}

		public void DoFocus()
		{
			this.BringToFront();
			MyNode.ForeColor = System.Drawing.Color.White;
			this.Server.CurrentPage = this;
			Obsidian.mainForm.CurrentPage = this;
			this.txtToSend.Focus();
		}

		public void AddUserToChannel(string nick, string info)
		{
			if (this.GetUserOnChannelByNick(nick) != null)
				return;

			System.Windows.Forms.TreeNode lvi = new TreeNode(nick);
			lvi.Tag = lvi.Text;
			this.tvcUsers.Nodes.Add(lvi);
		}

		public void RemoveUserFromChannel(string nick)
		{
			TreeNode tvnode = GetUserOnChannelByNick(nick);
			if (tvnode != null)
				tvnode.Remove();
		}

		public TreeNode GetUserOnChannelByNick(string nick)
		{
			foreach (TreeNode tvNode in this.tvcUsers.Nodes)
			{
				if (tvNode.Tag.ToString().ToUpper() == nick.ToUpper())
					return tvNode;
			}

			return null;
		}
	}
}
