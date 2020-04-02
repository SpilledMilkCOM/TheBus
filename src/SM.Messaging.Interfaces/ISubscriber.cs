using System;

namespace SM.Messaging.Interfaces
{
	public interface ISubscriber : IDisposable
	{
		int WaitDelay { get; set; }

		/// <summary>
		/// Wait for a SINGLE message.
		/// </summary>
		/// <param name="sleepInterval"></param>
		/// <returns></returns>
		IMessage WaitForMessage();

		/// <summary>
		/// Wait for the specified number of messages.
		/// </summary>
		/// <param name="messageCount"></param>
		/// <returns></returns>
		void WaitForMessages(int messageCount);
	}
}