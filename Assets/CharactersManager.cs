﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharactersManager : MonoBehaviour {

    public CharacterBehavior character;
    public List<CharacterBehavior> characters;
    public List<CharacterBehavior> deadCharacters;

	public Vector3 characterPosition = new Vector3(0,0,0);

    private float separationX  = 2;

    public float distance;
    private float speedRun = 19;
    private Missions missions;
    public List<int> playerPositions;
	public bool gameOver;
    private IEnumerator RalentaCoroutine;
	int totalCharacters;

    void Awake()
    {
		distance = 0;
    }
    void Start()
    {
        missions = Data.Instance.GetComponent<Missions>();

    }
	bool freezed;
	void FreezeCharacters(bool _freezed)
	{
		foreach (CharacterBehavior cb in characters) {
			if (!cb.player.IsDebbugerPlayer ()) {
				cb.GetComponent<Rigidbody> ().useGravity = !_freezed;
			}
		}
		freezed = _freezed;
	}
    void StartMultiplayerRace()
    {
		if (Data.Instance.isReplay) {
			//Data.Instance.isReplay = false;
		} else {
			speedRun = 19;
		}
    }
	void LateUpdate()
    {
		if (freezed)
			return;
		OnUpdate ();
		if(Input.GetKeyDown(KeyCode.M))
			AddChildPlayer( getMainCharacter() );
		
		if (Game.Instance.level.waitingToStart) return;
        if (gameOver) return;

        distance += speedRun * Time.deltaTime;

		
    }
	public virtual void OnUpdate(){ }
    public int GetPositionByID(int _playerID)
    {
        int position = 0;
        foreach(int playerID in playerPositions)
        {
            if(playerID == _playerID)
                return position;
            position++;
        }
        return position;
    }
    public virtual void Init()
    {
		Data.Instance.inputSavedAutomaticPlay.Init (this);
        Data.Instance.events.OnAlignAllCharacters += OnAlignAllCharacters;
        Data.Instance.events.OnReorderAvatarsByPosition += OnReorderAvatarsByPosition;
        Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
        Data.Instance.events.OnAvatarFall += OnAvatarFall;
        Data.Instance.events.StartMultiplayerRace += StartMultiplayerRace;
		Data.Instance.events.OnAutomataCharacterDie += OnAutomataCharacterDie;
		Data.Instance.events.FreezeCharacters += FreezeCharacters;

		StartCoroutine (AddCharactersInitials ());
    }
	IEnumerator AddCharactersInitials()
	{
		Vector3 pos;

		float _y = 4;

		if (Data.Instance.isReplay)
			_y = 15;

		pos = new Vector3(0, _y, 0);

		int positionID = 0;

		totalCharacters = Data.Instance.multiplayerData.GetTotalCharacters ();
		if (totalCharacters == 0)
			yield return null;
		
		if (Data.Instance.multiplayerData.player1) { addCharacter(CalculateInitialPosition(pos, positionID), 0); playerPositions.Add(0); };
		float timeToAppear = 0.08f;
		yield return new WaitForSeconds (timeToAppear);
		if (Data.Instance.multiplayerData.player2) { addCharacter(CalculateInitialPosition(pos, positionID+1), 1); playerPositions.Add(1); };
		yield return new WaitForSeconds (timeToAppear);
		if (Data.Instance.multiplayerData.player3) { addCharacter(CalculateInitialPosition(pos, positionID+2), 2); playerPositions.Add(2); };
		yield return new WaitForSeconds (timeToAppear);
		if (Data.Instance.multiplayerData.player4) { addCharacter(CalculateInitialPosition(pos, positionID+3), 3); playerPositions.Add(3); };

	}
    void OnDestroy()
    {
		Data.Instance.events.OnAvatarCrash -= OnAvatarCrash;
        Data.Instance.events.OnAvatarCrash -= OnAvatarCrash;
        Data.Instance.events.OnAvatarFall -= OnAvatarFall;
        Data.Instance.events.OnReorderAvatarsByPosition -= OnReorderAvatarsByPosition;
        Data.Instance.events.StartMultiplayerRace -= StartMultiplayerRace;
        Data.Instance.events.OnAlignAllCharacters -= OnAlignAllCharacters;
		Data.Instance.events.OnAutomataCharacterDie -= OnAutomataCharacterDie;
		Data.Instance.events.FreezeCharacters -= FreezeCharacters;
    }
	void OnAutomataCharacterDie(CharacterBehavior automataCharacter)
	{
		CharacterBehavior parentCharacter = null;
		CharacterBehavior childCharacter = null;
		foreach (CharacterBehavior cb1 in characters) {
			foreach (CharacterBehavior cb2 in cb1.controls.childs) {
				if (cb2 == automataCharacter) {					
					parentCharacter = cb1;
					childCharacter = cb2; 
					parentCharacter.controls.StopAllCoroutines ();
					childCharacter.GetComponent<CharacterAutomata> ().Reset ();
				}					
			}
		}
		if(childCharacter != null)
			parentCharacter.controls.RemoveChild (childCharacter);
		
		deadCharacters.Remove (automataCharacter);
	}
    void OnReorderAvatarsByPosition(List<int> playerPositions)
    {
        this.playerPositions = playerPositions;
    }
	public void OnAvatarFall(CharacterBehavior characterBehavior)
    {
        killCharacter(characterBehavior);
    }
    public void OnAvatarCrash(CharacterBehavior characterBehavior)
    {
        killCharacter(characterBehavior);
    }
    public bool existsPlayer(int id)
    {
        bool exists = false;
        characters.ForEach((cb) =>
        {
            if (cb.player.id == id) exists = true;
        });
        return exists;
    }
    public void addNewCharacter(int id)
    {
		if (characters.Count == 0 && Game.Instance.gameCamera.state != GameCamera.states.WAITING_TO_TRAVEL)
			return;

		Data.Instance.multiplayerData.AddNewCharacter (id);
		
        Data.Instance.events.OnSoundFX("coin", id);
        

		Vector3 pos = Vector3.zero;

		if(characters.Count >0)
			pos = characters[0].transform.position;
		
        pos.y += 3;
        pos.x = 0;

		if(distance<20)
			pos.x = (3.5f * id) - (5.3f);
		
        addCharacter(pos, id);

		Data.Instance.events.ForceFrameRate(1);
    }
	public CharacterBehavior addCharacter(Vector3 pos, int id)
	{
		Data.Instance.events.OnAddNewPlayer(id);
		CharacterBehavior newCharacter = null;
		foreach (CharacterBehavior cb in deadCharacters)
		{
			if (cb.player.id == id)
			{
				newCharacter = cb;
			}
		}
		if (newCharacter == null)
			newCharacter = Instantiate(character, Vector3.zero, Quaternion.identity) as CharacterBehavior;
		else
			deadCharacters.Remove(newCharacter);

		Player player = newCharacter.GetComponent<Player> ();
		player.Init(id);
		player.SetInvensible (3);
		player.id = id;
		newCharacter.Revive();
		characters.Add(newCharacter);
		newCharacter.transform.position = pos;
		Data.Instance.events.OnCharacterInit (id);

		return newCharacter;
	}
	int automaticIdPosition = 0;
	public CharacterBehavior AddAutomaticPlayer(int id)
	{
		CharacterBehavior newCharacter = addCharacter(CalculateInitialPosition(new Vector3(1,1,1), automaticIdPosition), id);
		newCharacter.controls.isAutomata = true;
		automaticIdPosition++;
		return newCharacter;
	}
	public CharacterBehavior AddChildPlayer(CharacterBehavior parentPlayer)
	{
		int id = parentPlayer.controls.childs.Count + 4;
		CharacterBehavior newCharacter = addCharacter(parentPlayer.transform.position, id);
		newCharacter.controls.isAutomata = true;
		parentPlayer.controls.AddNewChild( newCharacter );
		newCharacter.GetComponent<CharacterAutomata> ().Init ();
		return newCharacter;
	}
	float separationOnReplay = 1f;
	Vector3 CalculateInitialPosition(Vector3 pos, int positionID)
	{		
		float _x;
		if (Data.Instance.isReplay)
			_x = ((float)positionID * separationOnReplay)  - (((((float)totalCharacters-1))/2)*separationOnReplay);
		else
			_x = (3.5f * positionID+1) - (5.3f);

		//print ("positionID : " + positionID + "   separationOnReplay: " + separationOnReplay  + "    CalculateInitialPosition " + _x + "  totalCharacters " + totalCharacters);

		return new Vector3(_x,pos.y);
	}

	public void KillAllCharacters()
	{
		foreach (CharacterBehavior cb in characters)
			cb.Die ();
	}
    public void killCharacter(CharacterBehavior characterBehavior)
    {
		if (gameOver)
			return;
        characters.ForEach((cb) =>
        {
            if (cb.player.id == characterBehavior.player.id)
            {
                characters.Remove(cb);
                deadCharacters.Add(cb);
                if (characters.Count == 0)
                {
                    StartCoroutine(restart(cb));
                }
            }
        });
        Data.Instance.events.OnAvatarDie(characterBehavior);
    }
    IEnumerator restart(CharacterBehavior cb)
    {
		Data.Instance.events.OnCameraChroma (CameraChromaManager.types.RED);
		Data.Instance.events.OnSoundFX("dead", -1);

        gameOver = true;
        yield return new WaitForSeconds(0.05f);
        Data.Instance.events.OnGameOver(false);
        yield return new WaitForSeconds(1.32f);
    }
    public CharacterBehavior getMainCharacter()
    {
        if (characters.Count <= 0)
        {
            print("[ERROR] No hay más characters y sigue pidiendo...");
            return null;
        }
        return characters[0];
    }
    public Vector3 getPositionMainCharacter()
    {
        return getMainCharacter().transform.position;
    }
	public virtual Vector3 getCameraPosition()
    {		
		///////retomar
		if (characters.Count > 1) {
			Vector3 normalPosition = Vector3.zero;
			float MaxDistance = 0;
			foreach (CharacterBehavior cb in characters) {
				normalPosition += cb.transform.localPosition;
			}

			normalPosition /= characters.Count;
			normalPosition.y += 0.15f + (MaxDistance / 4f);
			normalPosition.z -= 0.3f + (MaxDistance/26);
			normalPosition.z = distance-1.5f;

			return normalPosition;
		} else if (characters.Count == 0)
			return characterPosition;
		//else return characterPosition = characters[0].transform.position;
		else {
			Vector3 p = characters [0].transform.position;
			p.z = distance-0.5f;
			return p;
		}
    }
    public int getTotalCharacters()
    {
        return characters.Count;
    }
    public float getDistance()
    {
        return distance;
    }
    void OnAlignAllCharacters()
    {
        foreach (CharacterBehavior cb in characters)
        {
            Vector3 pos = cb.transform.localPosition;
            pos.x = 0;
            pos.y = 1;
            cb.transform.localPosition = pos;
        }
    }
	public void ResetJumps(){
		foreach (CharacterBehavior cb in characters) {
			cb.ResetJump ();
		}
	}
	public void OnLevelComplete()
	{
		foreach (CharacterBehavior cb in characters) {
			//cb.SuperJump (2200);
			cb.player.SetInvensible (6);
		}
	}
	public virtual Vector3 getPositionByTeam(int id) {
		return Vector3.zero;
	}
}
