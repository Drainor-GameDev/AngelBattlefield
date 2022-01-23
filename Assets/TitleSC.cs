using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KILLER
{
	public class TitleSC : MonoBehaviour
	{
		public void changeTitle()
		{
			PlayFabManager pfm = GameObject.Find("PlayFabManager").GetComponent<PlayFabManager>();
			Lobby lobby = GameObject.Find("Lobby").GetComponent<Lobby>();
			pfm.SetUserData("Title", gameObject.name, 0, lobby.ActualiseTitleText);
		}
	}
}