using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameRoom : MonoBehaviour
{

    public GameObject roompanel;
    public PlayerName[] players;
    // Start is called before the first frame update
    void Start()
    {
        roompanel.SetActive(true);
        players = FindObjectsOfType<PlayerName>();
        foreach(PlayerName player in players)
        {
            player.Reset();
        }
        InvokeRepeating("CheckAllReady", 1, 1);
    }

    void CheckAllReady()
    {
        print("Checando...");
        
        bool allready = true;
        if (players.Length > 1)
        {
            allready = players.All(param => param.ready);

            if (allready)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false; //fecha a sala pra ninguem entrar
                PhotonNetwork.LoadLevel("Level1");
            }
        }
    }
}
