using System;

namespace SM.Messaging.Interfaces
{
	/// <summary>
	/// The interface for queue configuration which will be fairly similar across platforms.
	/// </summary>
	public interface IQueueConfiguration
	{
		/// <summary>
		/// The idle time before the resource is deleted. (null means use default)
		/// </summary>
		TimeSpan? AutoDeleteOnIdle { get; set; }

		/// <summary>
		/// The time to live (TTL) for default messages. (null means use default)
		/// </summary>
		TimeSpan? DefaultMessageTimeToLive { get; set; }

		bool? EnableBatchedOperations { get; set; }

		bool? EnforceMessageOrdering { get; set; }
	}
}