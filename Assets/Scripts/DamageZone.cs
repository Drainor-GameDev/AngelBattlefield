using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public int Damage;
    public PhotonView pv;
    public List<int> targets;
    private List<GameObject> colliders = new List<GameObject>();
    public bool repeat = false;
    public void Start()
    {
        pv = GetComponent<PhotonView>();
        if (repeat) { StartCoroutine(delay()); }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && pv.isMine && other.gameObject.name != "local" && other.gameObject.layer == 0)
        {
            print(other.name);
            if (repeat)
            {
                if (!colliders.Contains(other.gameObject)) { colliders.Add(other.gameObject); }
            }
            else
            {
                bool test = false;
                foreach (int id in targets)
                {
                    if (id == other.GetComponent<PhotonView>().viewID)
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
                            other.GetComponent<PhotonView>().RPC("res", PhotonPlayer.Find(other.GetComponent<PhotonView>().viewID), Damage, PhotonNetwork.player.ID, (int)PhotonNetwork.player.CustomProperties["Squad"], 0, false, false, 0);
                            GameObject.Find("local").GetComponent<Movement>().ultimateCharge += 4 - GameObject.Find("local").GetComponent<Movement>().ultimateChargeSpeed;
                            GameObject.Find("local").GetComponent<Movement>().HitMark();
                        }
                    }
                    else
                    {
                        if ((int)other.GetComponent<PhotonView>().owner.CustomProperties["Squad"] == (int)PhotonNetwork.player.CustomProperties["Squad"])
                        {
                            other.GetComponent<PhotonView>().RPC("res", PhotonPlayer.Find(other.GetComponent<PhotonView>().viewID), Damage, PhotonNetwork.player.ID, (int)PhotonNetwork.player.CustomProperties["Squad"], 0, false, false, 0);
                            GameObject.Find("local").GetComponent<Movement>().ultimateCharge += (4 - GameObject.Find("local").GetComponent<Movement>().ultimateChargeSpeed) + 1;
                        }
                    }
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && pv.isMine && other.gameObject.name != "local" && other.gameObject.layer == 0)
        {
            colliders.Remove(other.gameObject);
        }
    }
    public IEnumerator delay()
    {
        yield return new WaitForSeconds(1f);
        foreach(GameObject col in colliders)
        {
            col.GetComponent<PhotonView>().RPC("res", PhotonPlayer.Find(col.GetComponent<PhotonView>().viewID), Damage, PhotonNetwork.player.ID, (int)PhotonNetwork.player.CustomProperties["Squad"], 0, false, false, 0);
            GameObject.Find("local").GetComponent<Movement>().ultimateCharge += 4 - GameObject.Find("local").GetComponent<Movement>().ultimateChargeSpeed;
            GameObject.Find("local").GetComponent<Movement>().HitMark();
        }
        StartCoroutine(delay());
    }
}
