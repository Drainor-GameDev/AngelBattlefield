using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KILLER
{
	public class SkinSC : MonoBehaviour
	{
		public string Champ = "ArcherSkin";
		public void changeSkin()
		{
			PlayFabManager pfm = GameObject.Find("PlayFabManager").GetComponent<PlayFabManager>();
			Lobby lobby = GameObject.Find("Lobby").GetComponent<Lobby>();
			pfm.SetUserData(Champ, gameObject.name, 0, lobby.ActualiseTitleText);
		}
	}
}