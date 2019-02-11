using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour {
	
	void Start () {
		Invoke("Next", 1);
	}
	void Next () {
		#if UNITY_EDITOR
		Data.Instance.LoadLevel("MainMenu");
		#else
//		if(Data.Instance.isArcadeMultiplayer)
//			Data.Instance.LoadLevel("Settings");
//		else
			Data.Instance.LoadLevel("MainMenu");
		#endif
	}
}
