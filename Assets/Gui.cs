using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Gui : MonoBehaviour {
    
	public LevelComplete levelComplete;

    public GameObject[] hideOnCompetitions;
	private Data data;   

	private int barWidth = 200;
    private bool MainMenuOpened = false;

	public Text genericField;
	public GameObject centerPanel;

	void Start()
	{
		centerPanel.SetActive (false);
//		missionIcon = Instantiate (missionIcon_to_instantiate);
//		missionIcon.transform.localPosition = new Vector3 (1000, 0, 0);

        Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
        Data.Instance.events.OnAvatarFall += OnAvatarCrash;
		Data.Instance.events.OnBossActive += OnBossActive;
		Data.Instance.events.OnGenericUIText += OnGenericUIText;
    }
    void OnDestroy()
    {
        Data.Instance.events.OnAvatarCrash -= OnAvatarCrash;
        Data.Instance.events.OnAvatarFall -= OnAvatarCrash;
		Data.Instance.events.OnBossActive -= OnBossActive;
		Data.Instance.events.OnGenericUIText -= OnGenericUIText;

        levelComplete = null;
    }
	void OnBossActive(bool isOn)
	{
		CancelInvoke ();
		Reset ();
		if (isOn) {
			OnGenericUIText( "Kill 'em all");
		} else {
			levelComplete.gameObject.SetActive (true);
			levelComplete.Init (Data.Instance.missions.MissionActiveID);
		}
		Invoke ("Reset", 2);
	}
	void OnGenericUIText(string text)
	{
		centerPanel.SetActive (true);
		Data.Instance.handWriting.WriteTo(genericField, text, null);
		CancelInvoke ();
		Invoke ("Reset", 2);
	}
	void Reset()
	{
		levelComplete.gameObject.SetActive(false); 
		centerPanel.SetActive (false);
	}
    void OnAvatarCrash(CharacterBehavior cb)
    {
        levelComplete.gameObject.SetActive(false); 
    }
    public void Settings()
    {
        //Data.Instance.GetComponent<GameMenu>().Init();
    }
}
