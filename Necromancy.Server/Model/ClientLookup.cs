using System.Collections.Generic;

namespace Necromancy.Server.Model
{
    public class ClientLookup
    {
        private readonly List<NecClient> _clients;

        private readonly object _lock = new object();

        public ClientLookup()
        {
            _clients = new List<NecClient>();
        }

        /// <summary>
        /// Returns all Clients.
        /// </summary>
        public List<NecClient> GetAll()
        {
            lock (_lock)
            {
                return new List<NecClient>(_clients);
            }
        }

        /// <summary>
        /// Adds a Client.
        /// </summary>
        public void Add(NecClient client)
        {
            if (client == null)
            {
                return;
            }

            lock (_lock)
            {
                _clients.Add(client);
            }
        }

        /// <summary>
        /// Removes the Client from all lists and lookup tables.
        /// </summary>
        public void Remove(NecClient client)
        {
            lock (_lock)
            {
                _clients.Remove(client);
            }
        }
        
        /// <summary>
        /// Returns a Client by Soul name if it exists.
        /// </summary>
        public NecClient GetBySoulName(string soulName)
        {
            List<NecClient> clients = GetAll();
            foreach (NecClient client in clients)
            {
                if (client.Soul.Name == soulName)
                {
                    return client;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns a Client by CharacterName if it exists.
        /// </summary>
        public NecClient GetByCharacterId(int characterId)
        {
            List<NecClient> clients = GetAll();
            foreach (NecClient client in clients)
            {
                if (client.Character.Id == characterId)
                {
                    return client;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns a Client by AccountId if it exists.
        /// </summary>
        public NecClient GetByAccountId(int accountId)
        {
            List<NecClient> clients = GetAll();
            foreach (NecClient client in clients)
            {
                if (client.Account.Id == accountId)
                {
                    return client;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns a Client by AccountId if it exists.
        /// </summary>
        public NecClient GetByCharacterInstanceId(uint isntanceId)
        {
            List<NecClient> clients = GetAll();
            foreach (NecClient client in clients)
            {
                if (client.Character != null && client.Character.InstanceId == isntanceId)
                {
                    return client;
                }
            }

            return null;
        }
    }
}
