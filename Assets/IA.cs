using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KILLER
{
	public class IA : MonoBehaviour
	{

		NavMeshAgent agent;
		public bool canMove, dead;
		GameObject target;
		Animator anim;

	    void Start()
	    {
			if (PhotonNetwork.isMasterClient)
			{
				anim = GetComponent<Animator>();
				agent = GetComponent<NavMeshAgent>();
				if (!canMove)
				{
					agent.enabled = false;
				}
			}
	    }

	    void Update()
 	    {
			if (PhotonNetwork.isMasterClient)
			{
				if (!dead)
				{
					if (target == null || (bool)target.GetComponent<PhotonView>().owner.CustomProperties["Dead"] == true)
					{
						int nb = Random.Range(0, 4);
						foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
						{
							if ((bool)go.GetComponent<PhotonView>().owner.CustomProperties["Dead"] == false)
							{
								if (nb == 0)
								{
									target = go;
									break;
								}
								nb--;
							}
						}
					}
					if (canMove)
					{
						agent.SetDestination(target.transform.position);
					}
					else
					{
						var lookPos = target.transform.position;
						lookPos.y = transform.position.y;
						transform.LookAt(lookPos);
					}
				}
			}
	    }
	}
}