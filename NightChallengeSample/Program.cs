using System;
using System.Threading;
using MonoBrickFirmware.Movement;
using MonoBrickFirmware.Sensors;
using MonoBrickFirmware.Sound;
using MonoBrickFirmware.UserInput;

namespace NightChallengeSample
{
	// NOTE: TargetFramework = v4.0
	public static class Program
	{
		private const sbyte speed = 50;

		private static void Main()
		{
			BrickConsole.WriteLine("Start!");
			var speaker = new Speaker(10);

			speaker.Beep();

			var tank = new Tank(MotorPort.OutD, MotorPort.OutA);
			var irSensorWrapper = new IRSensorLockWrapper(new EV3IRSensor(SensorPort.In2));
			var colorSensor = new EV3ColorSensor(SensorPort.In1, ColorMode.Color);
			var touchSensor = new EV3TouchSensor(SensorPort.In3);
			
			var buttons = new ButtonEvents();
			buttons.EscapePressed += () =>
			{
				tank.Stop();
				BrickConsole.WriteLine("End!");
				speaker.Buzz();
				Environment.Exit(0);
			};

			var remote = new RemoteControl(irSensorWrapper, IRChannel.One);
			remote.ButtonsReleased += btn =>
			{
				tank.Stop();
				BrickConsole.WriteLine("TankState: {0}", tank);
			};

			remote.ButtonsPressed += btn =>
			{
				switch(btn)
				{
					case RemoteButton.LeftUp:
						tank.StartForward(speed);
						break;
					case RemoteButton.LeftDown:
						tank.StartBackward(speed);
						break;
					case RemoteButton.RightUp:
						tank.StartSpinLeft(speed);
						break;
					case RemoteButton.RightDown:
						tank.StartSpinRight(speed);
						break;
					case RemoteButton.LeftUp | RemoteButton.LeftDown:
						BrickConsole.WriteLine("Color: {0}", colorSensor.ReadColor());
						break;
					case RemoteButton.RightUp | RemoteButton.RightDown:
						BrickConsole.WriteLine("TouchSensor.IsPressed: {0}", touchSensor.IsPressed());
						break;
					case RemoteButton.LeftUp | RemoteButton.RightUp:
						BrickConsole.WriteLine("Distance: {0} cm", irSensorWrapper.Do(x => x.ReadDistance()));
						break;
					case RemoteButton.Beacon:
						BrickConsole.WriteLine("Color: {0}", colorSensor.ReadColor());
						BrickConsole.WriteLine("Distance: {0} cm", irSensorWrapper.Do(x => x.ReadDistance()));
						BrickConsole.WriteLine("TouchSensor.IsPressed: {0}", touchSensor.IsPressed());
						break;
				}
			};

			Thread.Sleep(Timeout.Infinite);
		}
	}
}