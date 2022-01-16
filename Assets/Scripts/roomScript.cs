using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class roomScript : MonoBehaviour
{
    public GameObject locker, sc1,sc2, sc3, startButton;
    public GameObject[] champs, profils, team1, team2;
    public Sprite[] sourceImages;
    AsyncOperation aO;
    public bool ready = false, select = false, load = false, forceStart = false;
    public string levelToLoad;
    public void Start()
    {
        sc1.SetActive(true);
        sc2.SetActive(false);
        sc3.SetActive(false);
        locker.SetActive(false);
        Hashtable hash = new Hashtable();
        hash.Add("Ready", false);
        hash.Add("Loaded", false);
        hash.Add("Forced", false);
        hash.Add("Champion", -1);
        hash.Add("Squad", 0);
        PhotonNetwork.player.SetCustomProperties(hash);
        startButton.SetActive(PhotonNetwork.isMasterClient);
    }
    public void Update()
    {
        try
        {
            bool test = false;
            foreach (PhotonPlayer pla in PhotonNetwork.playerList)
            {
                if ((bool)pla.CustomProperties["Forced"] == true && pla.IsMasterClient)
                {
                    PhotonNetwork.isMessageQueueRunning = false;
                    if (!load)
                    {
                        load = true;
                        sc3.SetActive(true);
                        sc2.SetActive(false);
                        StartCoroutine(LoadScenneWithDelay(3, levelToLoad));
                    }
                    break;
                }
                if ((bool)pla.CustomProperties["Ready"] == false && (int)pla.CustomProperties["Squad"] != 0)
                {
                    test = true;
                }
            }
            int players = 0;
            foreach (PhotonPlayer pla in PhotonNetwork.playerList)
            {
                if ((int)pla.CustomProperties["Squad"] != 0)
                {
                    players++;
                }
            }
            if ((!test && players == PhotonNetwork.room.MaxPlayers) || forceStart)
            {
                PhotonNetwork.isMessageQueueRunning = false;
                if (!load)
                {
                    load = true;
                    sc3.SetActive(true);
                    sc2.SetActive(false);
                    StartCoroutine(LoadScenneWithDelay(3, levelToLoad));
                }
            }
            int index = 0;
            foreach (PhotonPlayer pla in PhotonNetwork.playerList)
            {
                if ((int)pla.CustomProperties["Squad"] == (int)PhotonNetwork.player.CustomProperties["Squad"])
                {
                    if (pla != PhotonNetwork.player)
                    {
                        index++;
                        if ((int)pla.CustomProperties["Champion"] != -1)
                        {
                            profils[index].GetComponent<Image>().sprite = sourceImages[(int)pla.CustomProperties["Champion"]];
                            profils[index].GetComponent<Image>().color = Color.white;
                        }
                    }
                    else
                    {
                        if ((int)pla.CustomProperties["Champion"] != -1)
                        {
                            profils[0].GetComponent<Image>().sprite = sourceImages[(int)pla.CustomProperties["Champion"]];
                            profils[0].GetComponent<Image>().color = Color.white;
                        }
                    }
                }
            }
            int index1 = 0;
            foreach (PhotonPlayer pla in PhotonNetwork.playerList)
            {
                if ((int)pla.CustomProperties["Squad"] == 1)
                {
                    if ((int)pla.CustomProperties["Champion"] != -1)
                    {
                        team1[index1].GetComponent<Image>().sprite = sourceImages[(int)pla.CustomProperties["Champion"]];
                        team1[index1].GetComponent<Image>().color = Color.white;
                    }
                    index1++;
                }
            }
            int index2 = 0;
            foreach (PhotonPlayer pla in PhotonNetwork.playerList)
            {
                if ((int)pla.CustomProperties["Squad"] == 2)
                {
                    if ((int)pla.CustomProperties["Champion"] != -1)
                    {
                        team2[index2].GetComponent<Image>().sprite = sourceImages[(int)pla.CustomProperties["Champion"]];
                        team2[index2].GetComponent<Image>().color = Color.white;
                    }
                    index2++;
                }
            }
        }
        catch
        {
            print("pas encore log");
        }
    }
    public void SetChamp(int champ)
    {
        if (!ready)
        {
            bool test = false;
            foreach (PhotonPlayer pla in PhotonNetwork.playerList)
            {
                if ((int)pla.CustomProperties["Champion"] == champ && (int)pla.CustomProperties["Squad"] == (int)PhotonNetwork.player.CustomProperties["Squad"])
                {
                    test = true;
                }
            }
            if (!test)
            {
                select = true;
                foreach (GameObject go in champs)
                {
                    go.SetActive(false);
                }
                champs[champ].SetActive(true);
                Hashtable hash = new Hashtable();
                hash.Add("Champion", champ);
                PhotonNetwork.player.SetCustomProperties(hash);
            }
        }
    }
    public void Ready()
    {
        if (select && !ready)
        {
            ready = true;
            Hashtable hash = new Hashtable();
            hash.Add("Ready", true);
            PhotonNetwork.player.SetCustomProperties(hash);
            locker.SetActive(true);
        }
    }
    public void JoinTeam(int team)
    {
        Hashtable hash = new Hashtable();
        hash.Add("Squad", team);
        PhotonNetwork.player.SetCustomProperties(hash);
        sc1.SetActive(false);
        if(team == 0)
        {
            PhotonNetwork.player.SetTeam(PunTeams.Team.none);
            sc3.SetActive(true);
        }
        else
        {
            int playersTeam1 = 0,playersTeam2 = 0;
            foreach (PhotonPlayer pla in PhotonNetwork.playerList)
            {
                if ((int)pla.CustomProperties["Squad"] == 1)
                {
                    playersTeam1++;
                }
                if ((int)pla.CustomProperties["Squad"] == 2)
                {
                    playersTeam2++;
                }
            }
            if (team == 1 && playersTeam1 < 5)
            {
                PhotonNetwork.player.SetTeam(PunTeams.Team.blue);
                sc2.SetActive(true);
            }
            else if (playersTeam2 < 5)
            {
                PhotonNetwork.player.SetTeam(PunTeams.Team.red);
                sc2.SetActive(true);
            }
        }
    }

    public void ForceStart()
    {
        bool test = false;
        foreach (PhotonPlayer pla in PhotonNetwork.playerList)
        {
            if ((int)pla.CustomProperties["Champion"] == -1)
            {
                test = true;
            }
        }
        if (!test)
        {
            Hashtable hash = new Hashtable();
            hash.Add("Forced", true);
            PhotonNetwork.player.SetCustomProperties(hash);
            forceStart = true;
        }
    }

    public IEnumerator LoadScenneWithDelay(int delay, string scenne)
    {
        yield return new WaitForSeconds(delay);
        PhotonNetwork.LoadLevelAsync(scenne);
    }
}
