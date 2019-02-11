﻿using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Missions : MonoBehaviour {
	
	public TextAsset _all;
	public MissionsListInVideoGame all;

	public int times_trying_same_mission;

	public List<MissionsByVideoGame> videogames;
	[Serializable]
	public class MissionsListInVideoGame
	{
		public string[] missionsVideoGame1;
		public string[] missionsVideoGame2;
		public string[] missionsVideoGame3;
	}
	[Serializable]
	public class MissionsByVideoGame
	{
		public List<MissionsData> missions;
		public int missionUnblockedID;
	}
	[Serializable]
	public class MissionsData
	{
		public string title;
		public List<MissionData> data;
	}

	public int MissionActiveID = 0;

	[HideInInspector]
	public MissionData MissionActive;
	private float missionCompletedPercent = 0;

	private Level level;
	private bool showStartArea;
	private Data data;
	float distance;

	public AreaData areaDataActive;
	float areasLength;
	int offset = 120;
	int areaSetId = 0;
	int areaNum = 0;
	int areaID = 0;

	VideogamesData videogamesData;

	public void Init()
	{	
		videogamesData = GetComponent<VideogamesData> ();
		data = Data.Instance;

		//if (Data.Instance.DEBUG)
			LoadAll();
		
		data.events.OnMissionComplete += OnMissionComplete;
	}
	public void LoadAll()
	{
		all = JsonUtility.FromJson<MissionsListInVideoGame> (_all.text);

		LoadByVideogame (all.missionsVideoGame1, 0);
		LoadByVideogame (all.missionsVideoGame2, 1);
		LoadByVideogame (all.missionsVideoGame3, 2);
	}
	public string LoadResourceTextfile(string path)
	{
		string filePath = "missions/" + path.Replace(".json", "");
		TextAsset targetFile = Resources.Load<TextAsset>(filePath);
		return targetFile.text;
	}
	public void LoadByVideogame(string[] missionsInVideogame, int videogameID)
	{
		
		MissionsByVideoGame videogame = videogames [videogameID];
		videogame.missions = new List<MissionsData> ();	
		foreach (string missionName in missionsInVideogame) {	
			string dataAsJson = LoadResourceTextfile (missionName);
			MissionsData missionData = JsonUtility.FromJson<MissionsData> (dataAsJson);
			missionData.data [0].jsonName = missionName;
			videogame.missions.Add (missionData);
			videogame.missionUnblockedID = PlayerPrefs.GetInt ("missionUnblockedID_" + (videogameID + 1), 0);
		}
	}
	public MissionData GetMissionsDataByJsonName(string jsonName)
	{
		Debug.Log (jsonName);
		foreach (MissionsByVideoGame mvv in videogames) {
			foreach (MissionsData mmData in mvv.missions)  {
				foreach (MissionData mData in mmData.data)  {
					if (mData.jsonName == jsonName)
						return mData;
				}
			}
		}
		return null;
	}
	public void Init (Level level) {
		this.level = level;
		areasLength = -4;
		StartNewMission ();
		if (!Data.Instance.isReplay) {
			if (!Data.Instance.DEBUG && Data.Instance.playMode == Data.PlayModes.PARTYMODE)
				ShuffleMissions ();
			AddAreaByName ("start_Multiplayer");
		} else {
			AddAreaByName ("continue_Multiplayer");
		}
	}
	void ShuffleMissions()
	{		
	//	Debug.Log ("______ShuffleMissions");
		foreach (MissionsByVideoGame mbv in videogames) {		
			for (int a = 0; a < 50; a++) {	
				int rand = UnityEngine.Random.Range (2, mbv.missions.Count - 1);
				MissionsData randomMission1 = mbv.missions [1];
				MissionsData randomMission2 = mbv.missions [rand];

				mbv.missions [rand] = randomMission1;
				mbv.missions [1] = randomMission2;
			}
		}
	}
	void OnMissionComplete(int id)
	{
		times_trying_same_mission = 0;
		if (Data.Instance.playMode == Data.PlayModes.PARTYMODE) {
			AddAreaByName ("areaChangeLevel");
			return;
		} else if (MissionActiveID >= videogames [videogamesData.actualID].missions.Count - 1) {
			Game.Instance.GotoVideogameComplete ();
		} else {
			NextMission ();
			int videogameID = videogamesData.actualID+1;
			PlayerPrefs.SetInt ("missionUnblockedID_" + videogameID, MissionActiveID);
		}
		videogames [videogamesData.actualID].missionUnblockedID = MissionActiveID;
	}
	public int GetTotalMissionsInVideoGame(int videogameID)
	{
		return videogames [videogameID].missions.Count;
	}
	public MissionsByVideoGame GetMissionsByVideoGame(int videogameID)
	{
		return videogames [videogameID];
	}
	void NextMission()
	{
		AddAreaByName ("newLevel_playing");
		
		MissionActiveID++;
		StartNewMission ();
		Data.Instance.events.OnChangeBackgroundSide (MissionActive.fondo);
	}
	void StartNewMission()
	{
		areaSetId = 0;
		ResetAreaSet ();

		//HACK
		if (Data.Instance.playMode == Data.PlayModes.PARTYMODE && MissionActiveID >= videogames[videogamesData.actualID].missions.Count-1)
			MissionActiveID = 1;
		else
			MissionActive = videogames[videogamesData.actualID].missions[MissionActiveID].data[0];
		this.missionCompletedPercent = 0;
	}
	public MissionData GetActualMissionData()
	{
		return videogames[videogamesData.actualID].missions[MissionActiveID].data[0];
	}
	public MissionData GetMission(int videoGameID, int missionID)
	{
		return videogames[videoGameID].missions[missionID].data[0];
	}
	public int GetActualMissionByVideogame()
	{
		int viedogameActive = videogamesData.actualID;
		int id = 0;
		foreach (MissionData mission in videogames[viedogameActive].missions[0].data) {
			if (mission.id == MissionActive.id)
				return id;
			id++;
		}
		return 0;
	}
	public void OnUpdateDistance(float distance)
	{
		if (distance > areasLength-offset) {
			SetNextArea ();
		}
		if (MissionActiveID == 0)
			CheckTutorial (distance);
	}
	int total_areas = 1;
	void SetNextArea()
	{
		CreateCurrentArea ();
		Game.Instance.gameCamera.SetOrientation (MissionActive.areaSetData [areaSetId].cameraOrientation);
		total_areas = MissionActive.areaSetData [areaSetId].total_areas;

	//	if (MissionActive.areaSetData [areaSetId].randomize && Game.Instance.level.charactersManager.getTotalCharacters()==1) 
		//	total_areas /= 1.25f;
		
		if (areaNum >= total_areas) {
			if (areaSetId < MissionActive.areaSetData.Count - 1) {
				areaSetId++;
				ResetAreaSet ();
			} else {
				areaNum--;
			}
		}
		areaNum++;
	}
	void ResetAreaSet()
	{
		areaNum = 0;
		areaID = 0;
	}
	private void  CreateCurrentArea()
	{
		MissionData.AreaSetData areaSetData = MissionActive.areaSetData[areaSetId];
		string areaName = GetArea(areaSetData);

		//DEBUG:::::
		if(Data.Instance.testAreaName != "")
			AddAreaByName (Data.Instance.testAreaName);
		else
			AddAreaByName (areaName);
		
	}
	void AddAreaByName(string areaName)
	{
		TextAsset asset = Resources.Load ("areas/" + areaName ) as TextAsset;
		if (asset != null) {					
			areaDataActive = JsonUtility.FromJson<AreaData> (asset.text);
			areasLength += areaDataActive.z_length/2;
			level.sceneObjects.AddSceneObjects (areaDataActive, areasLength);
			//print ("km: " + areasLength + " mission: " + MissionActiveID +  " areaSetId: " + areaSetId + " areaID: " + areaID + " z_length: " + areaDataActive.z_length + " en: areas/" + areaName +  " totalAreas" + total_areas );
			areasLength += areaDataActive.z_length/2;
		} else {
			Debug.LogError ("Loco, no existe esta area: " + areaName + " en Respurces/areas/");
		}

	}
	List<MissionData.AreaSetData> GetActualAreaSetData()
	{
		return MissionActive.areaSetData;
	}
	string GetArea(MissionData.AreaSetData areaSetData)
	{
		if (areaSetData.randomize) {
			areaID++;
			return areaSetData.areas [UnityEngine.Random.Range(0,areaSetData.areas.Count)];
		} else if (areaID < areaSetData.areas.Count - 1) {
			areaID++;
			return areaSetData.areas [areaID-1];
		} else {
			return areaSetData.areas [areaSetData.areas.Count-1];
		}
	}

	int tutorialID = 0;
	void CheckTutorial(float distance)
	{
		if(tutorialID >= 3 || Data.Instance.playMode == Data.PlayModes.VERSUS )
			return;

		if (distance>142 && tutorialID < 1)
		{
			Data.Instance.voicesManager.PlayClip (Data.Instance.voicesManager.tutorials [0].audioClip);
			tutorialID = 1;
		} else if(distance>200 && tutorialID < 2)
		{
			Data.Instance.voicesManager.PlayClip (Data.Instance.voicesManager.tutorials [1].audioClip);
			tutorialID = 2;
		} else if(distance>305 && tutorialID < 3)
		{
			Data.Instance.voicesManager.PlayClip (Data.Instance.voicesManager.tutorials [2].audioClip);
			tutorialID = 3;
		}
	}
}
