using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [AddComponentMenu("GameEvent Listeners/PlayerInput Event Listener")]
    public class PlayerInputGameEventListener : GameEventListenerBase<PlayerInput>
    {
        [SerializeField] private PlayerInputVariable _event;

        public override IGameEvent<PlayerInput> GetGameEventT() => _event;
    }
}