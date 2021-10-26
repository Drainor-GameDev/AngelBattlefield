using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public int Damage, timer;
    private void Start()
    {
        StartCoroutine(delay());
    }
    public IEnumerator delay()
    {
        yield return new WaitForSeconds(timer);
        PhotonNetwork.Destroy(gameObject);
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.tag == "Player")
    //    {
    //        bool test = false;
    //        int id = other.GetComponent<PhotonView>().viewID;
    //        foreach (int IDtest in IDS)
    //        {
    //            if(IDtest == id)
    //            {
    //                test = true;
    //            }
    //        }
    //        if (!test)
    //        {
    //            other.GetComponent<PhotonView>().RPC("res", PhotonPlayer.Find(id), Damage, PhotonNetwork.player.ID);
    //            for (int ids = 0; ids < IDS.Length; ids++)
    //            {
    //                if (IDS[ids] == 0)
    //                {
    //                    IDS[ids] = id;
    //                }
    //            }
    //        }
    //    }
    //}
}
