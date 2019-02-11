﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICountDown : MonoBehaviour {

	public GameObject panel;

	public Text countDownField;
	int countDown = 3;
	bool isOn;

	void Start () {	
		panel.SetActive (false);	
		if (Data.Instance.isReplay)
			return;
		
		Data.Instance.events.OnAddNewPlayer += OnAddNewPlayer;
	}
	void OnDestroy()
	{
		Data.Instance.events.OnAddNewPlayer -= OnAddNewPlayer;
	}
	void OnAddNewPlayer(int id)
	{		
		if (isOn)
			return;
		
		isOn = true;
		panel.SetActive (true);
		Data.Instance.events.OnGameStart ();
		SetNextCountDown ();
	}
	void SetNextCountDown()
	{
		countDownField.text = countDown.ToString ();
		panel.GetComponent<Animation>().Play("logo");
		if (countDown <= 0) {
			Data.Instance.events.StartMultiplayerRace ();
			panel.SetActive (false);
			return;
		}
		countDown--;
		Invoke ("SetNextCountDown", 1f);
	}
}
