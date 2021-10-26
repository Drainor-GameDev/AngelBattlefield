using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab.ClientModels;
using PlayFab;
using System;


namespace KILLER
{
    public class PlayFabManager : MonoBehaviour
    {
        
        public static PlayFabManager instance = null;
        public GetCharacterLeaderboardResult players;
        public GetTitleNewsResult news;
        TMPro.TMP_Text txtMessage;
        public GameObject panel;
        GameObject player;
        [SerializeField]
        int LoadingTimeOut = 3;

        public string Player_ID, Player_DisplayName;
        public int Player_Lvl, Player_Dollar, Player_GradePoint, Player_Xp;
        public List<ItemInstance> inv;
        void Awake()
        {

            //Singleton
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);


            panel = transform.Find("CanvasLoading").Find("Panel").gameObject;
            txtMessage = panel.GetComponentInChildren<TMPro.TMP_Text>();

        }
        #region UTILS
        public void LoadingHide()
        {
            StartCoroutine(Timer());
        }
        IEnumerator Timer()
        {
            yield return new WaitForSeconds(LoadingTimeOut);
            panel.SetActive(false);
        }
        public void LoadingMessage(string msg)
        {
            LoadingShow();
            txtMessage.text = msg;
        }
        public void LoadingShow()
        {
            if (!panel.activeInHierarchy)
            {
                panel.SetActive(true);
            }
        }
        void DisplayPlayFabError(PlayFabError error)
        {
            LoadingMessage(error.ErrorMessage);
            print(error.ErrorDetails);
        }
        public List<PlayFab.ClientModels.FriendInfo> friendList;
        public void GetFriends()
        {
            PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest
            {
                ProfileConstraints = new PlayerProfileViewConstraints
                {
                    ShowStatistics = true,
                    ShowDisplayName = true
                }
            }, result =>
            {
                friendList = result.Friends;
            },
                DisplayPlayFabError
            );
        }
        public void RemoveFriend(string FriendID)
        {
            PlayFabClientAPI.RemoveFriend(new RemoveFriendRequest
            {
                FriendPlayFabId = FriendID
            }, result =>
            {
            },
                DisplayPlayFabError
            );
            GetFriends();
        }
        public void ADDFriend()
        {

            PlayFabClientAPI.AddFriend(new AddFriendRequest
            {
                FriendUsername = GameObject.Find("FriendSC").GetComponent<TMPro.TMP_InputField>().text
            }, result =>
            {

            },
                DisplayPlayFabError
            ) ;
            GetFriends();
        }
        public void ReportPlayer(string ID)
        {
            PlayFabClientAPI.ReportPlayer(new ReportPlayerClientRequest
            {
                ReporteeId = ID,
                Comment = "Reported"
            }, result =>
            {

            },
                DisplayPlayFabError
            );
        }
        public void ReadTitleNews()
        {
            PlayFabClientAPI.GetTitleNews(new GetTitleNewsRequest(), result => {
                news = result;
                // Process news using result.News
            }, error => Debug.LogError(error.GenerateErrorReport()));
        }
        public void GetInventory()
        {
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), LogSuccess, LogFailure);
        }

        private void LogSuccess(GetUserInventoryResult obj)
        {
            inv = obj.Inventory;
        }

        private void LogFailure(PlayFabError obj)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region SAVE
        public void save()
        {
            //ReadStatScore2();
            //LoadingMessage("save...");
            player = GameObject.Find("Player");
            var request = new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate {StatisticName = "xp", Value = Player_Xp}
                }
            };
            PlayFabClientAPI.UpdatePlayerStatistics(request, Success, Failed);
        }


        private void Success(UpdatePlayerStatisticsResult obj)
        {
            //LoadingHide();
            save1();
        }
        private void save1()
        {
            //LoadingMessage("save xp...");

            var request = new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate {StatisticName = "level", Value = Player_Lvl}
                }
            };
            PlayFabClientAPI.UpdatePlayerStatistics(request, Success1, Failed);
        }


        private void Success1(UpdatePlayerStatisticsResult obj)
        {
            //LoadingHide();
            ReadStatScore();
        }
        public void saveRoom()
        {
            //LoadingMessage("save xp...");

            var request = new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate {StatisticName = "room", Value = Convert.ToInt32(PhotonNetwork.room.Name)}
                }
            };
            PlayFabClientAPI.UpdatePlayerStatistics(request, SuccessRoom, Failed);
        }


        private void SuccessRoom(UpdatePlayerStatisticsResult obj)
        {
            //LoadingHide();
            ReadStatScore();
        }

        public void ReadStatScore()
        {
            var request = new GetPlayerStatisticsRequest();
            PlayFabClientAPI.GetPlayerStatistics(request, SuccessStat, FailedStat);
        }

        private void FailedStat(PlayFabError err)
        {
            LoadingMessage(err.ErrorMessage);
            LoadingHide();
        }
        private void SuccessStat(GetPlayerStatisticsResult result)
        {
            foreach (var item in result.Statistics)
            {
                Player_Xp = result.Statistics[0].Value;
                Player_Lvl = result.Statistics[2].Value;
            }
        }
        private void Failed(PlayFabError err)
        {
            LoadingMessage(err.ErrorMessage);
            LoadingHide();
        }
        #endregion

        private void OnApplicationQuit()
        {
            //LoadingMessage("save xp...");

            var request = new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate {StatisticName = "room", Value = 0}
                }
            };
            PlayFabClientAPI.UpdatePlayerStatistics(request, SuccessRoomQuit, Failed);
        }
        private void SuccessRoomQuit(UpdatePlayerStatisticsResult obj)
        {

        }
        #region PAYPAL
        public void MakePurchase(string ID)
        {
            PlayFabClientAPI.StartPurchase(new StartPurchaseRequest()
            {
                Items = new List<ItemPurchaseRequest>() {
                new ItemPurchaseRequest() {
                    ItemId = ID,
                    Quantity = 1,
                    Annotation = "Purchased via in-game store"
                }
            }
            }, result =>
            {
                
                // Handle success
                ContinuePurchase(result.OrderId);
            }, error =>
            {
                print(error.Error);
                // Handle error
            }); 
        }
        public void ContinuePurchase(string ID)
        {
            PlayFabClientAPI.PayForPurchase(new PayForPurchaseRequest()
            {
                OrderId = ID,
                ProviderName = "PayPal",
                Currency = "RM"
            }, result => {
                Application.OpenURL(result.PurchaseConfirmationPageURL);
                // Handle success
                FinishPurchase(ID);
            }, error => {
                print(error.ErrorMessage);
                // Handle error
            });
        }
        public void FinishPurchase(string ID)
        {
            PlayFabClientAPI.ConfirmPurchase(new ConfirmPurchaseRequest()
            {
                
                OrderId = ID
            }, result => {
                print("BG");
                // Handle success
            }, error => {
                // Handle error
                print(error.Error);
            });
        }
        #endregion
    }



}
