using System.Collections.Generic;

namespace NightChallengeSample
{
	public static class RemoteMapping
	{
		public static RemoteButton ToButtons(this RemoteCommand cmd)
		{
			RemoteButton btn;
			return mapping.TryGetValue(cmd, out btn) ? btn : RemoteButton.None;
		}

		private static readonly Dictionary<RemoteCommand, RemoteButton> mapping = new Dictionary<RemoteCommand, RemoteButton>
		{
			{RemoteCommand.LeftUp, RemoteButton.LeftUp},
			{RemoteCommand.LeftDown, RemoteButton.LeftDown},
			{RemoteCommand.RightUp, RemoteButton.RightUp},
			{RemoteCommand.RightDown, RemoteButton.RightDown},
			{RemoteCommand.LeftUpAndRightUp, RemoteButton.LeftUp | RemoteButton.RightUp},
			{RemoteCommand.LeftUpAndRightDown, RemoteButton.LeftUp | RemoteButton.RightDown},
			{RemoteCommand.LeftDownAndRightUp, RemoteButton.LeftDown | RemoteButton.RightUp},
			{RemoteCommand.LeftDownAndRightDown, RemoteButton.LeftDown | RemoteButton.RightDown},
			{RemoteCommand.LeftUpAndLeftDown, RemoteButton.LeftUp | RemoteButton.LeftDown},
			{RemoteCommand.RightUpAndRightDown, RemoteButton.RightUp | RemoteButton.RightDown},
			{RemoteCommand.Beacon, RemoteButton.Beacon},
		};
	}
}