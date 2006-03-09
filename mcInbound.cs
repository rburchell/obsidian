/* TODO (some day in the far off future)
 *   IRCd protocol modules, IRCd detected through 005 numeric.
 *   This would allow for cool ircd-independant features like
 *   inspircd's +g channelmode, and others :)
 */

using System;
using System.Text;

namespace Obsidian
{
	/// <summary>
	/// New incoming data processer.
	/// </summary>
	sealed public class mcInbound
	{

		private static string tempBuffer = "";
		
		public static void Parse(string data, mcPage page)
		{
			/* used to tokenise a single message */
			System.Text.StringBuilder part = new System.Text.StringBuilder();
			char prevchar;
			string[] parts; /* todo: can we ever recieve > 100 params? */
			int i;
			int pcount;

			/*
			 *  what do you get when you look at a tired australian
			 * programmer?.. one who makes mistakes.. oops.
			 */
			string prefix, command;
			
			/* filled by splitting \n */
			string[] messages;

			/* make \r into \n and split into commands */
			/* TODO: buffering. */
			data = data.Replace('\r', '\n');
			messages = data.Split('\n');
			
			/* If we have any tempBuffer from the last parse, and more than one element,
			 * then we have the end of the incomplete buffer from the last parse. */
			if (tempBuffer.Length > 0 && messages.Length > 1) 
			{
				messages[0] = tempBuffer + messages[0];
				tempBuffer = "";
			}

			/* The last element should be empty. If it isn't we have an incomplete buffer. */
			if (messages[messages.Length - 1] != "") 
			{
				tempBuffer = messages[messages.Length - 1];
				messages[messages.Length - 1] = "";
			}

			foreach (string message in messages)
			{
				/* 
				 * we might get CMD param\r\n which gets translated to
				 * CMD param\n\n above, giving us empty entries.
				 * If we don't ignore them here, we get problems :p
				 */
				if (message == "")
					continue;

				//page.MessageInfo("Processing " + message);

				pcount = 0;
				prevchar = 'b'; /* c# can really suck sometimes.. */
				parts = new string[100];
				part = new System.Text.StringBuilder();

				for (i = 0; i < message.Length + 1; i++)
				{
					if (i > message.Length - 1)
					{
						/* one last parameter */
						//System.Windows.Forms.MessageBox.Show("LASTPARAM!");
						parts.SetValue(part.ToString(), pcount);
						break;
					}

					if (message[i] == ' ')
					{
						/* finish parameter.. */
						parts.SetValue(part.ToString(), pcount);
						part = new System.Text.StringBuilder();
						pcount++;
					}
					else if (message[i] ==  ':' && prevchar == ' ')
					{
						/* lump everything into one last param */
						parts.SetValue(message.Substring(i + 1), pcount);
						break;
					}
					else
					{
							part.Append(message[i]);
					}
					prevchar = message[i];
				}

				if(message[0] == ':') 
				{
					/* if line has a prefix */
					prefix = parts[0].Substring(1); /* use a substring to remove ':' */
					command = parts[1];
				} 
				else 
				{
					/* null prefix */
					prefix = null;
					command = parts[0];
				}
				ActualParse(prefix, command, parts, page);
			}
		}

