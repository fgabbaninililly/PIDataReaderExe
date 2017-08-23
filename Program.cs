using NLog;
using PIDataReaderCommons;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace PIDataReaderExe {
	class PIDataReaderConsoleLauncher {
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private PIDRController pidrCtrl;

		private static AutoResetEvent waitTimerDisposedHandle = new AutoResetEvent(false);

		[DllImport("Kernel32")]
		private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);
		private delegate bool EventHandler(CtrlType sig);
		static EventHandler appClosingHandler;

		enum CtrlType {
			CTRL_C_EVENT = 0,
			CTRL_BREAK_EVENT = 1,
			CTRL_CLOSE_EVENT = 2,
			CTRL_LOGOFF_EVENT = 5,
			CTRL_SHUTDOWN_EVENT = 6
		}

		static void Main(string[] args) {
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
			PIDataReaderConsoleLauncher pidrLauncher = new PIDataReaderConsoleLauncher();
			pidrLauncher.run(args);
		}

		public void run(string[] args) {
			pidrCtrl = new PIDRController(Version.getVersion(), false);
			int res = pidrCtrl.start(args);
			if (ExitCodes.EXITCODE_SUCCESS != res) {
				logger.Fatal("Failed to start service! Reason: {0}", ExitCodes.Instance[res]);
				logger.Fatal("Press any key to continue");
				Console.ReadKey();
				return;
			}
			appClosingHandler += new EventHandler(this.appClosingHandlerImpl);
			SetConsoleCtrlHandler(appClosingHandler, true);

			waitTimerDisposedHandle.WaitOne();
			logger.Trace("Timer disposed handle unblocked");
		}

		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
			logger.Fatal("Unhandled exception. Details: {0}", e.ExceptionObject.ToString());
		}

		private bool appClosingHandlerImpl(CtrlType sig) {
			string eventCode = "Ctrl-C";
			bool retCode = false;
			switch (sig) {
				case CtrlType.CTRL_C_EVENT:
					eventCode = "Ctrl-C";
					break;
				case CtrlType.CTRL_LOGOFF_EVENT:
					eventCode = "Logoff";
					break;
				case CtrlType.CTRL_SHUTDOWN_EVENT:
					eventCode = "Shutdown";
					break;
				case CtrlType.CTRL_CLOSE_EVENT:
					eventCode = "App close";
					break;
				default:
					break;
			}
			logger.Info("Reader closing due to application termination ({0})", eventCode);

			pidrCtrl.sendMail();
			
			//DELIBERATELY NOT TRYING TO CLOSE MQTT CLIENT GRACEFULLY: WE WANT IT TO SEND A LAST WILL MESSAGE!

			LogManager.Flush();
			LogManager.Shutdown();
			return retCode;
		}
	}
}
