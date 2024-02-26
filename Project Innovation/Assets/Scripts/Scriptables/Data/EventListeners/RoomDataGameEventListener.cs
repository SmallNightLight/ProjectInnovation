using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [AddComponentMenu("GameEvent Listeners/RoomData Event Listener")]
    public class RoomDataGameEventListener : GameEventListenerBase<RoomData>
    {
        [SerializeField] private RoomDataVariable _event;

        public override IGameEvent<RoomData> GetGameEventT() => _event;
    }
}