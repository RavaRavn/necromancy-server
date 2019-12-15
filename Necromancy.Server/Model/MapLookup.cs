using System.Collections.Generic;
using Arrowgene.Services.Logging;

namespace Necromancy.Server.Model
{
    public class MapLookup
    {
        private readonly Dictionary<int, Map> _maps;

        private readonly object _lock = new object();
        private readonly ILogger _logger;

        public MapLookup()
        {
            _logger = LogProvider.Logger(this);
            _maps = new Dictionary<int, Map>();
        }

        /// <summary>
        /// Returns all maps from the lookup.
        /// </summary>
        public List<Map> GetAll()
        {
            lock (_lock)
            {
                return new List<Map>(_maps.Values);
            }
        }

        /// <summary>
        /// Returns a map by its id.
        /// </summary>
        public Map Get(int mapId)
        {
            lock (_lock)
            {
                if (!_maps.ContainsKey(mapId))
                {
                    _logger.Error($"MapId: {mapId} not found");
                    return null;
                }

                return _maps[mapId];
            }
        }

        /// <summary>
        /// Returns a map by its id.
        /// </summary>
        public bool TryGet(int mapId, out Map map)
        {
            lock (_lock)
            {
                if (!_maps.ContainsKey(mapId))
                {
                    _logger.Error($"MapId: {mapId} not found");
                    map = null;
                    return false;
                }

                map = _maps[mapId];
                return true;
            }
        }

        /// <summary>
        /// Adds a new map to the lookup.
        /// If the mapId already exists no insert will happen.
        /// </summary>
        public void Add(Map map)
        {
            if (map == null)
            {
                return;
            }

            lock (_lock)
            {
                if (_maps.ContainsKey(map.Id))
                {
                    return;
                }

                _maps.Add(map.Id, map);
            }
        }

        /// <summary>
        /// Adds a new map to the lookup.
        /// If the mapId already exists it will be overwritten.
        /// </summary>
        public void AddOverride(Map map)
        {
            if (map == null)
            {
                return;
            }

            lock (_lock)
            {
                _maps.Add(map.Id, map);
            }
        }

        /// <summary>
        /// Removes a map from the lookup
        /// </summary>
        public bool Remove(Map map)
        {
            lock (_lock)
            {
                return _maps.Remove(map.Id);
            }
        }
    }
}
