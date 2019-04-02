using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ArcadeGUI : MonoBehaviour {

   // public int score;
    public bool ended;
   // public MultiplayerUIStatus[] multiplayerUI;
  //  public List<int> multiplayerUI_Y;
    private CharactersManager characterManager;
    public GameObject singleSignal;
    public GameObject singleSignalTexts;
    public List<int> avatarsThatShoot;
	public JoysticksCanvas joysticksCanvas;

	void Start () {
		
        singleSignal.SetActive(false);
        characterManager = Game.Instance.GetComponent<CharactersManager>();
        ended = false;
        
        Data.Instance.events.OnGameOver += OnGameOver;
		SetFields ("");

		if(!Data.Instance.isReplay)
			Invoke ("AllowAddingCharacters", 2);
	}
	void AllowAddingCharacters()
	{
		Game.Instance.state = Game.states.ALLOW_ADDING_CHARACTERS;
	}
    void Update()
    {
		if (Game.Instance.state ==  Game.states.INTRO)
			return;
        if (ended) return;
        if (Data.Instance.isAndroid)
        {
            return;
        }
        if ((InputManager.getFireDown(0) || InputManager.getJump(0)) && joysticksCanvas.CanRevive(0))
        {
            if (!characterManager.existsPlayer(0))
                characterManager.addNewCharacter(0);
        }
		else if ((InputManager.getFireDown(1) || InputManager.getJump(1)) && joysticksCanvas.CanRevive(1))
        {
            if (!characterManager.existsPlayer(1))
                characterManager.addNewCharacter(1);
        }
		else if ((InputManager.getFireDown(2) || InputManager.getJump(2)) && joysticksCanvas.CanRevive(2))
        {
            if (!characterManager.existsPlayer(2))
                characterManager.addNewCharacter(2);
        }
		else if ((InputManager.getFireDown(3) || InputManager.getJump(3)) && joysticksCanvas.CanRevive(3))
        {
            if (!characterManager.existsPlayer(3))
                characterManager.addNewCharacter(3);
        }
    }
    void OnDestroy()
    {
        Data.Instance.events.OnGameOver -= OnGameOver;

    }
    void SetFields(string _text)
    {
        singleSignal.SetActive(true);
        foreach (Text field in singleSignalTexts.GetComponentsInChildren<Text>())
            field.text = _text;
        singleSignal.GetComponent<Animation>().Play("gameOver");
    }
	void OnGameOver(bool isTimeOver)
    {
		Data.Instance.LoseCredit ();
        Data.Instance.multiplayerData.distance = Game.Instance.GetComponent<CharactersManager>().distance;
        
		if (Data.Instance.credits > 0) {

			if (isTimeOver)
				SetFields ("TIME OVER");
			else if (Data.Instance.multiplayerData.GetTotalCharacters () == 1)
				SetFields ("DEAD!");
			else
				SetFields ("ALL DEAD!");

			Invoke("Reset", 2.2f);
		} else {
			SetFields ("GAME OVER");
			Invoke("Reset", 4);
		}

		GetComponent<CreditsUI> ().RemoveOne ();
        ended = true;
        Data.Instance.scoreForArcade = 0;
    }
    void Reset()
    {
		SetFields("");
    }
    

}
