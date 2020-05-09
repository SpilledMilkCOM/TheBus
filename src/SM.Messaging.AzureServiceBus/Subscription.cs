using Microsoft.Azure.ServiceBus;
using SM.Serialization;
using SM.Messaging.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using BrokeredMessage = Microsoft.Azure.ServiceBus.Message;

using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SM.Messaging.AzureServiceBus
{
	/// <summary>
	/// A wrapper around the Azure Subscription of a Subscription.
	/// This class relies on an Azure resource in the cloud which COULD be deleted by an external force.
	/// There will be errors if this resource is deleted.
	/// 
	/// NOTE: Try not to BLEED out the Azure implementation outside of this.
	/// 
	/// </summary>
	/// <typeparam name="TType"></typeparam>
	public class Subscription<TType> : ISubscription
	{
		private const int DELETE_ON_IDLE_TIME_DEFAULT = 600000;     // 5 minutes
		private const int LOCK_DURATION_DEFAULT = 10000;            // 10 seconds
		private const int MESSAGE_TIME_TO_LIVE_DEFAULT = 60000;     // 1 minute
		private const int WAIT_DELAY_DEFAULT = 100;

		private Action<IMessage> _callback;
		private bool _disposedValue = false; // To detect redundant calls
		private bool _peekOnly = false;
		private readonly SubscriptionClient _subscriptionClient;

#if NETSTANDARD2_0
		private ISerializationUtility _serializationUtility;
#else
		private bool _cleanupSubscription = false;
#endif

		/// <summary>
		/// Once this object is constructed the resource will be created/verified and the client is set up to "listen" for the Topic
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="topicName"></param>
		/// <param name="subscriptionName"></param>
#if NETSTANDARD2_0
		public Subscription(string connectionString, string topicName, string subscriptionName, bool createIfNeeded, bool cleanupIfNeeded, bool peekOnly, ISerializationUtility serializationUtility)
#else
		public Subscription(string connectionString, string topicName, string subscriptionName, bool createIfNeeded, bool cleanupIfNeeded, bool peekOnly)
#endif
		{
			ConnectionString = connectionString;
			Name = subscriptionName;
			TopicName = topicName;

			// TODO: Possibly inject configuration with a class.

			DeleteOnIdleTime = DELETE_ON_IDLE_TIME_DEFAULT;
			LockDuration = LOCK_DURATION_DEFAULT;
			MessageTimeToLive = MESSAGE_TIME_TO_LIVE_DEFAULT;
			WaitDelay = WAIT_DELAY_DEFAULT;

			_peekOnly = peekOnly;

			if (createIfNeeded)
			{
				Create(cleanupIfNeeded);
			}

#if NETSTANDARD2_0
			_serializationUtility = serializationUtility;

			//_subscriptionClient = new SubscriptionClient(new ServiceBusConnectionStringBuilder(ConnectionString), Name);
			_subscriptionClient = new SubscriptionClient(ConnectionString, TopicName, Name);
#else
			_subscriptionClient = SubscriptionClient.CreateFromConnectionString(ConnectionString, TopicName, Name, ReceiveMode.PeekLock);
#endif
			RegisterCallback();

			//ProcessMessages();
		}

		private void RegisterCallback()
		{
#if NETSTANDARD2_0
			var options = new MessageHandlerOptions(MessageError) { AutoComplete = false, MaxConcurrentCalls = 1 };

			_subscriptionClient.RegisterMessageHandler(OnMessageCallback, options);
#else
			_subscriptionClient.OnMessage(OnMessageCallback);
#endif
		}

		/// <summary>
		/// A finalizer is needed because this class could have created an unmanaged resource on the bus.
		/// </summary>
		~Subscription()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(false);
		}

		/// <summary>
		/// The client callback when a message is received.
		/// </summary>
		public Action<IMessage> Callback
		{
			get
			{
				return _callback;
			}
			set
			{
				_callback = value;

				if (_callback != null)
				{
					ProcessMessages();
				}
			}
		}

		public string ConnectionString { get; private set; }

		/// <summary>
		/// The lock duration in milliseconds (default 10 seconds)
		/// </summary>
		public int LockDuration { get; set; }

		/// <summary>
		/// The idle time before deletion milliseconds (default 10 seconds)
		/// This is based on idle after connection versus notification.
		/// </summary>
		public int DeleteOnIdleTime { get; set; }

		/// <summary>
		/// The message time to live in milliseconds (default 10 seconds)
		/// </summary>
		public int MessageTimeToLive { get; set; }

		/// <summary>
		/// The name of the Subscription
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// The name of the Topic to which the subscription is connected.
		/// </summary>
		public string TopicName { get; private set; }

		/// <summary>
		/// The delay between Peeks in WaitForMessage()
		/// </summary>
		public int WaitDelay { get; set; }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);

			// Call Garbage Collector here since UNmanaged resources are being freed.
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// This method can be overriden.  If it is NOT, then the Callback method must be defined.
		/// </summary>
		/// <param name="message"></param>
		public virtual void OnMessageCallback(IMessage message)
		{
			if (Callback == null)
			{
				throw new Exception("The Callback action was not defined.");
			}
			else
			{
				Callback(message);
			}
		}

		public IMessage WaitForMessage(int totalDelay)
		{
#if NETSTANDARD2_0
			throw new NotSupportedException($"{nameof(WaitForMessage)} not supported in .Net Standard 2.0");
#else
			IMessage result = null;

			// Always delay, because we are waiting for a callback.
			// TODO: Possibly break out of this wait loop if the callback message is called.

			while (totalDelay > 0)
			{
				// Look for the next message if one was not found.
				var message = (result == null) ? _subscriptionClient.Peek() : null;

				if (message == null)
				{
					Thread.Sleep(WaitDelay);
					totalDelay -= WaitDelay;
				}
				else
				{
					result = new Message() { Payload = message.GetBody<TType>() };
				}
			}

			return result;
#endif
		}

		[ExcludeFromCodeCoverage]
		public void WaitForProcessedMessage()
		{
#if !NETSTANDARD2_0
			var baseMessage = _subscriptionClient.Peek();

			while (baseMessage != null)
			{
				var message = _subscriptionClient.Peek();

				// If the new message doesn't exist or a new message is encountered, then break out of the loop.

				if (message == null || message.MessageId != baseMessage.MessageId)
				{
					baseMessage = null;
				}
				else
				{
					Thread.Sleep(WaitDelay);
				}
			}
#endif
		}

		//----==== PRIVATE ====--------------------------------------------------------------------

		/// <summary>
		/// Creates the subscription if it doesn't already exist.
		/// </summary>
		private void Create(bool cleanupIfNeeded)
		{
			// Once this is created, it could be deleted at some point even if THIS object is still around.

#if !NETSTANDARD2_0
			var namespaceManager = NamespaceManager.CreateFromConnectionString(ConnectionString);

			if (!namespaceManager.SubscriptionExists(TopicName, Name))
			{
				var descriptor = new SubscriptionDescription(TopicName, Name)
				{
					MaxDeliveryCount = 1,
					AutoDeleteOnIdle = TimeSpan.FromMilliseconds(DeleteOnIdleTime),
					DefaultMessageTimeToLive = TimeSpan.FromMilliseconds(MessageTimeToLive),
					LockDuration = TimeSpan.FromMilliseconds(LockDuration)
				};

				namespaceManager.CreateSubscription(descriptor, new RuleDescription());

				_cleanupSubscription = cleanupIfNeeded;
			}
#endif
		}

		private void Delete()
		{
#if !NETSTANDARD2_0
			var namespaceManager = NamespaceManager.CreateFromConnectionString(ConnectionString);

			if (_cleanupSubscription && namespaceManager.SubscriptionExists(TopicName, Name))
			{
				// Only delete the subscription if this class created it.

				namespaceManager.DeleteSubscription(TopicName, Name);
			}
#endif
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).

