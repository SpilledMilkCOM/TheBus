using System.Collections.Generic;

namespace SM.Messaging.Interfaces
{
	public interface IServiceBusConfiguration
	{
		IEnumerable<IQueueConfiguration> QueueConfigurations { get; }

		IEnumerable<ITopicConfiguration> TopicConfigurations { get; }
	}
}