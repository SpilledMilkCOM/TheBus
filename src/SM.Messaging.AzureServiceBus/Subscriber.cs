using SM.Messaging.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SM.Messaging.AzureServiceBus
{
	public class Subscriber : ISubscriber
	{
		private List<IMessage> _messages;
		private ISubscription _subscription;

		/// <summary>
		/// This subscriber will change the subscription's Callback.
		/// </summary>
		/// <param name="subscription"></param>
		public Subscriber(ISubscription subscription)
		{
			_subscription = subscription;
			_messages = new List<IMessage>();

			Callback = _subscription.Callback;
			_subscription.Callback = ProcessMessage;

			WaitDelay = 100;
		}

		public Action<IMessage> Callback { get; private set; }

		public int WaitDelay { get; set; }

		/// <summary>
		/// A list of messages retrieved by the subscription.
		/// NOTE: Returns a copy of the internal messages (thread safe).
		/// </summary>
		public List<IMessage> Messages
		{
			get
			{
				lock(_messages)
				{
					return new List<IMessage>(_messages);
				}
			}
		}

		/// <summary>
		/// Waits for a single message given a sleep interval.
		/// </summary>
		/// <param name="sleepInterval"></param>
		/// <returns>Number of milliseconds to sleep for each delay.</returns>
		public IMessage WaitForMessage()
		{
			// The notifications will happen on a different thread.

			do
			{
				Thread.Sleep(WaitDelay);

			} while (Messages.Count == 0);

			return Messages.First();
		}

		public void WaitForMessages(int messageCount)
		{
			// The notifications will happen on a different thread.

			while (MessageCount < messageCount)
			{
				Thread.Sleep(WaitDelay);
			}
		}

		//----==== PRIVATE ====---------------------------------------------------------------------------------

		private int MessageCount
		{
			get
			{
				lock(_messages)
				{
					return _messages.Count;
				}
			}
		}

		private void ProcessMessage(IMessage message)
		{
			Callback?.Invoke(message);

			lock(_messages)
			{
				_messages.Add(message);
			}
		}

		#region IDisposable Support

		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).

					_subscription.Dispose();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~Subscriber() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}

		#endregion
	}
}