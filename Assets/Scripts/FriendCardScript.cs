using UnityEngine;
using UnityEngine.UI;

namespace KILLER
{
    public class FriendCardScript : MonoBehaviour
    {
        public TMPro.TMP_Text nameText;
        public Image PP;
        public int room;
        public string FriendID;
        public void InitializeComponent(int roomInfo,string name,string ID, int PpId)
        {
            room = roomInfo;
            nameText.text = name ;
            FriendID = ID;
            PP.sprite = Resources.Load<Sprite>("PP/" + PpId);
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
