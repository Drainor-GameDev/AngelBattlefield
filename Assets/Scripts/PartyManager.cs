using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace KILLER
{
	public class PartyManager : MonoBehaviour
    {
        public bool end = false;
        public GameObject scoreBoard;
        public Image[] PPteam1, PPteam2;
        public TMPro.TextMeshProUGUI txtPlayerSquad1, txtPlayerSquad2, txtKillSquad1, txtKillSquad2, txtDeathSquad1, txtDeathSquad2, txtChampSquad1, txtChampSquad2, txtKillsSquad1, txtKillsSquad2;
        public List<string> champs = new List<string>();
        public void Start()
        {
            if (end)
            {
                GameObject.Find("PlayFabManager").GetComponent<PlayFabManager>().AddXp(1250 + 125 * (int)PhotonNetwork.player.CustomProperties["Kill"]);
                int kill1 = 0, kill2 = 0;
                foreach (PhotonPlayer play in PhotonNetwork.playerList)
                {
                    if ((int)play.CustomProperties["Squad"] == 1)
                    {
                        kill1 += (int)play.CustomProperties["Kill"];
                    }
                    else if ((int)play.CustomProperties["Squad"] == 2)
                    {
                        kill2 += (int)play.CustomProperties["Kill"];
                    }
                }
                scoreBoard.SetActive(true);
                txtPlayerSquad1.text = null;
                txtPlayerSquad2.text = null;
                txtKillSquad1.text = null;
                txtKillSquad2.text = null;
                txtDeathSquad1.text = null;
                txtDeathSquad2.text = null;
                txtChampSquad1.text = null;
                txtChampSquad2.text = null;
                int team1 = 0, team2 = 0;
                foreach (PhotonPlayer pla in PhotonNetwork.playerList)
                {
                    if ((int)pla.CustomProperties["Squad"] == 1)
                    {
                        PPteam1[team1].gameObject.SetActive(true);
                        PPteam1[team1].sprite = Resources.Load<Sprite>("PP/" + pla.CustomProperties["PPID"]);
                        team1++;
                        txtPlayerSquad1.text += pla.NickName;
                        txtKillSquad1.text += pla.CustomProperties["Kill"];
                        txtDeathSquad1.text += pla.CustomProperties["Death"];
                        txtChampSquad1.text += champs[(int)pla.CustomProperties["Champion"]];
                    }
                    else if ((int)pla.CustomProperties["Squad"] == 2)
                    {
                        PPteam2[team2].gameObject.SetActive(true);
                        PPteam2[team2].sprite = Resources.Load<Sprite>("PP/" + pla.CustomProperties["PPID"]);
                        team2++;
                        txtPlayerSquad2.text += pla.NickName;
                        txtKillSquad2.text += pla.CustomProperties["Kill"];
                        txtDeathSquad2.text += pla.CustomProperties["Death"];
                        txtChampSquad2.text += champs[(int)pla.CustomProperties["Champion"]];
                    }
                }
                txtKillsSquad1.text = "KILLS: " + kill1.ToString();
                txtKillsSquad2.text = "KILLS: " + kill2.ToString();
            }
        }
        void Update()
 	    {
            if (!end)
            {
                int kill1 = 0, kill2 = 0;
                foreach (PhotonPlayer play in PhotonNetwork.playerList)
                {
                    if ((int)play.CustomProperties["Squad"] == 1)
                    {
                        kill1 += (int)play.CustomProperties["Kill"];
                    }
                    else if ((int)play.CustomProperties["Squad"] == 2)
                    {
                        kill2 += (int)play.CustomProperties["Kill"];
                    }
                }
                if (Input.GetKey(KeyCode.Tab))
                {
                    scoreBoard.SetActive(true);
                    txtPlayerSquad1.text = null;
                    txtPlayerSquad2.text = null;
                    txtKillSquad1.text = null;
                    txtKillSquad2.text = null;
                    txtDeathSquad1.text = null;
                    txtDeathSquad2.text = null;
                    txtChampSquad1.text = null;
                    txtChampSquad2.text = null;
                    int team1 = 0, team2 = 0;
                    foreach (PhotonPlayer pla in PhotonNetwork.playerList)
                    {
                        if ((int)pla.CustomProperties["Squad"] == 1)
                        {
                            PPteam1[team1].gameObject.SetActive(true);
                            PPteam1[team1].sprite = Resources.Load<Sprite>("PP/" + pla.CustomProperties["PPID"]);
                            team1++;
                            txtPlayerSquad1.text += pla.NickName;
                            txtKillSquad1.text += pla.CustomProperties["Kill"];
                            txtDeathSquad1.text += pla.CustomProperties["Death"];
                            txtChampSquad1.text += champs[(int)pla.CustomProperties["Champion"]];
                        }
                        else if ((int)pla.CustomProperties["Squad"] == 2)
                        {
                            PPteam2[team2].gameObject.SetActive(true);
                            PPteam2[team2].sprite = Resources.Load<Sprite>("PP/" + pla.CustomProperties["PPID"]);
                            team2++;
                            txtPlayerSquad2.text += pla.NickName;
                            txtKillSquad2.text += pla.CustomProperties["Kill"];
                            txtDeathSquad2.text += pla.CustomProperties["Death"];
                            txtChampSquad2.text += champs[(int)pla.CustomProperties["Champion"]];
                        }
                    }
                    txtKillsSquad1.text = "KILLS: " + kill1.ToString();
                    txtKillsSquad2.text = "KILLS: " + kill2.ToString();
                }
                else
                {
                    scoreBoard.SetActive(false);
                }
                if (PhotonNetwork.masterClient.IsMasterClient)
                {
                    if (kill1 >= 2 || kill2 >= 2)
                    {
                        Hashtable hash = new Hashtable();
                        hash.Add("ScoreSquad1", kill1);
                        hash.Add("ScoreSquad2", kill2);
                        PhotonNetwork.player.SetCustomProperties(hash);
                        GetComponent<PhotonView>().RPC("EndGame", PhotonTargets.OthersBuffered);
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                        PhotonNetwork.LoadLevel("EndGame");
                    }
                }
            }
        }
        [PunRPC]
        public void EndGame()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //PhotonNetwork.Disconnect();
            //Destroy(GameObject.Find("Discord"));
            PhotonNetwork.LoadLevel("EndGame");
        }
        public void Leave()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PhotonNetwork.Disconnect();
            Destroy(GameObject.Find("Discord"));
            PhotonNetwork.LoadLevel("Menu");
        }
	}
}