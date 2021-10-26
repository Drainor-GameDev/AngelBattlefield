using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Lobby : MonoBehaviour
{
    public TMPro.TMP_Text newsTitle, newsBody, money, txt;
    public GameObject[] Friends;
    public GameObject tab, content, FriendPref, searchTab, shopTab, webTab, contactTab, settingsTab;
    public Text txtnb, txtroom;
    public Slider sl;
    public InputField pName, rName;
    public int nb, roomTest;
    public byte rm;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("Alpha1.2");
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
        PhotonNetwork.isMessageQueueRunning = false;
        //PhotonNetwork.LoadLevel("Room");
        PhotonNetwork.LoadLevelAsync("Room");
    }
    public void OnPhotonCreateRoomFailed()
    {
        roomTest++;
        CreateRoom(rm);
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
}
