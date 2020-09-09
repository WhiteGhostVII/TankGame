using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : MonoBehaviour
{
    public GameObject[] respawns;
    TankID[] tanks;
    public PhotonView pview;
    public GameObject winner;
    public GameRoom gameRoom;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("StartGame", 3);
       

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartGame()
    {
        int indexrespawn = Random.Range(0, respawns.Length);
        if (respawns[indexrespawn].GetComponent<RespawnValidator>().thing == null)
        {
            PhotonNetwork.Instantiate("TankFree", respawns[indexrespawn].transform.position, respawns[indexrespawn].transform.rotation, 0);
            InvokeRepeating("CheckStatus", 3, 1);
        }
        else
        {
            Invoke("StartGame", .1f);
        }

    }

    void CheckStatus()
    {
        tanks = FindObjectsOfType<TankID>();
        if (tanks.Length < 2)
        {
            pview.RPC("VictoryTank", RpcTarget.AllBuffered);
            PhotonNetwork.Instantiate("PlayerName", Vector3.zero, Quaternion.identity);
            CancelInvoke("CheckStatus");
        }
    }
    [PunRPC]
    void VictoryTank()
    {
        

        tanks = FindObjectsOfType<TankID>();
        Camera.main.GetComponent<NetCamera>().SetPlayer(tanks[0].gameObject);
        winner.transform.position = tanks[0].transform.position;
        winner.SetActive(true);
        Invoke("EnableGameRoom", 5);
    }


    void EnableGameRoom()
    {
        gameRoom.enabled = true;
    }
}
