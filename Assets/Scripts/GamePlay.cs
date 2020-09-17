using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : MonoBehaviour
{
    public GameObject[] respawns;
    TankID[] tanks;
    FPSID[] fpss;
    public PhotonView pview;
    public GameObject winner;
    public GameRoom gameRoom;
    public string playerPrefab;
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
        Cursor.lockState = CursorLockMode.Locked;        
        int indexrespawn = Random.Range(0, respawns.Length);
        if (respawns[indexrespawn].GetComponent<RespawnValidator>().thing == null)
        {
            PhotonNetwork.Instantiate(playerPrefab, respawns[indexrespawn].transform.position, respawns[indexrespawn].transform.rotation, 0);
            if (GameRoutines.gameType == GameRoutines.GameType.FPS)
                InvokeRepeating("CheckStatusFPS", 3, 1);

            if (GameRoutines.gameType == GameRoutines.GameType.Tank)
                InvokeRepeating("CheckStatusTank", 3, 1);
        }
        else
        {
            Invoke("StartGame", .1f);
        }

    }

    void CheckStatusTank()
    {
        tanks = FindObjectsOfType<TankID>();
        if (tanks.Length < 2)
        {
            pview.RPC("VictoryTank", RpcTarget.AllBuffered);
            PhotonNetwork.Instantiate("PlayerName", Vector3.zero, Quaternion.identity);
            CancelInvoke("CheckStatusTank");
        }
    }
    void CheckStatusFPS()
    {
        fpss = FindObjectsOfType<FPSID>();
        if (fpss.Length < 2)
        {
            pview.RPC("VictoryFPS", RpcTarget.AllBuffered);
            PhotonNetwork.Instantiate("PlayerName", Vector3.zero, Quaternion.identity);
            CancelInvoke("CheckStatusFPS");
        }
    }
    [PunRPC]
    void VictoryTank()
    {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        tanks = FindObjectsOfType<TankID>();
        Camera.main.GetComponent<NetCamera>().SetPlayer(tanks[0].gameObject);
        winner.transform.position = tanks[0].transform.position;
        winner.SetActive(true);
        Invoke("EnableGameRoom", 5);
    }
    [PunRPC]
    void VictoryFPS()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        fpss = FindObjectsOfType<FPSID>();
        Camera.main.GetComponent<NetCamera>().SetPlayer(fpss[0].gameObject);
        winner.transform.position = fpss[0].transform.position;
        winner.SetActive(true);
        Invoke("EnableGameRoom", 5);
    }


    void EnableGameRoom()
    {
        gameRoom.enabled = true;
    }
}
