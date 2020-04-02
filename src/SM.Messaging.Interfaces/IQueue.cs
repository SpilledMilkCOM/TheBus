using System;
using System.Collections.Generic;

namespace SM.Messaging.Interfaces
{
	public interface IQueue<TType> : IQueue
	{
		TType Peek();

		List<TType> Peek(int count);

		TType Pop();

		void Put(TType entry);
	}

	public interface IQueue : IDisposable
	{
		int MessageProcessedCount { get; }

		string Name { get; }
	}
}