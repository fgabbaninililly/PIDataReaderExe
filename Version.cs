using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIDataReaderExe {
	public class Version {
		public static readonly string version = "0.1.0";

		public static string getVersion() {
			return string.Format("PIDataReader Console Launcher v{0}", version);
		}
	}
}