		private static void ActualParse(string prefix, string command, string[] parameters, mcPage page)
		{
			mcPage target = null;
			string todisplay = null; /* join parts of parameters together to display */
			string[] userhost;
			int i = 0;

			switch (command)
			{
				case "001":
					//<- :devel.rburchell.org 001 w-mirc :Welcome to the Symmetic IRC Network w-mirc!w00t__@127.0.0.1
					page.Server.ServerName = prefix;	/* this is a good time to find the real name */
					page.Server.ServerPage.MyNode.Text = page.Server.ServerName;
					page.Server.ServerPage.Text = page.Server.ServerName;	/* and make sure the node matches 8) */
					//split param 3 to get network name..
					userhost = parameters[3].Split(' ');

					if (System.IO.File.Exists(".\\networks\\" + userhost[3] + "\\network.dat"))
					{
						page.MessageInfo("Network " + userhost[3] + " is a known network.");
						/* We have a networks file. */
						mcNetwork NewNetwork = mcNetwork.GetNetwork(userhost[3]);
						page.Server.IRCSend("NICK " + NewNetwork.Nickname);
						/* assume that user/real is already valid. */
						//todo: add this to the list of servers?
						//page.Server.ServerSocket.RemoteHost
						foreach (string str in NewNetwork.Perform)
						{
							if (str[0] != '#')
							{
								/* the "P" is stripped automagically */
								page.MessageInfo("Doing PERFORM " + str);
								page.Server.IRCSend(str);
							}
						}
					}
					else
					{
						page.MessageInfo("Network " + userhost[3] + " is not a known network.");
					}
					break;
				case "002":
					//<- :devel.rburchell.org 002 w-mirc :Your host is devel.rburchell.org, running version Unreal3.2.3
					page.MessageInfo(parameters[3]);
					break;
				case "003":
					//<- :devel.rburchell.org 003 w-mirc :This server was created Sat Apr 16 23:22:52 2005
					page.MessageInfo(parameters[3]);
					break;
				case "004":
					//<- :devel.rburchell.org 004 w-mirc devel.rburchell.org Unreal3.2.3 iowghraAsORTVSxNCWqBzvdHtGpE lvhopsmntikrRcaqOALQbSeIKVfMCuzNTGj
					page.MessageInfo(parameters[3] + " " + parameters[4] + " " + parameters[5] + " " + parameters[6]);
					break;
				case "005":  
					/* todo: do we need to support RPL_BOUNCE too? */
					//<- :devel.rburchell.org 005 w00t SAFELIST HCN MAXCHANNELS=10 CHANLIMIT=#:10 MAXLIST=b:60,e:60,I:60 NICKLEN=30 CHANNELLEN=32 TOPICLEN=307 KICKLEN=307 AWAYLEN=307 MAXTARGETS=20 WALLCHOPS WATCH=128 :are supported by this Server
					//<- :devel.rburchell.org 005 w00t SILENCE=15 MODES=12 CHANTYPES=# PREFIX=(qaohv)~&@%+ CHANMODES=beI,kfL,lj,psmntirRcOAQKVGCuzNSMTG NETWORK=Symmetic CASEMAPPING=ascii EXTBAN=~,cqnr ELIST=MNUCT STATUSMSG=~&@%+ EXCEPTS INVEX CMDS=KNOCK,MAP,DCCALLOW,USERIP :are supported by this Server
					foreach (string str in parameters)
					{
						//TODO: Finish RPL_ISUPPORT parsing.
						//str will be in form: TOK=VALUE
						//or optionally, TOK.
						if (str == null)
							break;
						string[] tokens = str.Split("=".ToCharArray());
						switch (tokens[0])
						{
							case "WATCH":
								page.Server.ISupport.WATCH = true;
								break;
							case "SILENCE":
								page.Server.ISupport.SILENCE = true;
								break;
							case "SAFELIST":
								page.Server.ISupport.SAFELIST = true;
								break;
							case "CHANMODES":
								/* CHANMODES=beI,kfL,lj,psmntirRcOAQKVGCuzNSMTG */
								userhost = tokens[1].Split(',');
								break;
							case "PREFIX":
								/* tokens[1] is something like (ohv)@%+ */
								tokens[1] = tokens[1].Substring(1, tokens[1].Length - 1);
								/* now ohv)@%+ - find the ) in the middle */
								int a = tokens[1].IndexOf(")");
								/* split string into prefixchars and equivilant modes */
								string modechars = tokens[1].Substring(0, a);
								string prefixchars = tokens[1].Substring(a+1);
								if (modechars.Length != prefixchars.Length)
								{
									/* o_O I want to hear if this ever happens :P */
									page.MessageInfo("Remote Server sent malformed 005 PREFIX token, ignoring. Please report this.");
									continue;
								}
								page.Server.ISupport.PREFIX_Characters = prefixchars;
								page.Server.ISupport.PREFIX_Modes = modechars;
								break;
							default:
								break;
						}
					}
					
					/* todo: this is inefficient given we loop over the array twice */
					todisplay = "Options: ";
					foreach (string tmp in parameters)
					{
						if (i > 2)
							todisplay = todisplay + " " + tmp;
						i++;
					}
					page.MessageInfo(todisplay);
					break;
				case "251":  // RPL_LUSERCLIENT 
					// ":There are <integer> users and <integer> services on <integer> servers" 
					page.MessageInfo(parameters[3]);
					break;
				case "252":  // RPL_LUSEROP 
					// "<integer> :operator(s) online" 
					page.MessageInfo(parameters[3] + " IRC operator(s) online");
					break;
				case "253":
					// "<integer> :unknown connection(s)" 
					page.MessageInfo(parameters[3] + " unknown connection(s)");
					break;
				case "254":
					//:devel.rburchell.org 254 w00t_ 1 :channels formed
					page.MessageInfo(parameters[3] + " channel(s) formed");
					break;
				case "255":
					//:devel.rburchell.org 255 w00t_ :I have 3 clients and 0 servers
					page.MessageInfo(parameters[3]);
					break;
				case "265":
					//:devel.rburchell.org 265 w00t_ :Current Local Users: 3  Max: 500
					page.MessageInfo(parameters[3]);
					break;
				case "266":
					//:devel.rburchell.org 266 w00t_ :Current Global Users: 3  Max: 3
					page.MessageInfo(parameters[3]);
					break;
/* WHOIS NUMERICS */
/*
 * TODO: A few more to be implemnted when I get time:
 * [16:38:25] --- UNKNOWN:  :irc.viroteck.net 310 Rob w00t is available for help.
 * [16:40:10] --- UNKNOWN:  :irc.viroteck.net 301 Rob Stskeeps CeBIT
 * [16:40:10] --- UNKNOWN:  :irc.viroteck.net 313 Rob Stskeeps is a Network Administrator
 * [16:40:10] --- UNKNOWN:  :irc.viroteck.net 671 Rob Stskeeps is using a Secure Connection
 */
				case "307":
					page.Server.CurrentPage.MessageInfo("Status: " + parameters[3] + " is a Registered Nickname");
					break;
				case "311":
					page.Server.CurrentPage.MessageInfo("-------------[WHOIS: " + parameters[3] + "]------------");
					page.Server.CurrentPage.MessageInfo("NUH: " + parameters[3] + "!" + parameters[4] + "@" + parameters[5]);
					
					for (i = 7; i < parameters.Length; i++)
						todisplay = todisplay + " " + parameters[i];
					
					page.Server.CurrentPage.MessageInfo("Real Name:" + todisplay);
					break;
				case "312":
					for (i = 4; i < parameters.Length; i++)
						todisplay = todisplay + " " + parameters[i];
					
					page.Server.CurrentPage.MessageInfo("Server:" + todisplay);
					break;
				case "318":
					page.Server.CurrentPage.MessageInfo("-------------[END OF WHOIS]------------");
					break;
				case "319":
					for (i = 4; i < parameters.Length; i++)
						todisplay = todisplay + " " + parameters[i];
					
					page.Server.CurrentPage.MessageInfo("Channels:" + todisplay);
					break;
				case "320":
					for (i = 3; i < parameters.Length; i++)
						todisplay = todisplay + " " + parameters[i];
					
					page.Server.CurrentPage.MessageInfo("Description:" + todisplay);
					break;
/* END WHOIS NUMERICS */
				case "332":
					//<- :devel.rburchell.org 332 w00t #test :ggagagagaqg
					target = page.Server.FindPage(parameters[3]);
					if(target == null) 
						return;
					target.MessageTopic(parameters[4]);	
					break;
				case "333":
					//<- :devel.rburchell.org 333 w00t #test w-mirc 1125627086
					target = page.Server.FindPage(parameters[3]);
					if(target == null) 
						return;
					target.MessageTopicTime(parameters[4], parameters[5]);
					break;
				case "353":  
					/*
					 *  RPL_NAMREPLY 
					 * <- :devel.rburchell.org 353 w-mirc = #test :@w-mirc 
					 * <- :devel.rburchell.org 366 w-mirc #test :End of /NAMES list.
					 */
					string[] userlist;
					
					/* pull channel out of info */
					target = page.Server.FindPage(parameters[4]);

					if (target == null)
						return;

					/* populate user list */
					/* TODO: clear the list first? */
					userlist = parameters[5].Split(' ');
					target.lstUsers.BeginUpdate(); 
					foreach (string name in userlist) 
					{
						StringBuilder thenick = new StringBuilder();
						StringBuilder theprefix = new StringBuilder();

						if (name.Length < 1)
							continue;

						foreach (char nickchar in name)
						{
							bool isprefix = false;

							foreach (char prefixchar in target.Server.ISupport.PREFIX_Characters)
							{
								if (prefixchar == nickchar)
								{
									/* it's a prefix */
									isprefix = true;
									break;
								}
							}

							if (isprefix == false)
								thenick.Append(nickchar);
							else
								theprefix.Append(nickchar);
						}

						target.AddUserToChannel(thenick.ToString(), null);
						foreach (char c in theprefix.ToString())
							target.AddPrefix(thenick.ToString(), c);
					}
					target.lstUsers.EndUpdate();
					break;
				case "366":
					// RPL_ENDOFNAMES 
					// do nothing.
					break;
				case "372":  // RPL_MOTD 
					// ":- <text>"
					page.MessageInfo(parameters[3]);
					break;
				case "375":  // RPL_MOTDSTART 
					// ":- <Server> Message of the day - " 					
					page.MessageInfo(parameters[3]);
					break;
				case "422":
					/* no motd */
					page.MessageInfo(parameters[3]);
					break;
				case "376":
					// RPL_ENDOFMOTD 
					page.MessageInfo(parameters[3]);
					break;
				case "433":
					//<- :devel.rburchell.org 433 * w-mirc :Nickname is already in use.
					page.Server.ServerPage.MessageInfo(parameters[3] + " is already in use. Retrying with " + parameters[3] + "_");
					page.Server.MyNickname = parameters[3] + "_";
					mcCommands.MainParser(page, "/nick " + parameters[3] + "_");
					break;
				case "421":
					//:devel.rburchell.org 421 w00t me :Unknown command
					page.Server.CurrentPage.MessageInfo(parameters[3] + ": " + parameters[4]);
					break;
					/* Now commands time. */
				case "ERROR":
					// Command: ERROR   Parameters: <error message>
					page.Server.ServerPage.MessageInfo("ERROR " + parameters[1]);
					/* this is being a bit cheeky ;) */
					page.Server.Disconnect("ERROR " + parameters[1]);
					break;
				//case "INVITE":

				//	break;
				case "JOIN":
					//:w00t!u@h JOIN :#test
					target = page.Server.FindPage(parameters[2]);
					userhost = prefix.Split('!');
					if(target == null)
					{
						/* Joining a new channel. */
						target = page.Server.AddPage(parameters[2], mcServer.PageType.Channel);
						target.IsChannel = true;
					}
					target.MessageJoin(userhost[0], userhost[1]);
					break;
				case "KICK":
					/* :aggressor!u@h KICK #somechan target :reason */
					userhost = prefix.Split('!');
					target = page.Server.FindPage(parameters[2].ToLower());
					if(target == null)
						return; /* probably server lag. */

					target.MessageKick(parameters[3], userhost[0], parameters[3]);
					//if(nick.Equals(page.Server.MyNickname))
					//	page.Server.DeletePage(target);
					break;
				case "NICK":
					// Command: NICK    Parameters: :<new>
					userhost = prefix.Split('!');
					if(userhost[0] == page.Server.MyNickname) 
					{
						page.Server.MyNickname = parameters[2];
					}
					page.Server.ChangeNick(userhost[0], parameters[2]);
					break;
				case "NOTICE":
					// Command: NOTICE  Parameters: <msgtarget> :<text>
					string source;
					if(prefix != null) 
					{
						userhost = prefix.Split('!');
						source = userhost[0];
					} 
					else 
					{
						source = page.Server.ServerName;
					}
					//parts = parameters.Split(null);
					//message = parameters.Substring(parts[0].Length + 
					//	((parameters.Substring(parts[0].Length).StartsWith(" :"))?2:1));
					target = page.Server.FindPage(parameters[2]);
					if(target != null)
						target.MessageNotice(source, parameters[3]);
					else
					{
						/*
						 * Dilemma, should we be sending to ACTIVE page,
						 * or Server page. Reminder to self, add
						 * CurrentPage to mcServer class.
						 */
						page.Server.ServerPage.MessageNotice(source, parameters[3]);
					}
					break;
				case "PART":
					//:n!u@h PART #chan :message
					userhost = prefix.Split('!');
					target = page.Server.FindPage(parameters[2]);
					if (target == null)
						return;

					target.MessagePart(userhost[0], userhost[1], parameters[3]);

					if(userhost[0] == page.Server.MyNickname)
						page.Server.DeletePage(target);
					break;
				case "PING":
					//PING :something.goes.here
					page.Server.IRCSend("PONG " + parameters[1]);
					break;
				case "PRIVMSG":
					//:n!u@h PRIVMSG target :message here!
					userhost = prefix.Split('!');
					if(parameters[3][0] == '\u0001') 
					{
						//todo: redo CTCP support (:
						CTCP(prefix, parameters, page);
						return;
					}
					target = page.Server.FindPage(parameters[2]);
					if(target != null) 
					{
						target.MessageUser(userhost[0], parameters[3]);
					} 
					else 
					{
						target = page.Server.FindPage(userhost[0]);
						if(target == null) 
						{
							page.Server.AddPage(userhost[0], mcServer.PageType.Message);
							target = page.Server.FindPage(userhost[0]);
						}
						target.MessageUser(userhost[0], parameters[3]);
					}
					break;
				case "QUIT":
					//:prefix!u@h QUIT :message :o
					userhost = prefix.Split('!');
					page.Server.QuitNick(userhost[0], parameters[2]);
					break;
				case "TOPIC":
					//:n!u@h TOPIC #chan :newtopic zomg!
					userhost = prefix.Split('!');
					target = page.Server.FindPage(parameters[2]);
					if(target == null) 
						return;

					target.Topic = parameters[3];
					target.MessageInfo(userhost[0] + " has changed the topic to: "+target.Topic);
					if(target.Equals(page.Server.CurrentPage))
						page.Server.CurrentPage.Topic = target.Topic;
					break;
				default:
					/* unknown numeric/command */
					todisplay = "UNKNOWN: ";
					foreach (string str in parameters)
					{
						todisplay = todisplay + " " + str;
					}
					page.MessageInfo(todisplay);
					break;
			}	
		}

