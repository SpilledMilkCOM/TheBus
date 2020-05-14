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

        private ISerializationUtility _serializationUtility;

        /// <summary>
        /// Once this object is constructed the resource will be created/verified and the client is set up to "listen" for the Topic
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="topicName"></param>
        /// <param name="subscriptionName"></param>
        public Subscription(string connectionString, string topicName, string subscriptionName, bool createIfNeeded, bool cleanupIfNeeded, bool peekOnly, ISerializationUtility serializationUtility)
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

            _serializationUtility = serializationUtility;

            //_subscriptionClient = new SubscriptionClient(new ServiceBusConnectionStringBuilder(ConnectionString), Name);
            _subscriptionClient = new SubscriptionClient(ConnectionString, TopicName, Name);

            RegisterCallback();

            //ProcessMessages();
        }

        private void RegisterCallback()
        {
            var options = new MessageHandlerOptions(MessageError) { AutoComplete = false, MaxConcurrentCalls = 1 };

            _subscriptionClient.RegisterMessageHandler(OnMessageCallback, options);
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
            throw new NotSupportedException($"{nameof(WaitForMessage)} not supported in .Net Standard 2.0");
        }

        [ExcludeFromCodeCoverage]
        public void WaitForProcessedMessage()
        {
        }

        //----==== PRIVATE ====--------------------------------------------------------------------

        /// <summary>
        /// Creates the subscription if it doesn't already exist.
        /// </summary>
        private void Create(bool cleanupIfNeeded)
        {
            // Once this is created, it could be deleted at some point even if THIS object is still around.
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

                    if (!_subscriptionClient.IsClosedOrClosing)
                    {
                        _subscriptionClient.CloseAsync();
                    }
                }

                // Free unmanaged resources (unmanaged objects) and override a finalizer below.

                Delete();

                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }

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

                    message = new Message
                    {
                        Payload = (brokeredMessage.ContentType == null) ? data : (object)_serializationUtility.Deserialize<TType>(data),
                        Metadata = new MessageMetadata
                        {
                            MessageId = brokeredMessage.MessageId,
                            DeadLetterErrorDescription = brokeredMessage.UserProperties["DeadLetterErrorDescription"].ToString(),
                            DeadLetterReason = brokeredMessage.UserProperties["DeadLetterReason"].ToString()
                        },
                        Success = !token.IsCancellationRequested
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

        /// <summary>
        /// Process the existing messages on the queue.
        /// </summary>
        private void ProcessMessages()
        {
            // There is NO PEEK in .NetStandard so there is NO way to get the existing messages.
            // Those will stay on the queue.
        }
    }
}