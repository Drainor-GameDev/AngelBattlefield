using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public int Damage;
    public PhotonView pv;
    public List<int> targets;
    public void Start()
    {
        pv = GetComponent<PhotonView>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && pv.isMine && other.gameObject.name != "local" && other.gameObject.layer == 0)
        {
            print(other.name);
            bool test = false;
            foreach(int id in targets)
            {
                if(id == other.GetComponent<PhotonView>().viewID)
                {
                    test = true;
                }
            }
            if (!test)
            {
                targets.Add(other.GetComponent<PhotonView>().viewID);
                if (Damage > 0)
                {
                    if ((int)other.GetComponent<PhotonView>().owner.CustomProperties["Squad"] != (int)PhotonNetwork.player.CustomProperties["Squad"])
                    {
                        other.GetComponent<PhotonView>().RPC("res", PhotonPlayer.Find(other.GetComponent<PhotonView>().viewID), Damage, PhotonNetwork.player.ID, (int)PhotonNetwork.player.CustomProperties["Squad"], 0, false, false,0);
                        GameObject.Find("local").GetComponent<Movement>().ultimateCharge += 4 - GameObject.Find("local").GetComponent<Movement>().ultimateChargeSpeed;
                    }
                }
                else
                {
                    if ((int)other.GetComponent<PhotonView>().owner.CustomProperties["Squad"] == (int)PhotonNetwork.player.CustomProperties["Squad"])
                    {
                        other.GetComponent<PhotonView>().RPC("res", PhotonPlayer.Find(other.GetComponent<PhotonView>().viewID), Damage, PhotonNetwork.player.ID, (int)PhotonNetwork.player.CustomProperties["Squad"], 0, false, false,0);
                        GameObject.Find("local").GetComponent<Movement>().ultimateCharge += (4 - GameObject.Find("local").GetComponent<Movement>().ultimateChargeSpeed)+1;
                    }
                }
            }
        }
    }
}
