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
			GameObject.Find("PlayFabManager").GetComponent<PlayFabManager>().Player_PPID = Int32.Parse(gameObject.name);
			GameObject.Find("PlayFabManager").GetComponent<PlayFabManager>().save();
		}
	}
}