using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Lobby : MonoBehaviour
{
    public TMPro.TMP_Text newsTitle, newsBody, money;
    public List<GameObject> Friends, ShopWindows, invWindows;
    public GameObject tab, content, PPcontent, TitleContent, SkinContent, FriendPref, ProfilePref, TitlePref, SkinPref, ShopProfilePref, ShopTitlePref, ShopSkinPref, searchTab, shopTab, inventoryTab, codeTab, webTab, contactTab, settingsTab, PPShopContent, TitleShopContent, SkinShopContent;
    public TMPro.TMP_Text txtName, txtLvl, txtTitle;
    public Slider sl;
    public InputField pName, rName;
    public int nb, roomTest;
    public byte rm;
    public KILLER.PlayFabManager pfm;
    public string mapName;
    public Image levelBar, profilePicture;

    // Start is called before the first frame update
    void Start()
    {
        settingsTab.GetComponent<KILLER.Setting>().myCloseEvent.AddListener(SettingsTab);
        pfm = GameObject.Find("PlayFabManager").GetComponent<KILLER.PlayFabManager>();
        pfm.ReadTitleNews(actualiseTitle);
        pfm.GetInventory();
        pfm.GetFriends();
        PhotonNetwork.ConnectUsingSettings("Alpha1.2"); 
        txtName.text = pfm.Player_DisplayName;
        txtLvl.text = "Level: " + pfm.Player_Lvl.ToString();
        levelBar.fillAmount = (float)pfm.Player_Xp / 2000f;
        profilePicture.sprite = Resources.Load<Sprite>("PP/" + pfm.Player_PPID);
        ActualiseTitleText();
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
            content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            foreach (PlayFab.ClientModels.FriendInfo friends in pfm.friendList)
            {
                content.GetComponent<RectTransform>().sizeDelta = new Vector2(content.GetComponent<RectTransform>().sizeDelta.x, content.GetComponent<RectTransform>().sizeDelta.y + 54.56943f);
                print(friends.Profile.LastLogin);
                GameObject go = Instantiate(FriendPref, content.transform);
                Friends.Add(go);
                count++;
                int room = 0;
                int PPID = 0;
                foreach(var item in friends.Profile.Statistics)
                {
                    if (item.Name == "room")
                        room = item.Value;
                    if (item.Name == "PpId")
                        PPID = item.Value;
                }
                go.GetComponent<KILLER.FriendCardScript>().InitializeComponent(room, friends.Profile.DisplayName, friends.Profile.PlayerId, PPID);
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
            pfm.GetCurencies(ActualiseMoneyTxt);
            shopTab.SetActive(true);
        }
    }

    public void ActualiseMoneyTxt()
    {
        money.text = pfm.Player_Gold.ToString() + " Gold";
    }

    public void InventoryTab()
    {
        if (inventoryTab.active == true)
        {
            inventoryTab.SetActive(false);
        }
        else
        {
            inventoryTab.SetActive(true);
        }
    }

    public void InventoryTabSelect(int ID)
    {
        foreach (GameObject go in invWindows)
        {
            go.SetActive(false);
        }
        invWindows[ID].SetActive(true);
    }

    public void CodeTab()
    {
        if (codeTab.active == true)
        {
            codeTab.SetActive(false);
        }
        else
        {
            codeTab.SetActive(true);
        }
    }

    public void UseCode()
    {
        pfm.UseCode(codeTab.GetComponentInChildren<TMPro.TMP_InputField>().text, CodeTab);
    }

    public void ActualiseInventory()
    {
        pfm.GetInventory();
        for (int i = 0; i < PPcontent.transform.childCount; i++)
        {
            Destroy(PPcontent.transform.GetChild(i).gameObject);
        }
        foreach (ItemInstance item in pfm.inv)
        {
            if (item.CatalogVersion == "PP" && item.ItemClass == "PP")
            {
                GameObject go = (Instantiate(ProfilePref, PPcontent.transform));
                go.name = (item.ItemId).ToString();
                go.GetComponent<Image>().sprite = Resources.Load<Sprite>("PP/" + item.ItemId);
            }
        }
        for (int i = 0; i < TitleContent.transform.childCount; i++)
        {
            Destroy(TitleContent.transform.GetChild(i).gameObject);
        }
        foreach (ItemInstance item in pfm.inv)
        {
            if (item.CatalogVersion == "PP" && item.ItemClass == "Title")
            {
                GameObject go = (Instantiate(TitlePref, TitleContent.transform));
                go.name = (item.ItemId).ToString();
                go.GetComponentInChildren<TMPro.TMP_Text>().text = item.ItemId;
            }
        }
        for (int i = 0; i < SkinContent.transform.childCount; i++)
        {
            Destroy(SkinContent.transform.GetChild(i).gameObject);
        }
        foreach (ItemInstance item in pfm.inv)
        {
            if (item.CatalogVersion == "PP" && item.ItemClass == "Skin")
            {
                GameObject go = (Instantiate(SkinPref, SkinContent.transform));
                go.name = (item.ItemId).ToString();
                go.GetComponent<Image>().sprite = Resources.Load<Sprite>("Skin/" + item.ItemId);
            }
        }
    }
    public void AddFriend()
    {
        pfm.ADDFriend();
        FriendsTab();
        FriendsTab();
    }

    public void ActualisePPStore()
    {
        pfm.GetStore("PPS", 0, ShowPPStore, "PP");
    }

    public void ActualiseTitleText()
    {
        txtTitle.text = pfm.keysTab[0];
    }

    public void ShowPPStore()
    {
        for (int i = 0; i < PPShopContent.transform.childCount; i++)
        {
            Destroy(PPShopContent.transform.GetChild(i).gameObject);
        }
        foreach (StoreItem pp in pfm.store[0])
        {
            GameObject go = Instantiate(ShopProfilePref, PPShopContent.transform);
            go.name = pp.ItemId;
            go.GetComponentInChildren<TMPro.TMP_Text>().text = pp.VirtualCurrencyPrices["GO"].ToString() + " Gold";
            go.GetComponent<Image>().sprite = Resources.Load<Sprite>("PP/" + pp.ItemId);
        }
    }

    public void ActualiseTitleStore()
    {
        pfm.GetStore("TitleS", 1, ShowTitleStore, "PP");
    }

    public void ShowTitleStore()
    {
        for (int i = 0; i < TitleShopContent.transform.childCount; i++)
        {
            Destroy(TitleShopContent.transform.GetChild(i).gameObject);
        }
        foreach (StoreItem title in pfm.store[1])
        {
            GameObject go = Instantiate(ShopTitlePref, TitleShopContent.transform);
            go.name = title.ItemId;
            go.GetComponentInChildren<TMPro.TMP_Text>().text = title.ItemId + "   " + title.VirtualCurrencyPrices["GO"].ToString() + " Gold";
        }
    }

    public void ActualiseSkinStore()
    {
        pfm.GetStore("SkinS", 2, ShowSkinStore, "PP");
    }

    public void ShowSkinStore()
    {
        for (int i = 0; i < SkinShopContent.transform.childCount; i++)
        {
            Destroy(SkinShopContent.transform.GetChild(i).gameObject);
        }
        foreach (StoreItem skin in pfm.store[2])
        {
            GameObject go = Instantiate(ShopSkinPref, SkinShopContent.transform);
            go.name = skin.ItemId;
            go.GetComponentInChildren<TMPro.TMP_Text>().text = skin.ItemId + "   " + (skin.VirtualCurrencyPrices["RM"]/100).ToString() + "€";
            go.GetComponent<Image>().sprite = Resources.Load<Sprite>("Skin/" + skin.ItemId);
        }
    }

    public void OpenShopWindows(int ID)
    {
        foreach(GameObject go in ShopWindows)
        {
            go.SetActive(false);
        }
        ShopWindows[ID].SetActive(true);
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
