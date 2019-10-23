using ComponentModelRPC.Client;
using System.Configuration;

namespace TVP.DocumentClient
{
    public class AppDataContext
    {
        private AppDataContext() {
            RemoteClient = new RemoteClient(ConfigurationManager.AppSettings["DestinationAddress"])
            {
                Binder = new ClientTypeNameBinder()
            };
        }

        public static AppDataContext Instance { get; } = new AppDataContext();

        public RemoteClient RemoteClient { get; }
    }
}
