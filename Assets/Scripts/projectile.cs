using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    public int Damage;
    public float speed;
    public GameObject pla;
    public PhotonView pv;
    public List<int> targets;
    bool hit = false;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        StartCoroutine(delay(5));
    }

    // Update is called once per frame
    void Update()
    {
        if (!hit)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && pv.isMine && other.name != "local" && other.gameObject.layer == 0 && hit)
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
        else if (pv.isMine && other.name != "local" && other.name != "GroundCol" && other.gameObject.layer == 0)
        {
            print(other.name);
            StartCoroutine(delay(1));
            hit = true;
        }
    }
    [PunRPC]
    public void Look(Vector3 target)
    {
        transform.LookAt(target);
    }
    public IEnumerator delay(float time)
    {
        yield return new WaitForSeconds(time);
        PhotonNetwork.Destroy(gameObject);
    }
}
