using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickController : MonoBehaviour {

	float lastClickedTime = 0;
	bool processAxis;
	bool isOff;
	float delayToReact = 0.3f;

	public void SetOff()
	{
		isOff = true;
	}
	public void SetOn()
	{
		isOff = false;
	}
	void Update()
	{
		if (isOff)
			return;
		lastClickedTime += Time.deltaTime;
		if (lastClickedTime > delayToReact)
			processAxis = true;
		for (int a = 0; a < 4; a++) {
			if (InputManager.getJumpDown (a)) 
				OnJoystickClick ();
			if (InputManager.getFireDown (a)) 
				OnJoystickClick ();
			if (InputManager.getWeapon (a)) 
				OnJoystickClick ();
			if (InputManager.getDash (a)) 
				OnJoystickClick ();
			if (processAxis) {				
				float v = InputManager.getVertical (a);
				if (v < -0.5f)
					OnJoystickUp ();
				else if (v > 0.5f)
					OnJoystickDown ();
				
				float h = InputManager.getHorizontal (a);
				if (h < -0.5f)
					OnJoystickRight ();
				else if (h > 0.5f)
					OnJoystickLeft ();
			}
		}
	}
	void OnJoystickUp () {
		Data.Instance.events.OnJoystickUp ();
		ResetMove ();
	}
	void OnJoystickDown () {
		Data.Instance.events.OnJoystickDown ();
		ResetMove ();
	}
	void OnJoystickRight () {
		Data.Instance.events.OnJoystickRight ();
		ResetMove ();
	}
	void OnJoystickLeft () {
		Data.Instance.events.OnJoystickLeft ();
		ResetMove ();
	}
	void OnJoystickClick () {
		Data.Instance.events.OnJoystickClick ();
	}
	void OnJoystickBack () {
		Data.Instance.events.OnJoystickBack ();
	}
	void ResetMove()
	{
		processAxis = false;
		lastClickedTime = 0;
	}
}
