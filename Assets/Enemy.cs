using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KILLER
{
	public class Enemy : MonoBehaviour
	{
		public int hp;
		Animator anim;

		void Start()
	    {
        	
	    }

	    void Update()
 	    {
			if (PhotonNetwork.isMasterClient)
			{
				if (hp == 0)
				{
					anim.SetBool("Dead", true);
				}
			}
	    }
	}
}