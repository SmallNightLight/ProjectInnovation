using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [AddComponentMenu("GameEvent Listeners/WeaponPartData Event Listener")]
    public class WeaponPartDataGameEventListener : GameEventListenerBase<WeaponPartData>
    {
        [SerializeField] private WeaponPartDataVariable _event;

        public override IGameEvent<WeaponPartData> GetGameEventT() => _event;
    }
}