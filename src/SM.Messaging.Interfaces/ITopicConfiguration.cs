using System;
using System.Collections.Generic;

namespace SM.Messaging.Interfaces
{
	/// <summary>
	/// The interface for topic configuration which will be fairly similar across platforms.
	/// </summary>
	public interface ITopicConfiguration
	{
		/// <summary>
		/// The idle time before the resource is deleted. (null means use default)
		/// </summary>
		TimeSpan? AutoDeleteOnIdle { get; set; }

		string ClientId { get; set; }

		string ClientSecret { get; set; }

		/// <summary>
		/// The time to live (TTL) for default messages. (null means use default)
		/// </summary>
		TimeSpan? DefaultMessageTimeToLive { get; set; }

		bool? EnableBatchedOperations { get; set; }

		bool? EnforceMessageOrdering { get; set; }

		string Name { get; set; }

		string NamespaceName { get; set; }

		string ResourceGroupName { get; set; }

		IEnumerable<ISubscriptionConfiguration> SubscriptionConfigurations { get; }

		string SubscriptionId { get; set; }

		string TenantId { get; set; }
	}
}