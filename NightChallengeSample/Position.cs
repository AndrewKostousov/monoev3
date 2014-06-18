namespace NightChallengeSample
{
	public struct Position
	{
		public double X;
		public double Y;

		public static Position operator +(Position current, Position shift)
		{
			return new Position {X = current.X + shift.X, Y = current.Y + shift.Y};
		}

		public static Position operator -(Position current, Position shift)
		{
			return new Position {X = current.X - shift.X, Y = current.Y - shift.Y};
		}

		public override string ToString()
		{
			return string.Format("X={0:0.0} Y={1:0.0}", X, Y);
		}
	}
}