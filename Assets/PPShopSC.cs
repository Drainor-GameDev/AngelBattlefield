using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KILLER
{
	public class PPShopSC : MonoBehaviour
	{
		public void BuyPP()
		{
			int price = 600;
			foreach(StoreItem item in GameObject.Find("PlayFabManager").GetComponent<PlayFabManager>().store)
            {
				if(item.ItemId == gameObject.name)
                {
					price = (int)item.VirtualCurrencyPrices["GO"];
                }
            }
			GameObject.Find("PlayFabManager").GetComponent<PlayFabManager>().MakePurchaseWithVC(gameObject.name,"PP",price);
		}
	}
}