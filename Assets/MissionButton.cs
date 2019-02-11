using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MissionButton : MonoBehaviour {

	public Animation anim;
	//public GameObject thumbPanel;

    public Image background;
	public GameObject lockImage;
    public int id;
	public int id_in_videogame;
	public int videoGameID;
	public bool isLocked;

	public Image logo;
	public Image floppyCover;

	public VideogameData videogameData;

	public void Init (VideogameData videogameData) {
		this.videogameData = videogameData;
		logo.sprite = videogameData.logo;
		floppyCover.sprite = videogameData.floppyCover;
		anim ["MissionButtonOn"].normalizedTime = 0;
		anim.Play ("MissionButtonOn");
	}
    public void OnClick()
    {
		anim.Play ("MissionTopSetActive");
    }
	public void SetOn()
	{
		anim.Play ("MissionTopSetActive");
	}
}
