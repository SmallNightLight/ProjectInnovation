using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [AddComponentMenu("GameEvent Listeners/PlayerCharacters Event Listener")]
    public class PlayerCharactersGameEventListener : GameEventListenerBase<PlayerCharacters>
    {
        [SerializeField] private PlayerCharactersVariable _event;

        public override IGameEvent<PlayerCharacters> GetGameEventT() => _event;
    }
}