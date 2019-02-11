using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NewHiscore : MonoBehaviour {

    public Text NewHiscoreText;

	void Start () {
        NewHiscoreText.text = Data.Instance.userData.hiscore.ToString();
	}
    public void Share()
    {
        Data.Instance.LoadLevel("Game");
    }
    public void Ok()
    {
        Data.Instance.LoadLevel("Game");
    }
    public void MainMenu()
    {
        Data.Instance.LoadLevel("MainMenu");
    }
}
