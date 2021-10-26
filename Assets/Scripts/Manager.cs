using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Manager : MonoBehaviour
{
    public GameObject[] spawns;
    public GameObject player;
    public int squad;
    bool setPlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        if ((int)PhotonNetwork.player.CustomProperties["Squad"] != 0)
        {
            Hashtable hash = new Hashtable();
            hash.Add("Loaded", true);
            PhotonNetwork.player.SetCustomProperties(hash);
        }
    }
    public void Update()
    {
        int loaded = 0;

        if (!setPlayer)
        {
            foreach (PhotonPlayer pla in PhotonNetwork.playerList)
            {
                if ((bool)pla.CustomProperties["Loaded"] != false)
                {
                    loaded++;
                }
            }
            if (loaded == PhotonNetwork.room.MaxPlayers)
            {
                setPlayer = true;
                GameObject go = PhotonNetwork.Instantiate("Player" + PhotonNetwork.player.CustomProperties["Champion"], spawns[(int)PhotonNetwork.player.CustomProperties["Squad"] - 1].transform.position, Quaternion.identity, 0);
                go.GetComponent<Movement>().playerCamera.SetActive(true);
            }
        }
    }

    public void Inst(int ID)
    {
        player.transform.position = spawns[ID].transform.position;
        player.GetComponent<Movement>().respawn();
    }

}
