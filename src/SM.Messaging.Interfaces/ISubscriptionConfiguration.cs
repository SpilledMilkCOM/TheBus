using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Messaging.Interfaces
{
	/// <summary>
	/// The interface for topic configuration which will be fairly similar across platforms.
	/// </summary>
	public interface ISubscriptionConfiguration
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

		/// <summary>
		/// The time of the message lock upon retrieval (null means use default)
		/// </summary>
		TimeSpan? LockDuration { get; set; }
	}
}