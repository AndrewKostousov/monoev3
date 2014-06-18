using System;
using System.Threading;
using MonoBrickFirmware.Sensors;
using MonoBrickFirmware.Tools;

namespace NightChallengeSample
{
	public class RemoteControl
	{
		private readonly IRSensorLockWrapper irSensorWrapper;
		private readonly IRChannel channel;
		private readonly Thread thread;
		private readonly QueueThread queue;
		private RemoteButton prev;

		public RemoteControl(IRSensorLockWrapper irSensorWrapper, IRChannel channel)
		{
			this.irSensorWrapper = irSensorWrapper;
			this.channel = channel;
			queue = new QueueThread();
			thread = new Thread(Loop) { IsBackground = true };
			thread.Start();
		}

		public event Action<RemoteButton> ButtonsPressed = btn => { };
		public event Action<RemoteButton> ButtonsReleased = btn => { };

		private void Loop()
		{
			while(true)
			{
				try
				{
					CheckButtons();
				}
				catch(Exception e)
				{
					Console.Error.WriteLine(e);
				}
				Thread.Sleep(10);
			}
		}

		private void CheckButtons()
		{
			var btn = TryGetCommand().ToButtons();
			if(btn == prev)
				return;

			var released = ~btn & prev;
			var pressed = btn & ~prev;
			prev = btn;

			if(released != RemoteButton.None)
			{
				BrickConsole.WriteLine("Remote button released: {0}", released);
				queue.Enqueue(() => ButtonsReleased(released));
			}

			if(pressed != RemoteButton.None)
			{
				BrickConsole.WriteLine("Remote button pressed: {0}", pressed);
				queue.Enqueue(() => ButtonsPressed(pressed));
			}
		}

		private RemoteCommand TryGetCommand()
		{
			var signal = GetSignal();
			return Enum.IsDefined(typeof(RemoteCommand), signal) ? (RemoteCommand)signal : RemoteCommand.None;
		}

		private byte GetSignal()
		{
			return irSensorWrapper.Do(sensor => sensor.ReadRemoteCommand(channel));
		}
	}
}