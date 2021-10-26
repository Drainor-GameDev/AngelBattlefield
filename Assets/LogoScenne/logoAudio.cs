using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KILLER
{
	public class logoAudio : MonoBehaviour
	{
	    void Start()
	    {
			StartCoroutine(delay());
			DontDestroyOnLoad(this);
			//SceneManager.sceneLoaded += OnSceneLoaded;
		}

		//void OnSceneLoaded(Scene sc, LoadSceneMode ld)
		//{
		//	if(sc.name != "LogoScreen" && sc.name != "Lobby")
  //          {
		//		Destroy(gameObject);
  //          }
		//}
		public IEnumerator delay()
        {
			yield return new WaitForSeconds(7f);
			SceneManager.LoadSceneAsync("Lobby");
        }
	}
}