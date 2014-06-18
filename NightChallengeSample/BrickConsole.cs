using System;
using MonoBrickFirmware.Display;

namespace NightChallengeSample
{
	public static class BrickConsole
	{
		public static void WriteLine(string format, params object[] args)
		{
			var message = string.Format(format, args);
			Console.WriteLine(message);
			LcdConsole.WriteLine(message);
		}
	}
}