		private static void CTCP(string prefix, string[] parameters, mcPage page)
		{
			//:n!u@h PRIVMSG #blah :VERSION
			mcPage target;
			string[] userhost;
			string[] ctcpparams;

			parameters[3] = parameters[3].Replace("", "");

			userhost = prefix.Split('!');
			ctcpparams = parameters[3].Split(' ');

			target = page.Server.FindPage(parameters[2]);
			if (target == null)
			{
				/* no channel page, try find a private one.. */
				target = page.Server.FindPage(userhost[0]);
			}

			switch (ctcpparams[0])
			{
				case "ACTION":
					/* if we didn't find a page previously, create one (presume private ;)) */
					if (target == null)
					{
						target = page.Server.AddPage(userhost[0], mcServer.PageType.Message);
					}
					int i = 0;
					string action = null;
					foreach (string str in ctcpparams)
					{
						if (i == 0) /* ugly :p */
						{
							i++;
							continue;
						}
						action = action + " " + str;
					}
					target.MessageAction(userhost[0], action);
					break;
				case "VERSION":
					if (target == null)
					{
						target = page.Server.ServerPage;
					}
					target.MessageInfo("[" + userhost[0] + " VERSION]");
					page.Server.IRCSend("NOTICE " + userhost[0] + " :VERSION  " + Obsidian.APP_NAME + " " + Obsidian.APP_VER + " - Got it yet?\r\n");
					break;
			}
			
		}	
	}
}
