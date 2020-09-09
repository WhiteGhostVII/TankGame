using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class Weapons : MonoBehaviour
{
    public PhotonView pview;
    public GameObject pointoffire;
    float cooldown;
    public VisualEffect vfx;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pview.IsMine)
        {
            cooldown -= Time.deltaTime;
            if (Input.GetButtonDown("Jump")&&cooldown<0)
            {
               GameObject ob= (GameObject) 
                    PhotonNetwork.Instantiate("Bullet", pointoffire.transform.position,
                    pointoffire.transform.rotation, 0);
               cooldown = 3;
               vfx.Play();

            }
        }
    }
}
