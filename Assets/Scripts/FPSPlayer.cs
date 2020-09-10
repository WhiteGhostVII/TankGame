using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    PhotonView pview;
    float power;
    float turn;
    float turnAxis;
    Rigidbody rdb;
    Animator anim;
    bool run = false;
    void Start()
    {
        rdb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        pview = GetComponent<PhotonView>();
        if (pview.IsMine)
        {
            Camera.main.GetComponent<NetCamera>().SetPlayer(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pview.IsMine)
        {
            power = Input.GetAxis("Vertical");
            turn = Input.GetAxis("Horizontal");
            turnAxis = Input.GetAxis("Mouse X");
            run = Input.GetButton("Fire3");
            if(Input.GetButtonDown("Fire1"))
            {
                pview.RPC("Shoot", RpcTarget.AllBuffered, null);
            }
            
        }
    }
    private void FixedUpdate()
    {
        if(pview.IsMine)
        {
            transform.Rotate(transform.up * turnAxis);            
            Vector3 localmovement = transform.TransformDirection(new Vector3(turn, 0, power));
             
            if(run)
            {
                localmovement = localmovement * 4;
            }
            else
            {
                localmovement = localmovement * 2;
            }
            rdb.velocity = new Vector3(localmovement.x, rdb.velocity.y, localmovement.z);
        }
        Vector3 localVelocity = transform.InverseTransformDirection(rdb.velocity/2);
        anim.SetFloat("VelX", localVelocity.x);
        anim.SetFloat("VelZ", localVelocity.z);
    }
    [PunRPC]
    void Shoot()
    {
        anim.SetTrigger("Shoot");
    }
}
