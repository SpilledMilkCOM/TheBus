using System;

namespace SM.Messaging.AzureServiceBus.Configuration
{
	public class ConfigurationUtility : IConfigurationUtility
	{
		public bool? GetBoolean(string boolean)
		{
			bool? result = null;

			if (!string.IsNullOrEmpty(boolean))
			{
				bool converted = false;

				if (bool.TryParse(boolean, out converted))
				{
					result = converted;
				}
				else
				{
					// See if the value is an int and handle it.

					int boolAsInt = 0;

					if (int.TryParse(boolean, out boolAsInt))
					{
						result = boolAsInt != 0;
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Converts milliseconds to a TimeSpan
		/// </summary>
		/// <param name="milliseconds"></param>
		/// <returns>A TimeSpan or null</returns>
		public TimeSpan? GetTimeSpan(string milliseconds)
		{
			TimeSpan? result = null;

			if (!string.IsNullOrEmpty(milliseconds))
			{
				int converted = 0;

				if (int.TryParse(milliseconds, out converted))
				{
					result = new TimeSpan(0, 0, 0, 0, converted);
				}
			}

			return result;
		}
	}
}