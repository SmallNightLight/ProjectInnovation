using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [AddComponentMenu("GameEvent Listeners/CharacterCollection Event Listener")]
    public class CharacterCollectionGameEventListener : GameEventListenerBase<CharacterCollection>
    {
        [SerializeField] private CharacterCollectionVariable _event;

        public override IGameEvent<CharacterCollection> GetGameEventT() => _event;
    }
}