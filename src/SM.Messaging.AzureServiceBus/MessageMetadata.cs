using System.Text;
using SM.Messaging.Interfaces;

namespace SM.Messaging.AzureServiceBus
{
	public class MessageMetadata : IMessageMetadata
	{
		public MessageMetadata()
		{
			MessageEncoding = Encoding.UTF8;
		}

		public string CorrelationId { get; set; }

		public string DeadLetterErrorDescription { get; set; }

		public string DeadLetterReason { get; set; }

		public Encoding MessageEncoding { get; set; }

		public string MessageId { get; set; }

		/// <summary>
		/// The part of the message ex: "1/1", "1/2", "2/2", "4/9".
		/// </summary>
		public string MessagePart { get; set; }

		/// <summary>
		/// Used for message filtering in Azure Service Bus.  This type is stored in the UserProperties using "messageType" as the key.
		/// </summary>
		public string MessageType { get; set; }
	}
}