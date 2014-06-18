using System;
using MonoBrickFirmware.Sensors;

namespace NightChallengeSample
{
	public class IRSensorLockWrapper
	{
		public IRSensorLockWrapper(EV3IRSensor sensor)
		{
			this.sensor = sensor;
		}

		public T Do<T>(Func<EV3IRSensor, T> func)
		{
			lock(sensor)
				return func(sensor);
		}

		private readonly EV3IRSensor sensor;
	}
}