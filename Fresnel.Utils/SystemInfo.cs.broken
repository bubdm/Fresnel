using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Envivo.TrueView;
using Microsoft.VisualBasic.Devices;

namespace Envivo.Fresnel.Utils
{
	/// <summary>
	/// 
	/// </summary>
	internal class SystemInfo
	{
		internal SystemInfo()
		{
			this.DetermineRuntimeHost();
		}

		internal Computer Computer = new Computer();
		internal ComputerInfo ComputerInfo = new ComputerInfo();

		internal bool IsRunningUnderAspNet { get; private set; }

		internal bool IsRunningUnderWcf { get; private set; }

		internal bool IsRunningOnClient { get; private set; }

		internal bool IsRunningOnServer { get; private set; }

		private void DetermineRuntimeHost()
		{
			var frameworkChecker = new NetFxVersionChecker();

			this.IsRunningUnderAspNet = System.Web.HttpContext.Current != null;
			this.IsRunningUnderWcf = (frameworkChecker.IsInstalled(FrameworkVersion.Fx30) &&
			(new WcfOperationContextChecker().IsAvailable()));
			this.IsRunningOnClient = (!this.IsRunningUnderAspNet && !this.IsRunningUnderWcf);
			this.IsRunningOnServer = !this.IsRunningOnClient;
		}

		internal void EmitSystemInformation(Preferences.UserPreferences preferences)
		{
			try
			{
				const long MEGABYTE = 1024 * 1024;
				const short MINIMUM_FREE_MEMORY = 64;   // MB!

				long totalMemory = (long)this.ComputerInfo.TotalPhysicalMemory / MEGABYTE;
				long freeMemory = (long)this.ComputerInfo.AvailablePhysicalMemory / MEGABYTE;

				long totalSwap = (long)this.ComputerInfo.TotalVirtualMemory / MEGABYTE;
				long freeSwap = (long)this.ComputerInfo.AvailableVirtualMemory / MEGABYTE;

				Trace.TraceInformation("Application started by '{0}' on workstation '{1}'", Environment.UserName, Environment.MachineName);

				Trace.TraceInformation("Running on {0}{1}, {2}Mb RAM ({3}Mb free), {4}Mb swap ({5}Mb free)",
					this.ComputerInfo.OSFullName.Trim(),
					Environment.OSVersion.ServicePack.IsNotEmpty() ? " " + Environment.OSVersion.ServicePack.Trim() : "",
					totalMemory, freeMemory, totalSwap, freeSwap);

				if (freeMemory < MINIMUM_FREE_MEMORY)
				{
					Trace.TraceWarning("This application requires at least {0}Mb to run adequately", MINIMUM_FREE_MEMORY);
				}

				if (preferences.CommsSettings.CheckNetworkAtStartup)
				{
					if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
					{
						Trace.TraceError("A network connection is not available");
					}
				}
			}
			catch (Exception ex)
			{
				My.Instance.Engine.LogException(ex);
			}
		}



		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		private static extern bool SetProcessWorkingSetSize(IntPtr handle, int minSize, int maxSize);


		/// <summary>
		/// Minimises the process' working set memory.
		/// </summary>
		/// <remarks>Use this to manage the application's memory usage</remarks>
		internal static void MinimiseWorkingSet()
		{
			SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
		}

		private long _previousBytesInUse = GC.GetTotalMemory(true);

		internal void CollectMemory()
		{
			MinimiseWorkingSet();

			if (System.Diagnostics.Debugger.IsAttached)
			{
				// This allows us to monitor memory usage for the My.Application.
				// NB: Each item in the MessageListView consumes about 1Kb, so that may be ignored.

				for (int i = 1; i <= 3; i++)
				{
					GC.Collect();
					GC.WaitForPendingFinalizers();
				}

				long bytesInUse = GC.GetTotalMemory(true);
				long difference = bytesInUse - _previousBytesInUse;
				_previousBytesInUse = bytesInUse;

				System.Diagnostics.Debug.WriteLine(string.Format("Current Memory: {0:#,###,##0} bytes ({1:+#,###,##0;-#,###,##0;0})", bytesInUse, difference));
			}
		}

	}

}
