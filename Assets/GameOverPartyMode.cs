﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPartyMode : MonoBehaviour {

	public GameObject panel;
	public GameObject[] panelsToHide;
	bool canClick;
	public Text gameOverField;
	public ScoreLine scoreLineToInstatiate;
	public Transform hsicoresContainer;
	bool isOn;

	void Start()
	{
		panel.SetActive (false);
	}
	public void Init()
	{
		if (isOn)
			return;
		isOn = true;

		StartCoroutine (Loop ());
		foreach (GameObject go in panelsToHide)
			go.SetActive (false);

		LoadHiscores ();

		gameOverField.text = Data.Instance.texts.genericTexts.gameOver;
	}
	IEnumerator Loop()
	{
		yield return new WaitForSeconds (2);
		panel.SetActive (true);
		yield return new WaitForSeconds (1);
		canClick = true;
	}
	void Update()
	{
		if (canClick) {
			for (int a = 0; a < 4; a++) {
				if (InputManager.getJump (a))
					OnJoystickClick ();
				if (InputManager.getFireDown (a))
					OnJoystickClick ();
			}
		}
	}
	void OnJoystickClick()
	{
		canClick = false;
		Data.Instance.events.OnResetScores ();
		Data.Instance.inputSavedAutomaticPlay.RemoveAllData ();
		Game.Instance.GotoMainMenu ();	
	}
	void LoadHiscores()
	{
		int num = 1;
		foreach(ArcadeRanking.Hiscore data in Data.Instance.GetComponent<ArcadeRanking>().all)
		{	
			if (num > 10)
				return;
			AddSignal (data, num);
			num++;
		}
	}
	void AddSignal(ArcadeRanking.Hiscore data, int puesto)
	{
		ScoreLine newSignal = Instantiate (scoreLineToInstatiate);
		newSignal.Init (puesto, data.username, data.hiscore);
		newSignal.transform.SetParent (hsicoresContainer);
		newSignal.transform.localScale = Vector3.one;
	}
	void OnDisable()
	{
		StopAllCoroutines ();
	}
}
