using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerName : MonoBehaviour
{
    public Text nametext;
    public PhotonView pview;
    public Toggle readyToggle;
    public bool ready = false;
    // Start is called before the first frame update
    void Start()
    {

        nametext.text = pview.Owner.NickName;

        GameObject content = GameObject.FindGameObjectWithTag("PlayerList");
        if(content)
        transform.SetParent(content.transform);
        transform.localScale = new Vector3(1, 1, 1);
        if (pview.IsMine)
        {
            readyToggle.interactable = true;
        }

    }

    public void Reset()
    {
        nametext.text = pview.Owner.NickName;
        GameObject content = GameObject.FindGameObjectWithTag("PlayerList");
        transform.SetParent(content.transform);
        transform.localScale = new Vector3(1, 1, 1);
        readyToggle.isOn = false;
        if (pview.IsMine)
        {
            readyToggle.interactable = true;
        }
        else
        {
            readyToggle.interactable = false;
        }
    }

    public void ReadyisChange()
    {
        if (ready != readyToggle.isOn)
        {
            ready = readyToggle.isOn;
            pview.RPC("StatusChange", RpcTarget.OthersBuffered, ready);
        }
    }
    
    [PunRPC]
    void StatusChange(bool myready)
    {
        readyToggle.isOn = myready;
        ready = myready;
    }



}
