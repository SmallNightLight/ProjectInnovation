using ScriptableArchitecture.Core;
using System.Collections.Generic;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public class TeamData : IDataPoint
    {
        public List<string> Players = new();
    }
}