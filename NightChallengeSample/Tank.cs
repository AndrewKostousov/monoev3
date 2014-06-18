using System;
using MonoBrickFirmware.Movement;

namespace NightChallengeSample
{
	public class Tank
	{
		// NOTE: константы могут зависеть от модели робота :-)
		private const double tankWidthInCm = 17.7;
		private const double tachoToCmCoef = 0.02755;
		private const double spinEfficiencyCoef = 0.940;

		private const double degreesToRadCoef = Math.PI / 180;
		private const double radToDegreesCoef = 180 / Math.PI;

		private readonly Vehicle vehicle;
		private readonly Motor leftMotor;
		private readonly Motor rightMotor;
		private readonly double tankRadius;

		private TankState state;

		public Tank(MotorPort left, MotorPort right)
		{
			vehicle = new Vehicle(left, right);
			vehicle.Off();
			tankRadius = tankWidthInCm / 2;
			leftMotor = new Motor(left);
			rightMotor = new Motor(right);
		}

		public double Angle { get; private set; }
		public Position Position { get; private set; }

		public void Forward(sbyte speed, double cm)
		{
			StartMove(() => vehicle.Forward(speed, CmToSteps(cm), brake: true, waitForCompletion: true), TankState.Forward);
			Stop();
		}

		public void Backward(sbyte speed, double cm)
		{
			StartMove(() => vehicle.Backward(speed, CmToSteps(cm), brake: true, waitForCompletion: true), TankState.Backward);
			Stop();
		}

		public void SpinRight(sbyte speed, double degrees)
		{
			StartMove(() => vehicle.SpinRight(speed, DegreesToSteps(degrees), brake: true, waitForCompletion: true), TankState.SpinRight);
			Stop();
		}

		public void SpinLeft(sbyte speed, double degrees)
		{
			StartMove(() => vehicle.SpinLeft(speed, DegreesToSteps(degrees), brake: true, waitForCompletion: true), TankState.SpinLeft);
			Stop();
		}

		public void StartForward(sbyte speed)
		{
			StartMove(() => vehicle.Forward(speed), TankState.Forward);
		}

		public void StartBackward(sbyte speed)
		{
			StartMove(() => vehicle.Backward(speed), TankState.Backward);
		}

		public void StartSpinRight(sbyte speed)
		{
			StartMove(() => vehicle.SpinRight(speed), TankState.SpinRight);
		}

		public void StartSpinLeft(sbyte speed)
		{
			StartMove(() => vehicle.SpinLeft(speed), TankState.SpinLeft);
		}

		public void Stop()
		{
			vehicle.Off();
			BrickConsole.WriteLine("Last move tacho: left={0} right={1}", leftMotor.GetTachoCount(), rightMotor.GetTachoCount());
			switch(state)
			{
				case TankState.Forward:
					Position += CalcPositionAfterMove();
					break;
				case TankState.Backward:
					Position -= CalcPositionAfterMove();
					break;
				case TankState.SpinRight:
					Angle += CalcAngleAfterSpin();
					break;
				case TankState.SpinLeft:
					Angle -= CalcAngleAfterSpin();
					break;
			}
			state = TankState.None;
		}

		public void Reset()
		{
			vehicle.Off();
			leftMotor.ResetTacho();
			rightMotor.ResetTacho();
			Position = default(Position);
			Angle = 0;
		}

		private void StartMove(Action action, TankState newState)
		{
			ResetBeforeMove();
			state = newState;
			action();
		}

		private uint DegreesToSteps(double degrees)
		{
			return (uint)(degrees * tankRadius * degreesToRadCoef * spinEfficiencyCoef / tachoToCmCoef);
		}

		private static uint CmToSteps(double cm)
		{
			return (uint)(cm / tachoToCmCoef);
		}

		private Position CalcPositionAfterMove()
		{
			var shift = CalcShiftCmAfterMove();
			return new Position {X = shift * Math.Sin(Angle), Y = shift * Math.Cos(Angle)};
		}

		private double CalcAngleAfterSpin()
		{
			return CalcShiftCmAfterMove() * radToDegreesCoef / tankRadius / spinEfficiencyCoef;
		}

		private void ResetBeforeMove()
		{
			if(state != TankState.None)
				Stop();
			leftMotor.ResetTacho();
			rightMotor.ResetTacho();
		}

		private double CalcShiftCmAfterMove()
		{
			var leftCount = Math.Abs(leftMotor.GetTachoCount());
			var rightCount = Math.Abs(rightMotor.GetTachoCount());
			return (leftCount + rightCount) / 2.0 * tachoToCmCoef;
		}

		public override string ToString()
		{
			return string.Format("Position: {0}, Angle={1:0.0}", Position, Angle);
		}
	}
}