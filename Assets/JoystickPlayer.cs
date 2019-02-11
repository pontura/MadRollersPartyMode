﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickPlayer : MonoBehaviour {

	public int playerID;
	public GameObject joystick;
	public GameObject button1;
	public GameObject button2;
	public GameObject insertCoin;
	public GameObject dead;
	public Image deadFill;
	public Text puesto;
	public Text field;
	Animation anim;
	public states state;
	public Image playerImageColor;


	public enum states
	{
		INSERT_COIN,
		PLAYING,
		DEAD,
		GAME_OVER
	}
	void Start()
	{
		timeToRespawn = Data.Instance.timeToRespawn;
		anim = GetComponent<Animation> ();
		RefreshStates ();
		Color color = Data.Instance.multiplayerData.colors [playerID];
		deadFill.color = color;
		puesto.color = color;
		field.color = color;
		playerImageColor.color = color;
	}
	public void SetFields(int scorePosition, string text)
	{		
		anim.Play ("on");
		puesto.text = scorePosition.ToString();
		field.text = text;
	}
	public void OnGameOver(bool isTimeOver)
	{
		SetHorizontal (0);
		state = states.GAME_OVER;
		dead.SetActive (false);
		insertCoin.SetActive (false);
	
		int score_player = Data.Instance.multiplayerData.GetScore(playerID);

		int total = Data.Instance.multiplayerData.GetTotalScore();

		if (score_player > 0) {
			Vector2 pos =  transform.localPosition;
			int positionByScore = Data.Instance.multiplayerData.GetPositionByScore (playerID);
			pos.y -= positionByScore * 2;
			transform.localPosition = pos;
			int perc = score_player * 100 / total;
			SetFields (positionByScore, perc.ToString () + "%");
		}
	}
	public void RefreshStates() {	
		if (Data.Instance.playMode == Data.PlayModes.VERSUS) {

			dead.SetActive (false);
			insertCoin.SetActive (false);
			gameObject.SetActive (false);

			if (Data.Instance.multiplayerData.player1 && playerID == 0)
				gameObject.SetActive (true);
			if (Data.Instance.multiplayerData.player2 && playerID == 1)
				gameObject.SetActive (true);
			if (Data.Instance.multiplayerData.player3 && playerID == 2)
				gameObject.SetActive (true);
			if (Data.Instance.multiplayerData.player4 && playerID == 3)
				gameObject.SetActive (true);	
		} else {
			if (playerID == 0) {
				if(Data.Instance.multiplayerData.player1)
					SetState (states.PLAYING);
				else
					SetState (states.INSERT_COIN);
			} else if (playerID == 1) {
				if(Data.Instance.multiplayerData.player2)
					SetState (states.PLAYING);
				else
					SetState (states.INSERT_COIN);
			} else if (playerID == 2) {
				if(Data.Instance.multiplayerData.player3)
					SetState (states.PLAYING);
				else
					SetState (states.INSERT_COIN);
			} else if (playerID == 3) {
				if(Data.Instance.multiplayerData.player4)
					SetState (states.PLAYING);
				else
					SetState (states.INSERT_COIN);
			}
		}
	}
	public void SetState(states _state)
	{
		this.state = _state;
		switch (state) {
		case states.INSERT_COIN:
			dead.SetActive (false);
			insertCoin.SetActive (true);
			break;
		case states.DEAD:
			fillAmount = 0;
			dead.SetActive (true);
			insertCoin.SetActive (false);
			break;
		case states.PLAYING:
			dead.SetActive (false);
			insertCoin.SetActive (false);
			break;
		}
	}

	void Update () {
		if (state == states.GAME_OVER)
			return;
		if (InputManager.getFireDown (playerID))
			PressButton (button1);
		else if (button1.transform.localPosition.y < 0) {
			Vector2 pos = button1.transform.localPosition;
			pos.y += 0.1f;
			button1.transform.localPosition = pos;
		}
		
		if (InputManager.getJump(playerID))
			PressButton(button2);
		else if (button2.transform.localPosition.y < 0) {
			Vector2 pos = button2.transform.localPosition;
			pos.y += 0.1f;
			button2.transform.localPosition = pos;
		}

		float h = InputManager.getHorizontal (playerID);

		SetHorizontal (h);

		if (state == states.DEAD && Data.Instance.playMode != Data.PlayModes.VERSUS) {
			if(Input.GetButtonDown("Jump"+(playerID+1)) || Input.GetButtonDown("Fire"+(playerID+1)))
			{
				fillAmount += 0.015f;
				if (deadFill.fillAmount > 1)
					deadFill.fillAmount = 1;
				dead.GetComponent<Animation> () ["deadAndClicked"].normalizedTime = 0;
				dead.GetComponent<Animation> ().Play ();
			}
			FillDead ();
		}
		
	}
	float lastValue;
	void SetHorizontal(float value)
	{
		if (value == lastValue)
			return;
		lastValue = value;
		Vector3 pos = joystick.transform.localEulerAngles;
		pos.z = value*-15;
		joystick.transform.localEulerAngles = pos;
	}
	void PressButton(GameObject button)
	{
		if (button.transform.localPosition.y == -1.5f)
			return;
		Vector2 pos = button.transform.localPosition;
		pos.y = -1.5f;
		button.transform.localPosition = pos;
	}
	void ResetButton(GameObject button)
	{
		if (button.transform.localPosition.y == 0)
			return;

		Vector2 pos = button.transform.localPosition;
		pos.y = 0;
		button.transform.localPosition = pos;		
	}
	float fillAmount;
	float timeToRespawn;
	void FillDead()
	{
		deadFill.fillAmount = fillAmount;
		fillAmount += Time.deltaTime / timeToRespawn;
		if (fillAmount > 1)
			SetState (states.INSERT_COIN);
	}
}
