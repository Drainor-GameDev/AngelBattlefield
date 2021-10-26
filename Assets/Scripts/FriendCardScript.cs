using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KILLER
{
    public class FriendCardScript : MonoBehaviour
    {
        public TMPro.TMP_Text nameText;
        public int room;
        public string FriendID;
        public void InitializeComponent(int roomInfo,string name,string ID)
        {
            room = roomInfo;
            nameText.text = name ;
            FriendID = ID;
        }
        public void JoinFriends()
        {
            GameObject.Find("Lobby").GetComponent<Lobby>().RejRoom(room);
        }
        public void RemoveFriend()
        {
            GameObject.Find("PlayFabManager").GetComponent<PlayFabManager>().RemoveFriend(FriendID);
            Destroy(gameObject);
        }
        public void Report()
        {
            GameObject.Find("PlayFabManager").GetComponent<PlayFabManager>().ReportPlayer(FriendID);
        }
    }
}
