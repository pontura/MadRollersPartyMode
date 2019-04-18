using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;


public class MissionButton : MonoBehaviour {

	public Animation anim;
	//public GameObject thumbPanel;

    public Image background;
    public int id;
	public int videoGameID;

	public Image logo;
	public Image floppyCover;

	public VideogameData videogameData;

    public Text missionField;
    public Text usernameField;
    public int missionActive;
    public LevelSelectorMobile levelSelectorMobile;

    // se usa tanto para mobile como para Standalone!

    public void Init (VideogameData videogameData) {
		this.videogameData = videogameData;
		logo.sprite = videogameData.logo;
		floppyCover.sprite = videogameData.floppyCover;
		anim ["MissionButtonOn"].normalizedTime = 0;
		anim.Play ("MissionButtonOn");
        missionField.text = videogameData.name;
        usernameField.text = "MISSION 0";
    }
    public void SetMobile(LevelSelectorMobile levelSelectorMobile)
    {
        this.levelSelectorMobile = levelSelectorMobile;
    }

    // solo version Mobile Android!
    public void OnClick()
    {
		anim.Play ("videoGameButtonMobile");
        Data.Instance.videogamesData.actualID = videogameData.id;
        Data.Instance.missions.MissionActiveID = Data.Instance.missions.GetMissionsByVideoGame(videogameData.id).missionUnblockedID;
        Invoke("DelayedClick", 1);
        levelSelectorMobile.OnMissionButtonClicked(this);
    }

	public void SetOn()
	{
		anim.Play ("MissionTopSetActive");
	}
    public void SetMenuButtonOff()
    {
        anim.Play("MissionButtonOff");
    }
    void DelayedClick()
    {
        Data.Instance.LoadLevel("Game");
    }
    public void GetHiscore()
    {
        missionActive = Data.Instance.missions.GetMissionsByVideoGame(videogameData.id).missionUnblockedID;
        missionField.text = "HiSCORE MISSION " + (missionActive + 1);
        usernameField.text = "<loading...>";
        UserData.Instance.hiscoresByMissions.LoadHiscore(videogameData.id+1, missionActive, OnLoaded);
    }
    void OnLoaded(HiscoresByMissions.MissionHiscoreData data)
    {
        if (data == null || data.all.Count == 0 || !isActiveAndEnabled)
            return;
       
        usernameField.text = data.all[0].username + " - " + Utils.FormatNumbers(data.all[0].score);
    }

}
