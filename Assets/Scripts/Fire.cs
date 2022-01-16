using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public bool attack;
    public int[] targets;
    public int Damage, BDamage, percentageHp = 0;
    public PhotonView pv;
    public GameObject playerCamera, projectilePrefab, origin, player, tentacleEffect1, tentacleEffect2;
    public Animator charAnim;
    public void Start()
    {
        pv = GetComponentInParent<PhotonView>();
    }
    public void StartFire()
    {
        GetComponent<Animator>().SetBool("attack", false);
        GetComponent<Animator>().SetBool("stay", false);
        charAnim.SetBool("stay", false);
        charAnim.SetBool("attack", false);
    }
    public void BladeAttack()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = 0;            
        }
        GetComponent<Animator>().SetBool("attack", false);
        GetComponent<Animator>().SetBool("stay", true);
        charAnim.SetBool("stay", true);
        charAnim.SetBool("attack", false);
        attack = true;
        StartCoroutine(delay());
    }
    public void EndAttack()
    {
        GetComponent<Animator>().SetBool("stay", false);
        charAnim.SetBool("stay", false);
    }
    public IEnumerator delay()
    {
        yield return new WaitForSeconds(0.25f);
        attack = false;
    }
    public void FireInst()
    {
        GetComponent<Animator>().SetBool("attack", false);
        GameObject go = PhotonNetwork.Instantiate(player.GetComponent<Movement>().Projectile, origin.transform.position, origin.transform.rotation, 0);
        player.GetComponent<Movement>().Projectile = "Arrow1";
        go.GetComponent<projectile>().pla = player;
    }
    public void OnTriggerStay(Collider other)
    {
        if (attack)
        {
            if(other.tag == "Player")
            {
                if(other.gameObject != player && (int)other.GetComponent<PhotonView>().owner.CustomProperties["Squad"] != (int)PhotonNetwork.player.CustomProperties["Squad"])
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
                        for (int i = 0; i < targets.Length; i++)
                        {
                            if (targets[i] == 0)
                            {
                                targets[i] = other.GetComponent<PhotonView>().viewID;
                                break;

                            }
                        }
                        other.GetComponent<PhotonView>().RPC("res", PhotonPlayer.Find(other.GetComponent<PhotonView>().viewID), Damage, PhotonNetwork.player.ID, (int)PhotonNetwork.player.CustomProperties["Squad"], 0, false, false, percentageHp);
                        GameObject.Find("local").GetComponent<Movement>().HitMark();
                        Damage = BDamage;
                        percentageHp = 0;
                        GameObject.Find("local").GetComponent<Movement>().ultimateCharge += 4 - GameObject.Find("local").GetComponent<Movement>().ultimateChargeSpeed;
                        tentacleEffect1.SetActive(false);
                        tentacleEffect2.SetActive(false);
                    }
                }
            }
            
        }
    }
    public void MageAttack()
    {
        RaycastHit hit;
        Ray ray;
        Vector2 SCP = new Vector2(Screen.width / 2, Screen.height / 2);
        ray = playerCamera.GetComponent<Camera>().ScreenPointToRay(SCP);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.transform.name != "local" && hit.transform.name != "GroundCol" && hit.transform.tag == "Player" && (int)hit.transform.GetComponent<PhotonView>().owner.CustomProperties["Squad"] != (int)PhotonNetwork.player.CustomProperties["Squad"])
            {
                //PhotonNetwork.Destroy(hit.transform.gameObject);
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("res", PhotonPlayer.Find(hit.transform.gameObject.GetComponent<PhotonView>().viewID), Damage, PhotonNetwork.player.ID, (int)PhotonNetwork.player.CustomProperties["Squad"], -5, false, false,0);
                PhotonNetwork.Instantiate("MageAttackEffect", hit.point, Quaternion.identity, 0);
                GameObject.Find("local").GetComponent<Movement>().ultimateCharge += 4 - GameObject.Find("local").GetComponent<Movement>().ultimateChargeSpeed;
                GameObject.Find("local").GetComponent<Movement>().HitMark();

            }
        }
    }
}
