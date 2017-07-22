using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManagerRegistrations : NetworkBehaviour {

    // Use this for initialization
    void Start ()
    {
    }

    /*
    public static void OnChangeTransformRate(NetworkMessage message)
    {
        var body = message.ReadMessage<SetTransformRate>();
        var player = Players[message.conn.connectionId];
        Debug.Log("Set sendInterval to " + body.Rate + " on " + player.GetHashCode());
        player.GetComponent<NetworkTransform>().sendInterval = body.Rate;
    }

    public static void OnChangeTransformSyncMode(NetworkMessage message)
    {
        var body = message.ReadMessage<SetTransformSyncMode>();
        var player = Players[message.conn.connectionId];
        Debug.Log("Set SyncMode to " + body.Mode + " on " + player.GetHashCode());
        player.GetComponent<NetworkTransform>().transformSyncMode = body.Mode;
    }

    public class SetTransformRate : MessageBase
    {
        public static short MessageId = 60;
        public int Rate;
    }

    public class SetTransformSyncMode : MessageBase
    {
        public static short MessageId = 61;
        public NetworkTransform.TransformSyncMode Mode;
    }*/

}
