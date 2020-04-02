using System;
using System.Threading.Tasks;

namespace SM.Messaging.Interfaces
{
	/// <summary>
	/// The generic version of the topic interface.
	/// </summary>
	/// <typeparam name="TType">The type of the topic to publish</typeparam>
	public interface ITopic<TType> : ITopic
	{
		void Publish(TType entry, IMessageMetadata messageMeta = null);

		Task PublishAsync(TType entry, IMessageMetadata messageMeta = null);
	}

	/// <summary>
	/// The non generic version of the topic interface (handy for lists etc.)
	/// </summary>
	public interface ITopic : IDisposable
	{
		string Name { get; }

		void AddSubscriber(ISubscription subscription);

		/// <summary>
		/// Subscribe to the topic.
		/// </summary>
		/// <param name="subscriptionName"></param>
		/// <returns></returns>
		ISubscription Subscribe(string subscriptionName, bool peekOnly = false);
	}
}