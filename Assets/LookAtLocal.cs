using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KILLER
{
	public class LookAtLocal : MonoBehaviour
	{
		GameObject pla;
	    void Start()
	    {
			pla = GameObject.Find("local").GetComponent<Movement>().playerCamera;
	    }

	    void Update()
 	    {
			transform.LookAt(pla.transform);
	    }
	}
}