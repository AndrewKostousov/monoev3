using System;

namespace NightChallengeSample
{
	[Flags]
	public enum RemoteButton
	{
		None = 0,
		LeftUp = 1,
		LeftDown = 2,
		RightUp = 4,
		RightDown = 8,
		Beacon = 16,
	}
}