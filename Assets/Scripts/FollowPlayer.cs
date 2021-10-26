using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    PhotonView pv;
    public void Start()
    {
        pv = GetComponent<PhotonView>();
    }
    void Update()
    {
        if (pv.isMine)
        {
            transform.position = player.transform.position;
        }
    }
}
