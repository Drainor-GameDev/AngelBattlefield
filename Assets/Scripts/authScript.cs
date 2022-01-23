using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

namespace KILLER
{
    public class authScript : MonoBehaviour
    {
        public GameObject SettingUI;
        public string Player_ID;
        string Leveltoload;
        [SerializeField]
        TMPro.TMP_InputField ifDisplayName, ifEmail, ifPassword;
        [SerializeField]
        PlayFabManager playFabmanager;
        // Start is called before the first frame update
        void Start()
        {
            SettingUI.GetComponent<KILLER.Setting>().myCloseEvent.AddListener(CloseSettings);
            playFabmanager = GameObject.Find("PlayFabManager").GetComponent<PlayFabManager>();
        }
        public void CloseSettings()
        {
            if (SettingUI.active)
            {
                SettingUI.SetActive(false);
            }
            else
            {
                SettingUI.SetActive(true);
            }

        }
        public void RegistertPlayer()
        {
            var request = new RegisterPlayFabUserRequest()
            {
                Email = ifEmail.text,
                Password = ifPassword.text,
                DisplayName = ifDisplayName.text,
                Username = ifDisplayName.text
            };
            PlayFabClientAPI.RegisterPlayFabUser(request, Success, Failed);
        }

        private void Failed(PlayFabError err)
        {
            playFabmanager.LoadingMessage(err.ErrorMessage);
            playFabmanager.LoadingHide();
        }

        private void Success(RegisterPlayFabUserResult success)
        {

            playFabmanager.LoadingMessage("Initialize Statistics...");
            InitStat();
        }
        private void InitStat(bool logged = false)
        {
            var request = new UpdatePlayerStatisticsRequest()
            {
                Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate {StatisticName="level", Value=playFabmanager.Player_Lvl}
                }
            };
            if(logged)
                PlayFabClientAPI.UpdatePlayerStatistics(request, InitStatLoggedSuccess, InitStatFailed);
            else
                PlayFabClientAPI.UpdatePlayerStatistics(request, InitStatSuccess, InitStatFailed);

        }

        private void InitStatFailed(PlayFabError err)
        {
            playFabmanager.LoadingMessage(err.ErrorMessage);
            playFabmanager.LoadingHide();
        }

        private void InitStatSuccess(UpdatePlayerStatisticsResult result)
        {
            playFabmanager.LoadingMessage("initstat1succes");
            playFabmanager.LoadingHide();
            InitStat1();
        }
        private void InitStatLoggedSuccess(UpdatePlayerStatisticsResult result)
        {
            playFabmanager.LoadingMessage("initstat1succes");
            playFabmanager.LoadingHide();
            InitStat1(true);
        }

