using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MissionButton : MonoBehaviour {

	public Animation anim;
	//public GameObject thumbPanel;

    public Image background;
    public int id;
	public int id_in_videogame;
	public int videoGameID;

	public Image logo;
	public Image floppyCover;

	public VideogameData videogameData;


    // se usa tanto para mobile como para Standalone!

	public void Init (VideogameData videogameData) {
		this.videogameData = videogameData;
		logo.sprite = videogameData.logo;
		floppyCover.sprite = videogameData.floppyCover;
		anim ["MissionButtonOn"].normalizedTime = 0;
		anim.Play ("MissionButtonOn");
	}

    // solo version Mobile Android!
    public void OnClick()
    {
		anim.Play ("videoGameButtonMobile");
        Data.Instance.videogamesData.actualID = videogameData.id;
        Data.Instance.missions.MissionActiveID = Data.Instance.missions.GetMissionsByVideoGame(videogameData.id).missionUnblockedID;
        Invoke("DelayedClick", 0.5f);
    }

	public void SetOn()
	{
		anim.Play ("MissionTopSetActive");
	}
    void DelayedClick()
    {
        Data.Instance.LoadLevel("Game");
    }
}
