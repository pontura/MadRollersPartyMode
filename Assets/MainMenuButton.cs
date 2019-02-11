using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MainMenuButton : MonoBehaviour {
	
	public Text[] overs;
	public GameObject selected;
	public Color onColor;
	public Color offColor;

	public void SetOn(bool isOn)
	{
		if (isOn) {
			foreach (Text m in overs)
				m.color = onColor;
			selected.SetActive (true);
		} else {
			foreach (Text m in overs)
			{
				m.color = offColor;
			}
			selected.SetActive (false);
		}
	}
}