        private void InitStat1(bool logged = false)
        {
            var request = new UpdatePlayerStatisticsRequest()
            {
                Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate {StatisticName="xp", Value=playFabmanager.Player_Xp}
                }
            };
            if(logged)
                PlayFabClientAPI.UpdatePlayerStatistics(request, InitStatLogged1Success, InitStat1Failed);
            else
                PlayFabClientAPI.UpdatePlayerStatistics(request, InitStat1Success, InitStat1Failed);
        }

        private void InitStat1Failed(PlayFabError err)
        {
            playFabmanager.LoadingMessage(err.ErrorMessage);
            playFabmanager.LoadingHide();
        }

        private void InitStat1Success(UpdatePlayerStatisticsResult result)
        {
            playFabmanager.LoadingMessage("initstat2succes");
            playFabmanager.LoadingHide();
            InitStat2();
        }

        private void InitStatLogged1Success(UpdatePlayerStatisticsResult result)
        {
            playFabmanager.LoadingMessage("initstat2succes");
            playFabmanager.LoadingHide();
            InitStat2(true);
        }

        private void InitStat2(bool logged = false)
        {
            var request = new UpdatePlayerStatisticsRequest()
            {
                Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate {StatisticName="room", Value=0}
                }
            };
            if (logged)
                PlayFabClientAPI.UpdatePlayerStatistics(request, InitStatLogged2Success, InitStat2Failed);
            else
                PlayFabClientAPI.UpdatePlayerStatistics(request, InitStat2Success, InitStat2Failed);

        }

        private void InitStat2Failed(PlayFabError err)
        {
            playFabmanager.LoadingMessage(err.ErrorMessage);
            playFabmanager.LoadingHide();
        }

        private void InitStat2Success(UpdatePlayerStatisticsResult result)
        {
            playFabmanager.LoadingMessage("initstat3succes");
            playFabmanager.LoadingHide();
            InitStat3();
        }

        private void InitStatLogged2Success(UpdatePlayerStatisticsResult result)
        {
            playFabmanager.LoadingMessage("initstat3succes");
            playFabmanager.LoadingHide();
            InitStat3(true);
        }

        private void InitStat3(bool logged = false)
        {
            var request = new UpdatePlayerStatisticsRequest()
            {
                Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate {StatisticName="PpId", Value=playFabmanager.Player_PPID}
                }
            };
            if (logged)
                PlayFabClientAPI.UpdatePlayerStatistics(request, InitStatLogged3Success, InitStat3Failed);
            else
                PlayFabClientAPI.UpdatePlayerStatistics(request, InitStat3Success, InitStat3Failed);

        }

        private void InitStat3Failed(PlayFabError err)
        {
            playFabmanager.LoadingMessage(err.ErrorMessage);
            playFabmanager.LoadingHide();
        }

        private void InitStat3Success(UpdatePlayerStatisticsResult result)
        {
            playFabmanager.LoadingMessage("initstat3succes");
            playFabmanager.LoadingHide();
            LoginPlayer();
        }

        private void InitStatLogged3Success(UpdatePlayerStatisticsResult result)
        {
            playFabmanager.LoadingMessage("initstat3succes");
            playFabmanager.LoadingHide();
            LoadGame();
        }

        public void LoginPlayer()
        {
            playFabmanager.LoadingMessage("Connecting server...");
            var request = new LoginWithEmailAddressRequest()
            {
                Password = ifPassword.text,
                Email = ifEmail.text
            };
            PlayFabClientAPI.LoginWithEmailAddress(request, LoginSuccess, LoginFailed);
        }

        private void LoginFailed(PlayFabError err)
        {
            playFabmanager.LoadingMessage(err.ErrorMessage);
            playFabmanager.LoadingHide();
        }

        private void LoginSuccess(LoginResult success)
        {
            playFabmanager.LoadingMessage("Login Successfull");
            playFabmanager.Player_ID = success.PlayFabId;
            playFabmanager.LoadingHide();
            GetPlayerName();

        }
        void GetPlayerName()
        {
            playFabmanager.LoadingMessage("Loading DisplayName...");
            var request = new GetAccountInfoRequest();
            PlayFabClientAPI.GetAccountInfo(request, InfoSuccess, InfoFailed);
        }

        private void InfoFailed(PlayFabError err)
        {
            playFabmanager.LoadingMessage(err.ErrorMessage);
            playFabmanager.LoadingHide();
        }

        private void InfoSuccess(GetAccountInfoResult result)
        {
            playFabmanager.Player_DisplayName = result.AccountInfo.Username;
            //read Statistics score
            ReadStatScore();
        }
        private void ReadStatScore()
        {
            playFabmanager.LoadingMessage("Loading Statistics...");

            var request = new GetPlayerStatisticsRequest();
            PlayFabClientAPI.GetPlayerStatistics(request, SuccessStat, FailedStat);
        }

        private void FailedStat(PlayFabError err)
        {
            playFabmanager.LoadingMessage(err.ErrorMessage);
            playFabmanager.LoadingHide();
        }

        private void SuccessStat(GetPlayerStatisticsResult result)
        {
            foreach (var item in result.Statistics)
            {
                if (item.StatisticName == "xp")
                    playFabmanager.Player_Xp = item.Value;
                if (item.StatisticName == "level")
                    playFabmanager.Player_Lvl = item.Value;
                if (item.StatisticName == "PpId")
                    playFabmanager.Player_PPID = item.Value;
            }
            //playFabmanager.Player_Score = result.Statistics[0].Value;
            playFabmanager.LoadingMessage("Loading Profil SuccessFull...");
            playFabmanager.LoadingHide();
            playFabmanager.GetUserData("Title", 0);
            playFabmanager.GetUserData("ArcherSkin", 5);
            GetBalance();
        }

        void GetBalance()
        {
            var request = new GetUserInventoryRequest();
            PlayFabClientAPI.GetUserInventory(request, InventorySuccess, InventoryFailed);
        }

        private void InventorySuccess(GetUserInventoryResult result)
        {
            foreach (var item in result.VirtualCurrency)
            {
                if (item.Key == "GO")
                {
                    playFabmanager.Player_Gold = item.Value;
                }
            }

            playFabmanager.LoadingMessage("Loading Currency Successfull");
            InitStat(true);
        }
        private void InventoryFailed(PlayFabError err)
        {
            playFabmanager.LoadingMessage(err.ErrorMessage);
            playFabmanager.LoadingHide();
        }
        public void Quit()
        {
            Application.Quit();
        }
        void LoadGame()
        {
            playFabmanager.panel.SetActive(false);
            Leveltoload = "Lobby";
            UnityEngine.SceneManagement.SceneManager.LoadScene(Leveltoload);
        }
        public void OpenUrl(string URL)
        {
            Application.OpenURL(URL);
        }
    }
}
