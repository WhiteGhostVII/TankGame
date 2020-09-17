using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSID : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMesh myname;
    public PhotonView pview;
    void Start()
    {
        myname.text = pview.Owner.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        myname.transform.forward = transform.position - Camera.main.transform.position;
    }
}
