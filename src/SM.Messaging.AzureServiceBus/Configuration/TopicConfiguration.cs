using System;
using System.Collections.Generic;
using SM.Messaging.Interfaces;
using Microsoft.Extensions.Configuration;

namespace SM.Messaging.AzureServiceBus.Configuration
{
	public class TopicConfiguration : ITopicConfiguration
	{
		private const string AZURE_SERVICE_BUS_KEY = "AzureServiceBus";
		private const string CONNECTION_STRING_KEY = "ConnectionString";

		public TopicConfiguration(string topicName, IConfigurationUtility configurationUtility, IConfigurationRoot configuration)
		{
			var busSection = configuration?.GetSection(AZURE_SERVICE_BUS_KEY);

			// If there is no configuration, then no big deal (for now) you still will get a named topic.
			// You're on your own setting the properties.

			if (busSection != null)
			{
				ConnectionString = busSection[CONNECTION_STRING_KEY];
			}

			Name = topicName;
		}

		public TimeSpan? AutoDeleteOnIdle { get; set; }

		public string ClientId { get; set; }

		public string ClientSecret { get; set; }

		/// <summary>
		/// NOT a part of the interface.
		/// </summary>
		public string ConnectionString { get; set; }

		public bool CreateIfNeeded { get; set; }

		public TimeSpan? DefaultMessageTimeToLive { get; set; }

		public bool DeleteIfCreated { get; set; }

		public bool? EnableBatchedOperations { get; set; }

		public bool? EnforceMessageOrdering { get; set; }

		public string Name { get; set; }

		public string NamespaceName { get; set; }

		public string ResourceGroupName { get; set; }

		public IEnumerable<ISubscriptionConfiguration> SubscriptionConfigurations => throw new NotImplementedException();

		public string SubscriptionId { get; set; }

		public string TenantId { get; set; }
	}
}