#if NETSTANDARD2_0
					if (!_subscriptionClient.IsClosedOrClosing)
					{
						_subscriptionClient.CloseAsync();
					}
#endif
				}

				// Free unmanaged resources (unmanaged objects) and override a finalizer below.

				Delete();

				// TODO: set large fields to null.

				_disposedValue = true;
			}
		}

#if NETSTANDARD2_0
		/// <summary>
		/// This message is registered on the subscription client and is called in a separate thread.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		private async Task OnMessageCallback(BrokeredMessage brokeredMessage, CancellationToken token)
		{
			if (brokeredMessage != null)
			{
				IMessage message = null;

				try
				{
					var data = Encoding.UTF8.GetString(brokeredMessage.Body);

					message = new Message { Payload = (brokeredMessage.ContentType == null) ? data : (object)_serializationUtility.Deserialize<TType>(data)
							, MessageId = brokeredMessage.MessageId
							, Success = !token.IsCancellationRequested
							, DeadLetterErrorDescription = brokeredMessage.UserProperties["DeadLetterErrorDescription"].ToString()
							, DeadLetterReason = brokeredMessage.UserProperties["DeadLetterReason"].ToString()
					};

					OnMessageCallback(message);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}

				// If this subscription is peek only then don't give up any of the peek locks so the next message will be processed.
				
				if (!_peekOnly)
				{
					if (message?.Success == true)
					{
						await _subscriptionClient.CompleteAsync(brokeredMessage.SystemProperties.LockToken);
					}
					else
					{
						await _subscriptionClient.AbandonAsync(brokeredMessage.SystemProperties.LockToken);
					}
				}

				// TODO: Add dead letter?
			}
		}

		private Task MessageError(ExceptionReceivedEventArgs arg)
		{
			return new Task(() => Console.WriteLine(arg.Exception.Message));
		}
#else
		/// <summary>
		/// The method called when a subscription is triggered.
		/// </summary>
		/// <param name="message"></param>

		private void OnMessageCallback(BrokeredMessage brokeredMessage)
		{
			if (brokeredMessage != null)
			{
				IMessage message = new Message() { Payload = brokeredMessage.GetBody<TType>() };

				// TODO: Investigate exceptions thrown here.
				// TODO: Call based on IsAsynchronous

				OnMessageCallback(message);

				if (message.Success)
				{
					_subscriptionClient.Complete(brokeredMessage.LockToken);
				}
				else
				{
					_subscriptionClient.Abandon(brokeredMessage.LockToken);
				}
			}
		}
#endif

		/// <summary>
		/// Process the existing messages on the queue.
		/// </summary>
		private void ProcessMessages()
		{
			// There is NO PEEK in .NetStandard so there is NO way to get the existing messages.
			// Those will stay on the queue.

#if !NETSTANDARD2_0
			while (_subscriptionClient.Peek() != null)
			{
				var message = _subscriptionClient.Receive();

				OnMessageCallback(message);
			}
#endif
		}
	}
}