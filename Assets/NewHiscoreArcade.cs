using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NewHiscoreArcade : MonoBehaviour {

	public GameObject introPanel;
	public GameObject title;
	public GameObject subtitle;
	public GameObject countDown;
    public GameObject scoreField;
    public GameObject flash;

   // public Light lightInScene;

    int newHiscore;

    void Start () {
        flash.SetActive(false);
        scoreField.SetActive(false);
        
        introPanel.SetActive(true);
		Invoke("ResetIntro", 3);

        string actualCompetition = Data.Instance.GetComponent<MultiplayerCompetitionManager>().actualCompetition;

        SetTexts(title, "CAMPEONATO: " + actualCompetition);

        scoreField.SetActive(true);
//        newHiscore = Data.Instance.GetComponent<ArcadeRanking>().newHiscore;
//        SetTexts(scoreField, newHiscore + " PUNTOS");
//        int puesto = 1;
//        foreach (ArcadeRanking.RankingData rd in Data.Instance.GetComponent<ArcadeRanking>().all)
//        {
//            if (newHiscore > rd.score)
//            {
//                SetTexts(subtitle, "PUESTO #" + puesto);
//                return;
//            }
//            puesto++;
//        }
    }
	
	void ResetIntro () {
		introPanel.SetActive(false);
		LoopCountDown();
	}

	int sec = 5;
    int n = 0;
    bool stopLights;
    void Update()
    {
        if (stopLights) return;
        n++;
        if (n > 5)
        {
         //   lightInScene.intensity = (float)Random.Range(70, 100) / 100;
            n = 0;
        }
    }
    void LoopCountDown () {
		if(sec == 0)
		{
			SetTexts(countDown, "");
			TakePhoto();
		} else
		{
			SetTexts(countDown, sec.ToString());
			Invoke("LoopCountDown", 1.5f);
			sec--;
		}
		
	}
    void TakePhoto()
    {
        stopLights = true;
        flash.SetActive(true);
        Invoke("DoIt", 0.1f);
    }
    void DoIt()
    { 
        Data.Instance.events.OnInterfacesStart();        
       // GetComponent<WebCamPhotoCamera>().TakePhoto(Data.Instance.GetComponent<ArcadeRanking>().newHiscore);
        Invoke("Reset", 3);
	}

    void Reset()
    {
        Data.Instance.LoadLevel("MainMenu");
    }

    void SetTexts(GameObject container, string _text)
	{
		foreach(Text field in container.GetComponentsInChildren<Text>() )
		{
			field.text = _text;
		}
	}
}
