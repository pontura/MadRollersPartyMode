﻿using UnityEngine;
using UnityEngine.UI;

public class Continue : MonoBehaviour {

	public GameObject panel;
	private int num = 9;
	public Text countdown_txt;
	public Text credits_txt;
	private float speed = 0.8f;
    private bool canClick;

	void Start () {
		canClick = false;
		panel.SetActive (false);

		if (Data.Instance.playMode == Data.PlayModes.STORYMODE)
			return;
		
		Data.Instance.events.OnGameOver += OnGameOver;

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
	void OnDestroy()
	{
		Data.Instance.events.OnGameOver -= OnGameOver;
	}
	public void OnGameOver(bool isTimeOver)
	{	
		Invoke ("OnGameOverDelayed", 2);
	}	
	public void OnGameOverDelayed()
	{			
		if (!Data.Instance.canContinue || Data.Instance.credits == 0) {
			if (Data.Instance.playMode == Data.PlayModes.PARTYMODE) {
				Invoke ("Done", 2);
			} else {
				canClick = false;
				panel.GetComponent<Animation> ().Play ("signalOff");
				Invoke ("Done", 1f);
			}
			return;
		}		
		credits_txt.text = Data.Instance.credits + " CREDITS"; 
		panel.SetActive (true);
		num = 9;
		countdown_txt.text = num.ToString();
		Invoke ("Loop", 0.5f);
	}	
	public void Loop()
	{
		canClick = true;
		num--;
		if(num<=0)
		{
			canClick = false;
			panel.GetComponent<Animation> ().Play ("signalOff");
			Invoke ("Done", 1f);
			return;
		}
		countdown_txt.text = num.ToString();
		Invoke ("Loop", speed);
	}	
	void Done()
	{
		//GetComponent<SummaryCompetitions> ().SetOn ();
		GetComponent<HiscoresComparison> ().Init ();
		panel.SetActive (false);
	}
	void OnJoystickClick()
	{
		if (canClick) {
			canClick = false;
			CancelInvoke ();
			Game.Instance.Continue();  
		}
	}

}
