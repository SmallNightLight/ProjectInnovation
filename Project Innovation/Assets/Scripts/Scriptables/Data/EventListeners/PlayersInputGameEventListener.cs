using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [AddComponentMenu("GameEvent Listeners/PlayersInput Event Listener")]
    public class PlayersInputGameEventListener : GameEventListenerBase<PlayersInput>
    {
        [SerializeField] private PlayersInputVariable _event;

        public override IGameEvent<PlayersInput> GetGameEventT() => _event;
    }
}