using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace HttpForwarder.Core.Concurrency
{
    public partial class Sync
    {
        public class SyncGroup : Dictionary<string, Sync> { }

        private static Dictionary<string, SyncGroup> _syncGroups = new Dictionary<string, SyncGroup>();

        private static void AddSync(string uid, Sync sync)
        {
            if(!_syncGroups.ContainsKey(uid))
            {
                _syncGroups[uid] = new SyncGroup();
            }

            _syncGroups[uid][sync.RequestId] = sync;
        }

        public static Sync GetSync(string uid, string requestId)
        {
            if (_syncGroups.ContainsKey(uid) && _syncGroups[uid].ContainsKey(requestId))
            {
                return _syncGroups[uid][requestId];
            }
            else
            {
                return null;
            }
        }

        public static Sync Remove(string uid, string requestId)
        {
            if (_syncGroups.ContainsKey(uid) && _syncGroups[uid].ContainsKey(requestId))
            {
                var sync = _syncGroups[uid][requestId];
                _syncGroups[uid].Remove(requestId);
                return sync;
            }
            else
            {
                return null;
            }
        }
    }
}