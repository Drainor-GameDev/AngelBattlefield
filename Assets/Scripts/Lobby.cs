using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Lobby : MonoBehaviour
{
    public TMPro.TMP_Text newsTitle, newsBody, money;
    public GameObject[] Friends;
    public GameObject tab, content, FriendPref, searchTab, shopTab, webTab, contactTab, settingsTab;
    public TMPro.TMP_Text txtName, txtLvl;
    public Slider sl;
    public InputField pName, rName;
    public int nb, roomTest;
    public byte rm;
    public KILLER.PlayFabManager pfm;
    public string mapName;

    // Start is called before the first frame update
    void Start()
    {
        settingsTab.GetComponent<KILLER.Setting>().myCloseEvent.AddListener(SettingsTab);
        pfm = GameObject.Find("PlayFabManager").GetComponent<KILLER.PlayFabManager>();
        pfm.ReadTitleNews();
        pfm.GetInventory();
        pfm.GetFriends();
        PhotonNetwork.ConnectUsingSettings("Alpha1.2"); 
        StartCoroutine(delay());
        txtName.text = pfm.Player_DisplayName;
        txtLvl.text = "Level: " + pfm.Player_Lvl.ToString();
    }
    public IEnumerator delay()
    {
        yield return new WaitForSeconds(0.5f);
        actualiseTitle();
    }
    public void RejRoom(int room = 0)
    {
        PhotonNetwork.JoinRoom(room.ToString());
    }
    public void CreateRoom(int PlayersNumber)
    {
        rm = (byte)PlayersNumber;
        RoomOptions MyRoomOption = new RoomOptions();
        MyRoomOption.MaxPlayers = rm;
        MyRoomOption.IsVisible = true;
        PhotonNetwork.CreateRoom(rm + roomTest.ToString(), MyRoomOption, TypedLobby.Default);
    }
    void OnJoinedRoom()
    {
        pfm.saveRoom();
        pfm.GetInventory();
        PhotonNetwork.player.NickName = pfm.Player_DisplayName;
        PhotonNetwork.isMessageQueueRunning = false;
        //PhotonNetwork.LoadLevel("Room");
        PhotonNetwork.LoadLevelAsync(mapName);
    }
    public void OnPhotonCreateRoomFailed()
    {
        roomTest++;
        CreateRoom(rm);
    }
    public void OnPhotonJoinRoomFailed()
    {
        pfm.LoadingMessage("The ROOM does not exist");
        pfm.LoadingHide();
    }
    public void SettingsTab()
    {
        if (settingsTab.active == true)
        {
            settingsTab.SetActive(false);
        }
        else
        {
            settingsTab.SetActive(true);
        }
    }
    public void JoinMain()
    {
        RoomOptions MyRoomOption = new RoomOptions();
        MyRoomOption.MaxPlayers = 6;
        MyRoomOption.IsVisible = true;
        PhotonNetwork.JoinOrCreateRoom("Main", MyRoomOption, TypedLobby.Default);
    }

    public void FriendsTab()
    {
        if (tab.active == true)
        {
            foreach (GameObject go in Friends)
            {
                Destroy(go);
            }
            tab.SetActive(false);
        }
        else
        {
            pfm.GetFriends();
            int count = 0;
            tab.SetActive(true);
            foreach (PlayFab.ClientModels.FriendInfo friends in pfm.friendList)
            {
                content.GetComponent<RectTransform>().sizeDelta = new Vector2(content.GetComponent<RectTransform>().sizeDelta.x, content.GetComponent<RectTransform>().sizeDelta.y + 54.56943f);
                print(friends.Profile.LastLogin);
                GameObject go = Instantiate(FriendPref, content.transform);
                Friends.SetValue(go, count);
                count++;
                go.GetComponent<KILLER.FriendCardScript>().InitializeComponent(friends.Profile.Statistics[2].Value, friends.Profile.DisplayName, friends.Profile.PlayerId);
            }
        }
    }
    public void FriendsSearchTab()
    {
        if (searchTab.active == true)
        {
            searchTab.SetActive(false);
        }
        else
        {
            searchTab.SetActive(true);
        }
    }
    public void ShopTab()
    {
        if (shopTab.active == true)
        {
            shopTab.SetActive(false);
        }
        else
        {
            money.text = pfm.Player_Dollar.ToString() + "$";
            shopTab.SetActive(true);
        }
    }
    public void AddFriend()
    {
        pfm.ADDFriend();
        FriendsTab();
        FriendsTab();
    }
    public void actualiseTitle()
    {
        foreach (var item in pfm.news.News)
        {
            newsTitle.text = item.Title;
            newsBody.text = item.Body;
        }
    }
    public void BuyItem(string ID)
    {
        pfm.MakePurchase(ID);
    }
    public void ShowPaiment(string URL = "")
    {
        if (webTab.active == true)
        {
            webTab.SetActive(false);
        }
        else
        {
            webTab.SetActive(true);
            webTab.GetComponent<VideoPlayer>().url = URL;
            webTab.GetComponent<VideoPlayer>().Play();
        }
    }
    public void OpenUrl(string URL)
    {
        Application.OpenURL(URL);
    }
    public void ShowContact()
    {
        if (contactTab.active == true)
        {
            contactTab.SetActive(false);
        }
        else
        {
            contactTab.SetActive(true);
        }
    }
    public void NotAvailable(string Mess)
    {
        pfm.LoadingMessage("No " + Mess + " available (Coming Soon)");
        pfm.LoadingHide();
    }

    public void SetMap(string map)
    {
        mapName = map;
    }
}
