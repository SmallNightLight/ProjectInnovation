using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [AddComponentMenu("GameEvent Listeners/WeaponData Event Listener")]
    public class WeaponDataGameEventListener : GameEventListenerBase<WeaponData>
    {
        [SerializeField] private WeaponDataVariable _event;

        public override IGameEvent<WeaponData> GetGameEventT() => _event;
    }
}