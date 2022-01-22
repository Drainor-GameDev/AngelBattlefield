using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KILLER
{
	public class PPSC : MonoBehaviour
	{
	    public void changePP()
        {
			PlayFabManager pfm = GameObject.Find("PlayFabManager").GetComponent<PlayFabManager>();
			Lobby lobby = GameObject.Find("Lobby").GetComponent<Lobby>();
			pfm.Player_PPID = Int32.Parse(gameObject.name);
			pfm.save();
			lobby.profilePicture.sprite = Resources.Load<Sprite>("PP/" + pfm.Player_PPID);
		}
	}
}