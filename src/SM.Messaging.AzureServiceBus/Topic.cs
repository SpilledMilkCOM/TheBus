using Microsoft.Azure.Management.ServiceBus;
using Microsoft.Azure.ServiceBus;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using SM.Messaging.AzureServiceBus.Configuration;
using SM.Messaging.Interfaces;
using SM.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;

using BrokeredMessage = Microsoft.Azure.ServiceBus.Message;

namespace SM.Messaging.AzureServiceBus
{
	/// <summary>
	/// A wrapper around the Azure Service Bus Topic.
	/// NOTE: Try not to BLEED out the Azure implementation outside of this.
	/// </summary>
	/// <typeparam name="TType"></typeparam>
	public class Topic<TType> : ITopic<TType>
	{
		private bool _disposedValue = false; // To detect redundant calls
		private readonly TopicClient _topicClient;
		private ISerializationUtility _serializationUtility;

		/// <summary>
		/// NOTE: If creating the topic within ASB there is currently no way to configure the Topic using this class.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="topicName"></param>
		/// <param name="createIfNeeded">Provision the Topic within ASB if needed.</param>
		public Topic(ITopicConfiguration configuration, ISerializationUtility serializationUtility)
		{
			var config = configuration as TopicConfiguration;

			if (config == null)
			{
				throw new ArgumentOutOfRangeException($"The type {nameof(TopicConfiguration)} is expected");
			}

			ConnectionString = config.ConnectionString;
			Name = config.Name;

			if (config.CreateIfNeeded)
			{
				Create(config);
			}

			//var builder = new ServiceBusConnectionStringBuilder(ConnectionString);
			//_topicClient = new TopicClient(builder);
			_topicClient = new TopicClient(ConnectionString, Name);

			_serializationUtility = serializationUtility;
		}

		~Topic()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(false);
		}

		public string ConnectionString { get; private set; }

		/// <summary>
		/// The name of the Topic
		/// </summary>
		public string Name { get; private set; }

		[ExcludeFromCodeCoverage]
		public void AddSubscriber(ISubscription subscription)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);

			// TODO: uncomment the following line if the finalizer is overridden above.
			GC.SuppressFinalize(this);
		}

		public void Publish(TType entry, IMessageMetadata messageMeta = null)
		{
			throw new NotImplementedException();
		}

		public async Task PublishAsync(TType entry, IMessageMetadata messageMeta = null)
		{
			string messageData = (typeof(TType) == typeof(string)) ? entry as string : _serializationUtility.Serialize(entry);

			var message = new BrokeredMessage(messageMeta.MessageEncoding.GetBytes(messageData));

			if (messageMeta != null && !string.IsNullOrEmpty(messageMeta.CorrelationId))
			{
				message.CorrelationId = messageMeta.CorrelationId;
			}

			if (messageMeta != null && !string.IsNullOrEmpty(messageMeta.MessageId))
			{
				message.MessageId = messageMeta.MessageId;
			}

			if (messageMeta != null && !string.IsNullOrEmpty(messageMeta.MessagePart))
			{
				message.UserProperties.Add("messagePart", messageMeta.MessagePart);
			}

			if (messageMeta != null && !string.IsNullOrEmpty(messageMeta.MessageType))
			{
				message.UserProperties.Add("messageType", messageMeta.MessageType);
			}

			await _topicClient.SendAsync(message);
		}

		public ISubscription Subscribe(string subscriptionName, bool peekOnly = false)
		{
			// TODO: Possibly add this to a list of Subscriptions.

			return new Subscription<TType>(ConnectionString, Name, subscriptionName, true, true, peekOnly, _serializationUtility);
		}

		//----==== PRIVATE ====--------------------------------------------------------------------

		/// <summary>
		/// Creates the topic if it doesn't already exist.
		/// NOTE: There is no need to delete this resource, because other services may be using this topic.  The "auto delete" will take care of that.
		/// </summary>
		private async void Create(ITopicConfiguration config)
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

					_topicClient.CloseAsync();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				Delete();

				_disposedValue = true;
			}
		}
	}
}