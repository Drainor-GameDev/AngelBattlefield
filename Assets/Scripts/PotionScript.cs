using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KILLER
{
	public class PotionScript : MonoBehaviour
	{
        [SerializeField]
        int Heal;
        private PhotonView pv;

        public void Start()
        {
            pv = GetComponent<PhotonView>();
        }
        public void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player" && pv.isMine && other.gameObject.name != "local" && other.gameObject.layer == 0)
            {

                other.GetComponent<PhotonView>().RPC("res", PhotonPlayer.Find(other.GetComponent<PhotonView>().viewID), -Heal, PhotonNetwork.player.ID, (int)PhotonNetwork.player.CustomProperties["Squad"], 0, false, false,0);
                PhotonNetwork.Destroy(gameObject);

            }
        }
    }
}