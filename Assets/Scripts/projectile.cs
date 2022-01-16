using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    public int Damage, timeToDestroy;
    public float speed;
    public GameObject pla;
    public PhotonView pv;
    public List<int> targets;
    bool hit = false;
    public bool Inst = false;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        StartCoroutine(delay(timeToDestroy));
        GetComponent<Rigidbody>().AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && pv.isMine && other.name != "local" && other.gameObject.layer == 0 && !hit && !Inst)
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
                if (Damage > 0)
                {
                    targets.Add(other.GetComponent<PhotonView>().viewID);
                    GameObject go = PhotonNetwork.Instantiate("FireballExplode", transform.position, Quaternion.identity, 0);
                    other.GetComponent<PhotonView>().RPC("res", PhotonPlayer.Find(other.GetComponent<PhotonView>().viewID), Damage, PhotonNetwork.player.ID, (int)PhotonNetwork.player.CustomProperties["Squad"],0,false,false,0);
                    GameObject.Find("local").GetComponent<Movement>().ultimateCharge += 3;
                    pla.GetComponent<Movement>().HitMark();
                    PhotonNetwork.Destroy(gameObject);
                }
                else
                {
                    targets.Add(other.GetComponent<PhotonView>().viewID);
                    other.GetComponent<PhotonView>().RPC("res", PhotonPlayer.Find(other.GetComponent<PhotonView>().viewID), Damage, PhotonNetwork.player.ID, (int)PhotonNetwork.player.CustomProperties["Squad"],0,false,false,0);
                    GameObject.Find("local").GetComponent<Movement>().ultimateCharge += 3;
                    PhotonNetwork.Destroy(gameObject);

                }
            }
        }
        else if (pv.isMine && other.name != "local" && other.name != "GroundCol" && other.gameObject.layer == 0 && !hit)
        {
            print(other.name);
            StartCoroutine(delay(1));
            hit = true;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().Sleep();
            if (Inst)
            {
                GameObject go = PhotonNetwork.Instantiate("ArcherExplosion", transform.position, Quaternion.identity, 0);
                StartCoroutine(DestroyZone(go, 4));
            }
        }
    }
    [PunRPC]
    public void Look(Vector3 target)
    {
        transform.LookAt(target);
    }
    public IEnumerator delay(float time)
    {
        if (Inst) { time += 4; }
        yield return new WaitForSeconds(time);
        PhotonNetwork.Destroy(gameObject);
    }
    public IEnumerator DestroyZone(GameObject go, int time)
    {
        yield return new WaitForSeconds(time);
        PhotonNetwork.Destroy(go);
    }
}
