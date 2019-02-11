﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterControls : MonoBehaviour {

	public bool isAutomata;
    CharacterBehavior characterBehavior;
	public List<CharacterBehavior> childs;
    Player player;
    private float rotationY;
    private float turnSpeed = 2.8f;
    private float speedX = 9f;
    private bool mobileController;
    public bool ControlsEnabled = true;
    private CharactersManager charactersManager;

	float lastKeyPressedTime;
	int lastKeyPressed;

	float jumpingPressedSince;
	float jumpingPressedTime = 0.28f;

	void Start () {
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name != "Game")
			return;
        characterBehavior = GetComponent<CharacterBehavior>();
        player = GetComponent<Player>();
        charactersManager = Game.Instance.GetComponent<CharactersManager>();
	}
	public void EnabledMovements(bool enabledControls)
    {
		ControlsEnabled = enabledControls;
    }
	public void AddNewChild(CharacterBehavior child)
	{
		childs.Add (child);
	}
	public void RemoveChild(CharacterBehavior child)
	{
		childs.Remove (child);
	}
	float lastDH;
	void LateUpdate () {
		if (characterBehavior == null || characterBehavior.player == null)
			return;
		if (characterBehavior.state == CharacterBehavior.states.CRASH || characterBehavior.state == CharacterBehavior.states.DEAD) 
			return;
		if (Time.deltaTime == 0) return;


		if (lastDH != InputManager.getDH (player.id)) {
			lastDH = InputManager.getDH (player.id);
			characterBehavior.characterMovement.DH (-lastDH);
		}
		
		if (InputManager.getWeapon(player.id))
			characterBehavior.shooter.ChangeNextWeapon ();

		if (InputManager.getFireDown(player.id))
			characterBehavior.shooter.CheckFire ();
		
		if (InputManager.getDash (player.id)) {
			characterBehavior.characterMovement.DashForward ();
		}
	

		if (
			characterBehavior.state == CharacterBehavior.states.RUN
		) {
			
			if (InputManager.getJumpDown (player.id)) {
				jumpingPressedSince = 0;
			}
			if (InputManager.getJump (player.id)) {
				jumpingPressedSince += Time.deltaTime;
				if (jumpingPressedSince > jumpingPressedTime)
					Jump ();
				else
					characterBehavior.JumpingPressed ();
			} else if (InputManager.getJumpUp (player.id)) {
				Jump ();
			}
		} else if (InputManager.getJumpDown (player.id)) {
			Jump ();
		}
		
		moveByKeyboard();

		if (characterBehavior.player.charactersManager == null || characterBehavior.player.charactersManager.gameOver)
			return;
		characterBehavior.UpdateByController(rotationY); 
	}
	void Jump()
	{
		jumpingPressedSince = 0;
		characterBehavior.Jump ();
	}  
	float lastHorizontalKeyPressed;
	float last_x;
	float last_x_timer;
    private void moveByKeyboard()
    {
		if (characterBehavior.player.charactersManager == null)
			return;
		
		if (characterBehavior.player.charactersManager.distance<12)
			return;
		float _speed = InputManager.getHorizontal(player.id);

		if ( _speed>0 && lastHorizontalKeyPressed <=0
			|| _speed<0 && lastHorizontalKeyPressed >=0
			|| _speed==0 && (lastHorizontalKeyPressed <0  ||  lastHorizontalKeyPressed >0)
		) {			
			lastHorizontalKeyPressed = _speed;
			if(!isAutomata)
				Data.Instance.inputSaver.MoveInX (transform.position.x);
		}

		MoveInX (_speed);
    }
	bool playerPlayed;
	public void MoveInX(float _speed)
	{
		
		if (_speed < -0.5f || _speed > 0.5f) {
			if (!playerPlayed) {
				playerPlayed = true;
				Data.Instance.multiplayerData.PlayerPlayed (characterBehavior.player.id);
			}
			float newPosX = _speed*speedX;
			float newRot = turnSpeed * ( Time.deltaTime * 35);
			if (newPosX > 0)
				rotationY += newRot;
			else if (newPosX < 0)
				rotationY -= newRot;
			else if (rotationY > 0)
				rotationY -= newRot;
			else if (rotationY < 0)
				rotationY += newRot;
		} else{
			rotationY = 0;
		}

		if (rotationY > 30) rotationY = 30;
		else if (rotationY < -30) rotationY = -30;

		if (Time.deltaTime == 0) return;

		characterBehavior.SetRotation (rotationY);

		//if (childs.Count > 0)
		//	UpdateChilds ();
	}

	//childs:
	IEnumerator ChildsJump()
	{
		if(childs == null || childs.Count>0)
			yield return null;
		foreach (CharacterBehavior cb in childs) {
			yield return new WaitForSeconds (0.18f);
			cb.Jump ();
		}
		yield return null;
	}
//	void UpdateChilds()
//	{
//		foreach (CharacterBehavior cb in childs) {
//			cb.controls.rotationY = rotationY / 1.5f;
//			cb.transform.localRotation = transform.localRotation;
//		}
//	}
//












//	/// <summary>
//	/// For mobile
//	/// </summary>
//	private void moveByAccelerometer()
//	{
//		if (Input.touchCount > 0)
//		{
//			var touch = Input.touches[0];
//			if (touch.position.x < Screen.width / 2)
//			{
//				if (Input.GetTouch(0).phase == TouchPhase.Began)
//					characterBehavior.Jump();
//			}
//			else if (touch.position.x > Screen.width / 2)
//			{
//				characterBehavior.shooter.CheckFire();
//			}
//		} else
//		{
//			characterBehavior.AllButtonsReleased();
//		} 
//
//
//		if (Time.deltaTime == 0) return;
//		transform.localRotation = Quaternion.Euler(transform.localRotation.x, Input.acceleration.x * 50, rotationZ);
//
//		// transform.Translate(0, 0, Time.deltaTime * characterBehavior.speed);
//
//	}
}
