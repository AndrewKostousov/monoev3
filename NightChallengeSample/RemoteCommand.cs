namespace NightChallengeSample
{
	public enum RemoteCommand : byte
	{
		None = 0,
		LeftUp = 1,
		LeftDown = 2,
		RightUp = 3,
		RightDown = 4,
		LeftUpAndRightUp = 5,
		LeftUpAndRightDown = 6,
		LeftDownAndRightUp = 7,
		LeftDownAndRightDown = 8,
		Beacon = 9,
		LeftUpAndLeftDown = 10,
		RightUpAndRightDown = 11
	}
}