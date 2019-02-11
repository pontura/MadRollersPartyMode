using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataDebug : MonoBehaviour {
	[HideInInspector]
	public bool isArcadeMultiplayer;
	[HideInInspector]
	public bool isDebbug;
	[HideInInspector]
	public int videogameID;
	[HideInInspector]
	public int missionID;
	[HideInInspector]
	public string testArea;

	static LevelDataDebug mInstance = null;

	public static LevelDataDebug Instance
	{
		get
		{
			return mInstance;
		}
	}

	void Awake () {
		mInstance = this;
		DontDestroyOnLoad (this.gameObject);
	}
}
