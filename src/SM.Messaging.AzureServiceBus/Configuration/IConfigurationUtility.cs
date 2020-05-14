using System;

namespace SM.Messaging.AzureServiceBus.Configuration
{
	public interface IConfigurationUtility
	{
		bool? GetBoolean(string boolean);

		TimeSpan? GetTimeSpan(string milliseconds);
	}
}