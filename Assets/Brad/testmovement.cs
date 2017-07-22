using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class testmovement : NetworkBehaviour {

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(MyCo());
        AddPlayer(gameObject.GetComponent<NetworkTransform>().netId.Value, gameObject);
    }

    IEnumerator MyCo()
    {
        while (true)
        {
                var msg = string.Format("isLocalPlayer: {4}, user: {0}, lastSyncTime: {1}, sendInterval: {2}, transformSyncMode: {3}",
                    gameObject.GetComponent<NetworkTransform>().netId.Value,
                    gameObject.GetComponent<NetworkTransform>().lastSyncTime,
                    gameObject.GetComponent<NetworkTransform>().sendInterval,
                    gameObject.GetComponent<NetworkTransform>().transformSyncMode,
                    isLocalPlayer);
                Debug.Log(msg);
            yield return new WaitForSeconds(1f);
        }
    }


    private bool driving = false;

    [Command]
    private void CmdAskStartDriving()
    {
        gameObject.transform.parent = GameObject.FindGameObjectWithTag("Ship").transform;
        //gameObject.GetComponent<NetworkTransform>().transformSyncMode = NetworkTransform.TransformSyncMode.SyncNone;
        GameObject.FindGameObjectWithTag("Ship").GetComponent<NetworkTransformChild>().target = gameObject.transform;
        RpcStartDriving(gameObject);
    }

    [ClientRpc]
    private void RpcStartDriving(GameObject gameObject)
    {
        gameObject.transform.parent = GameObject.FindGameObjectWithTag("Ship").transform;
    }

    [Command]
    private void CmdAskStopDriving()
    {
        gameObject.transform.parent = null;
        //gameObject.GetComponent<NetworkTransform>().transformSyncMode = NetworkTransform.TransformSyncMode.SyncTransform;
        GameObject.FindGameObjectWithTag("Ship").GetComponent<NetworkTransformChild>().target = GameObject.FindGameObjectWithTag("Interior").transform;
        RpcStopDriving(gameObject);
    }

    [ClientRpc]
    private void RpcStopDriving(GameObject gameObject)
    {
        gameObject.transform.parent = null;
    }

    private static object Lock = new object();
    public static void AddPlayer(uint connectionId, GameObject gameObject)
    {
        lock (Lock)
        {
            Players.Add(new PlayerId { Id = connectionId, Player = gameObject });
            Players.Sort(PlayerComparison);
        }
    }

    private static List<PlayerId> Players = new List<PlayerId>();

    public class PlayerId
    {
        public uint Id;
        public GameObject Player;
    }

    public static int PlayerComparison(PlayerId left, PlayerId right)
    {
        if (left.Id < right.Id) return -1;
        if (left.Id > right.Id) return 1;
        return 0;
    }

    // Update is called once per frame
    private KeyCode lastCmd;
    void Update ()
    {
        var tmp = KeyCode.None;
        if (lastCmd != KeyCode.None)
        {
            tmp = Input.GetKeyDown(KeyCode.Alpha1) ? KeyCode.Alpha0 : tmp;
            tmp = Input.GetKeyDown(KeyCode.Alpha1) ? KeyCode.Alpha1 : tmp;
            tmp = Input.GetKeyDown(KeyCode.Alpha2) ? KeyCode.Alpha2 : tmp;
            tmp = Input.GetKeyDown(KeyCode.Alpha3) ? KeyCode.Alpha3 : tmp;
            tmp = Input.GetKeyDown(KeyCode.Alpha4) ? KeyCode.Alpha4 : tmp;
            if (tmp != KeyCode.None)
            {
                var playersToSet = new List<GameObject>();
                switch (tmp)
                {
                    case KeyCode.Alpha0:
                        foreach (var x in Players)
                        {
                            playersToSet.Add(x.Player);
                        }
                        break;
                    case KeyCode.Alpha1:
                        playersToSet.Add(Players[0].Player);
                        break;
                    case KeyCode.Alpha2:
                        playersToSet.Add(Players[1].Player);
                        break;
                    case KeyCode.Alpha3:
                        playersToSet.Add(Players[2].Player);
                        break;
                    case KeyCode.Alpha4:
                        playersToSet.Add(Players[3].Player);
                        break;
                }

                foreach (var player in playersToSet)
                {
                    switch (lastCmd)
                    {
                        case KeyCode.A:
                            player.GetComponent<NetworkTransform>().sendInterval = 0;
                            break;
                        case KeyCode.B:
                            player.GetComponent<NetworkTransform>().sendInterval = .01f;
                            break;
                        case KeyCode.C:
                            player.GetComponent<NetworkTransform>().transformSyncMode = NetworkTransform.TransformSyncMode.SyncNone;
                            break;
                        case KeyCode.D:
                            player.GetComponent<NetworkTransform>().transformSyncMode = NetworkTransform.TransformSyncMode.SyncTransform;
                            break;
                    }
                }

                return;
            }
        }

        tmp = Input.GetKeyDown(KeyCode.A) ? KeyCode.A : tmp;
        tmp = Input.GetKeyDown(KeyCode.B) ? KeyCode.B : tmp;
        tmp = Input.GetKeyDown(KeyCode.C) ? KeyCode.C : tmp;
        tmp = Input.GetKeyDown(KeyCode.D) ? KeyCode.D : tmp;
        if (tmp != KeyCode.None) lastCmd = tmp;

        if (!isLocalPlayer) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var ship = GameObject.FindGameObjectWithTag("Ship");
            if (gameObject.transform.parent == null)
            {
                gameObject.transform.parent = GameObject.FindGameObjectWithTag("Ship").transform;
                //gameObject.GetComponent<NetworkTransform>().transformSyncMode = NetworkTransform.TransformSyncMode.SyncNone;
                ship.GetComponent<NetworkTransformChild>().target = gameObject.transform;
                CmdAskStartDriving();
                driving = true;
                return;
            }
            else
            {
                gameObject.transform.parent = null;
                //gameObject.GetComponent<NetworkTransform>().transformSyncMode = NetworkTransform.TransformSyncMode.SyncTransform;
                gameObject.transform.parent = null;
                ship.GetComponent<NetworkTransformChild>().target = GameObject.FindGameObjectWithTag("Interior").transform;
                CmdAskStopDriving();
                driving = false;
                return;

            }
        }

        float speed = driving ? 1f : 5f;
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (!driving)
        {
            Vector2 pos = transform.position;
            transform.position = pos + (new Vector2(input.x, input.y) * speed * Time.deltaTime);
        }
        else
        {
            Vector2 pos = transform.localPosition;
            transform.localPosition = pos + (new Vector2(input.x, input.y) * speed * Time.deltaTime);
        }
    }
}
