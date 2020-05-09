using Microsoft.Azure.ServiceBus;
using SM.Messaging.Interfaces;
using SM.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using BrokeredMessage = Microsoft.Azure.ServiceBus.Message;

namespace SM.Messaging.AzureServiceBus
{
	public class Queue<TType> : IQueue<TType>
	{
		private bool _disposedValue = false; // To detect redundant calls
		private readonly QueueClient _queueClient;

		private ISerializationUtility _serializationUtility;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="connectionString">The full connection string to the Azure Service Bus namespace.</param>
		/// <param name="queueName">The name of the queue.</param>
		/// <param name="createIfNeeded">Create the queue resource within the namespace.</param>
		public Queue(string connectionString, string queueName, bool createIfNeeded, ISerializationUtility serializationUtility)
		{
			ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString), "The Azure connection string cannot be null");
			Name = queueName ?? throw new ArgumentNullException(nameof(queueName), "The Azure queue name cannot be null");

			if (createIfNeeded)
			{
				Create();
			}

			_queueClient = new QueueClient(ConnectionString, Name, ReceiveMode.PeekLock, null);

			IsAsynchronous = true;

			_serializationUtility = serializationUtility;
		}

		/// <summary>
		/// A finalizer is needed because this class could have created an unmanaged resource on the bus.
		/// </summary>
		~Queue()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(false);
		}

		public string ConnectionString { get; private set; }

		public bool IsAsynchronous { get; private set; }

		public int MessageProcessedCount { get; private set; }

		/// <summary>
		/// The name of the Queue
		/// </summary>
		public string Name { get; private set; }

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);

			// Call Garbage Collector here since UNmanaged resources are being freed.
			GC.SuppressFinalize(this);
		}

		public TType Peek()
		{
			throw new NotSupportedException($"{nameof(Peek)} not supported in .Net Standard 2.0");
		}

		public List<TType> Peek(int count = 1)
		{
			var result = new List<TType>();
			var options = new MessageHandlerOptions (ExceptionReceivedHandler) { AutoComplete = false, MaxConcurrentCalls = 1 };

			_queueClient.RegisterMessageHandler(ProcessMessagesAsync, options);

			while (MessageProcessedCount < count)
			{
				Thread.Sleep(1000);
			}

			return result;
		}

		public TType Pop()
		{
			throw new NotSupportedException($"{nameof(Pop)} not supported in .Net Standard 2.0");
		}

		public void Put(TType entry)
		{
			var message = new BrokeredMessage(Encoding.UTF8.GetBytes(_serializationUtility.Serialize(entry)));

			if (IsAsynchronous)
			{
				// "Fire and forget" the message.

				_queueClient.SendAsync(message);

			}
		}

		//----==== PRIVATE ====--------------------------------------------------------------------

		/// <summary>
		/// Creates the subscription if it doesn't already exist.
		/// </summary>
		private void Create()
		{
			// Once this is created, it could be deleted at some point even if THIS object is still around.

			// Currently the support for creation is in the, but Microsoft is supposedly bringing back the old interface.
		}

		private void Delete()
		{
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
				}

				// Free unmanaged resources (unmanaged objects) and override a finalizer below.

				Delete();

				// TODO: set large fields to null.

				_disposedValue = true;
			}
		}

		// Use this handler to examine the exceptions received on the message pump.
		private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
		{
			Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
			var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
			Console.WriteLine("Exception context for troubleshooting:");
			Console.WriteLine($"- Endpoint: {context.Endpoint}");
			Console.WriteLine($"- Entity Path: {context.EntityPath}");
			Console.WriteLine($"- Executing Action: {context.Action}");

			return Task.CompletedTask;
		}

		private async Task ProcessMessagesAsync(BrokeredMessage message, CancellationToken token)
		{
			//// Process the message.

			Console.WriteLine($"{++MessageProcessedCount}|{message.MessageId}|{message.UserProperties["DeadLetterReason"]}|{message.UserProperties["DeadLetterErrorDescription"]}|");

			// Complete the message so that it is not received again.
			// This can be done only if the queue Client is created in ReceiveMode.PeekLock mode (which is the default).
			await _queueClient.CompleteAsync(message.SystemProperties.LockToken);

			// Note: Use the cancellationToken passed as necessary to determine if the queueClient has already been closed.
			// If queueClient has already been closed, you can choose to not call CompleteAsync() or AbandonAsync() etc.
			// to avoid unnecessary exceptions.
		}
	}
}