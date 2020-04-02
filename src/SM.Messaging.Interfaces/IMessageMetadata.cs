using System.Text;

namespace SM.Messaging.Interfaces
{
	public interface IMessageMetadata
	{
		/// <summary>
		/// An ID used to correlate between many messages (possibly on different queues or topics)
		/// </summary>
		string CorrelationId { get; set; }

		string DeadLetterErrorDescription { get; set; }

		string DeadLetterReason { get; set; }

		Encoding MessageEncoding { get; set; }

		/// <summary>
		/// A unique identifier for the message.
		/// </summary>
		string MessageId { get; set; }

		/// <summary>
		/// The part of the message ex: "1/1", "1/2", "2/2", "4/9".
		/// </summary>
		string MessagePart { get; set; }

		/// <summary>
		/// The type of the message which can be used for filtering.
		/// </summary>
		string MessageType { get; set; }
	}
}