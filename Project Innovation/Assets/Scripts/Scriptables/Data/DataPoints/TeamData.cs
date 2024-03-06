using Photon.Pun;
using ScriptableArchitecture.Core;
using System.Collections.Generic;
using System.Linq;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public class TeamData : IDataPoint, IPunObservable
    {
        public List<string> Players = new();
        public int Points;

        public void ResetPoints() => Points = 0;

        public void AddPoint() => Points++;

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(Players.ToArray());
            }
            else
            {
                Players = ((string[])stream.ReceiveNext()).ToList();
            }
        }
    }
}