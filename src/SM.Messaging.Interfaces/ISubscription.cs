using System;

namespace SM.Messaging.Interfaces
{
	public interface ISubscription : IDisposable
	{
		Action<IMessage> Callback { get; set; }

		string ConnectionString { get; }

		string Name { get; }

		string TopicName { get; }

		// TODO: Remove overridable method.
		void OnMessageCallback(IMessage message);

		// TODO: Move this wait API to a Subscriber.
		IMessage WaitForMessage(int totalDelay);

		void WaitForProcessedMessage();
	}
}