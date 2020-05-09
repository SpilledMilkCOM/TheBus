using SM.Messaging.Interfaces;
using System.Text;

namespace SM.Messaging.AzureServiceBus
{
	public class Message : IMessage
	{
		public Message()
		{
			Metadata.MessageEncoding = Encoding.UTF8;
		}

		public IMessageMetadata Metadata { get; set; }

		public object Payload { get; set; }

		//public bool Processed { get; set; }

		public bool? Success { get; set; }
	}
}