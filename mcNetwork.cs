using System;

namespace Obsidian
{
	/// <summary>
	/// Summary description for mcNetwork.
	/// </summary>
	public class mcNetwork
	{
		public string NetworkName;
		public string Nickname;
		public string Username;
		public string Realname;

		/* these all really need to be marked private. */
		public System.Collections.ArrayList Buddies = new System.Collections.ArrayList();
		public System.Collections.ArrayList Perform = new System.Collections.ArrayList();

		//these are stored like serverdns:port
		public System.Collections.ArrayList Servers = new System.Collections.ArrayList();

		public bool ConnectOnStartup = false;
		

		public mcNetwork()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/*
		 * saves a network object to disk.
		 * really i could have implemented this as object serialisation,
		 * but i can learn that later and do it then.
		 */
		public static int SaveNetwork(mcNetwork aNetwork)
		{
			System.IO.StreamWriter fd;
			string line;

			try
			{
				fd = new System.IO.StreamWriter("networks\\" + aNetwork.NetworkName + "\\network.dat");
			}
			catch (Exception ex)
			{
				System.Windows.Forms.MessageBox.Show("An exception occured while trying to write network file " + aNetwork.NetworkName + ": " + ex.ToString(), "Error!");
				return 0;
			}

			return 0;
		}

		/* Parses a network file, and returns a mcNetwork network object. */
		public static mcNetwork GetNetwork(string NetworkName)
		{
			mcNetwork NewNetwork = new mcNetwork();
			System.IO.StreamReader fd;
			string line;
			string temp;
			string[] parts;

			try
			{
				fd = new System.IO.StreamReader("networks\\" + NetworkName + "\\network.dat");
			}
			catch (Exception ex)
			{
				System.Windows.Forms.MessageBox.Show("An exception occured while trying to read network " + NetworkName + ": " + ex.ToString(), "Error!");
				return null;
			}

			while (true)
			{
				/* no, this does actually end ;) */
				line = fd.ReadLine();
				if (line == null)
					break;

				parts = line.Split(' ');

				switch (line[0])
				{
					case 'N':
						/* nickname token */
						if (parts.Length < 2)
						{
							System.Windows.Forms.MessageBox.Show("Malformed 'N' token in network file " + NetworkName + ", aborting read effort.", "Error!");
							return null;
						}
						NewNetwork.Nickname = parts[1];
						break;
					case 'U':
						/* username token */
						if (parts.Length < 2)
						{
							System.Windows.Forms.MessageBox.Show("Malformed 'U' token in network file " + NetworkName + ", aborting read effort.", "Error!");
							return null;
						}
						NewNetwork.Username = parts[1];
						break;
					case 'R':
						/* realname token */
						if (parts.Length < 2)
						{
							System.Windows.Forms.MessageBox.Show("Malformed 'R' token in network file " + NetworkName + ", aborting read effort.", "Error!");
							return null;
						}
						NewNetwork.Realname = parts[1];
						for (int i = 0; i < parts.Length; i++)
						{
							if (i > 1)
								NewNetwork.Realname = NewNetwork.Realname + " " + parts[i];
						}
						break;
					case 's':
						/* connect on startup */
						/* optional token- if it's here, we're supposed to connect on startup. */
						NewNetwork.ConnectOnStartup = true;
						break;
					case 'S':
						/* a server/port combination */
						NewNetwork.Servers.Add(parts[1]);
						break;
					case '#':
						/* perform comment */
						temp = parts[0];
						for (int i = 1; i < parts.Length; i++)
						{
							temp = temp + " " + parts[i];
						}
						NewNetwork.Perform.Add(temp);
						break;
					case 'P':
						/* perform line */
						temp = parts[1];
						for (int i = 0; i < parts.Length; i++)
						{
							if (i > 1)
								temp = temp + " " + parts[i];
						}
						NewNetwork.Perform.Add(temp);
						break;
				}
			}

			fd.Close();
			return NewNetwork;
		}
	}
}
