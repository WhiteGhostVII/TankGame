using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FPSPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    PhotonView pview;
    float power;
    float turn;
    float turnAxisX;
    float turnAxisY;
    Rigidbody rdb;
    Animator anim;
    bool run = false;
    Vector3 localVelocity;
    public GameObject aim;
    public int live = 100;
    public Transform aimref;
    public VisualEffect vfxshoot;
    public GameObject laser;
    public AudioSource AudShoot;
    void Start()
    {
        rdb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        pview = GetComponent<PhotonView>();
        if (pview.IsMine)
        {
            Camera.main.GetComponent<NetCamera>().SetPlayer(gameObject);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            laser.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pview.IsMine)
        {            
            power = Input.GetAxis("Vertical");
            turn = Input.GetAxis("Horizontal");
            turnAxisX = Input.GetAxis("Mouse X");
            turnAxisY = Input.GetAxis("Mouse Y");
            run = Input.GetButton("Fire3");
            if(Input.GetButtonDown("Fire1"))
            {
                pview.RPC("Shoot", RpcTarget.AllBuffered, null);
            }
            
            
        }
        else
        {
            Collider[] cols = GetComponentsInChildren<Collider>();
            foreach (Collider col in cols)
            {
                col.gameObject.tag = "RemotePlayer";
            }
        }
    }
    private void FixedUpdate()
    {
        if(pview.IsMine)
        {
            aimref.Rotate(new Vector3(0,.1f,1) * turnAxisY, Space.Self);
            transform.Rotate(transform.up * turnAxisX);            
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
            localVelocity = transform.InverseTransformDirection(rdb.velocity)/ 2;
        }
        else
        {
            localVelocity = Vector3.Lerp(localVelocity,transform.InverseTransformDirection(rdb.velocity)/2,Time.fixedDeltaTime);
        }
        
        anim.SetFloat("VelX", localVelocity.x);
        anim.SetFloat("VelZ", localVelocity.z);
    }
    [PunRPC]
    void Shoot()
    {
        vfxshoot.Play();
        AudShoot.Play();
        if(Physics.Raycast(aim.transform.position,aim.transform.forward,out RaycastHit hit, 1000))
        {
            if (hit.collider.CompareTag("RemotePlayer"))
            {
                PhotonView remotepview = hit.collider.GetComponentInParent<PhotonView>();
                if(remotepview)
                {
                    remotepview.RPC("Shooted", RpcTarget.AllBuffered, null);
                }
            }
        }
        anim.SetTrigger("Shoot");
    }

    [PunRPC]
    public void Shooted()
    {
        live-=10;
        anim.SetTrigger("Damage");
        if(live<1)
        {
            anim.SetBool("Died", true);
            this.enabled = false;
            Invoke("SelfDestroy", 5);
        }
    }
    private void OnAnimatorIK(int layerIndex)
    {
        anim.SetBoneLocalRotation(HumanBodyBones.Spine, aimref.localRotation);
    }
    void SelfDestroy()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
