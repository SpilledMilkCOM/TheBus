namespace SM.Messaging.Interfaces
{
	public interface IMessage
	{
		IMessageMetadata Metadata { get; set; }

		/// <summary>
		/// The payload of the message.
		/// </summary>
		object Payload { get; set; }

		bool? Success { get; set; }
	}
}