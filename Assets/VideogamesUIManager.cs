using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideogamesUIManager : MonoBehaviour {

	public float separation;
	public VideogameButton button_to_instantiate;
	public Transform container;
	public List<VideogameButton> all;
	LevelSelector levelSelector;

	public void Init () {
		levelSelector = GetComponent<LevelSelector> ();
		foreach (VideogameData data in Data.Instance.videogamesData.all) {
			VideogameButton button = Instantiate (button_to_instantiate);
			button.transform.SetParent (container);
			button.transform.localPosition = new Vector3 (separation*data.id, 0, 0);
			button.transform.localScale = Vector3.one;
			button.Init (data.logo);
			all.Add (button);
		}
		all [levelSelector.videgameID].SetSelected (true);
	}
	public void Select(int id)
	{
		ResetAll ();
		all [id].SetSelected (true);
	}
	void ResetAll()
	{
		foreach (VideogameButton v in all)
			v.SetSelected (false);
	}
	public void Change()
	{
		ResetAll ();
		all [levelSelector.videgameID].SetSelected (true);
		SelectFirstLevel ();
	}
	void SelectFirstLevel()
	{
		//GetComponent<LevelSelector> ().SelectFirstLevelOf (id);
	}
}
