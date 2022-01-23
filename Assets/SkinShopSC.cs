using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KILLER
{
	public class SkinShopSC : MonoBehaviour
	{
		public void BuySkin()
		{
			PlayFabManager pfm = GameObject.Find("PlayFabManager").GetComponent<PlayFabManager>();
			Lobby lobby = GameObject.Find("Lobby").GetComponent<Lobby>();
			pfm.MakePurchase(gameObject.name);
		}
	}
}