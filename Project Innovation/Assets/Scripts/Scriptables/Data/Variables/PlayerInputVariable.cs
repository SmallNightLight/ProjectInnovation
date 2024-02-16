using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [CreateAssetMenu(fileName = "PlayerInputVariable", menuName = "Scriptables/Variables/PlayerInput")]
    public class PlayerInputVariable : Variable<PlayerInput>
    {
    }
}