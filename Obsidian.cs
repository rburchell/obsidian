using System;
using System.Reflection;
using System.Windows.Forms;

namespace Obsidian
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited =false)]
	public class Args : System.Attribute
	{
		private readonly int _Args;
		public Args(int MinimumArguments)
		{
			this._Args = MinimumArguments;
		}
	}

	public class Obsidian
    {
		public const string APP_NAME = "Obsidian";
		public const string APP_VER = "0.0.1-Pie-Alpha1";

		public static mcMainForm mainForm = new mcMainForm();

		[STAThread]
		public static void Main()
		{
			/* make sure the networks directory exists here.. */
			if (!System.IO.Directory.Exists("networks"))
				System.IO.Directory.CreateDirectory("networks");

			/* allocate our first Server instance. */
			mcServer aServer;
			aServer = Obsidian.mainForm.AddServer();
			aServer.ServerPage.MessageInfo("Welcome to " + APP_NAME + " v" + APP_VER);

			/* here goes nothing.. */
			System.Windows.Forms.Application.Run(mainForm);
		}
	}
}