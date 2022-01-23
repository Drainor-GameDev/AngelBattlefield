using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KILLER
{
	public class TitleShopSC : MonoBehaviour
	{
		public void BuyTitle()
		{
			PlayFabManager pfm = GameObject.Find("PlayFabManager").GetComponent<PlayFabManager>();
			Lobby lobby = GameObject.Find("Lobby").GetComponent<Lobby>();
			int price = 600;
			foreach (StoreItem item in pfm.store[1])
			{
				if (item.ItemId == gameObject.name)
				{
					price = (int)item.VirtualCurrencyPrices["GO"];
				}
			}
			pfm.MakePurchaseWithVC(gameObject.name, "PP", price);
			pfm.GetCurencies(lobby.ActualiseMoneyTxt);
		}
	}
}