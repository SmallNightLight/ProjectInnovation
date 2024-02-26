using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [AddComponentMenu("GameEvent Listeners/TeamData Event Listener")]
    public class TeamDataGameEventListener : GameEventListenerBase<TeamData>
    {
        [SerializeField] private TeamDataVariable _event;

        public override IGameEvent<TeamData> GetGameEventT() => _event;
    }
}