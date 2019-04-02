using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LevelSelectorMobile : MonoBehaviour
{
    public Text title;

    public MissionButton diskette1;
    public MissionButton diskette2;
    public MissionButton diskette3;

    VideogameData videogameData;
    public int videgameID;
    //VideogamesUIManager videogameUI;
    bool canInteract;
    float timePassed;
    MissionSelector missionSelector;

    void Start()
    {
        
        missionSelector = GetComponent<MissionSelector>();
        Data.Instance.multiplayerData.ResetAll();
        Data.Instance.events.OnResetScores();

        title.text = "SELECT GAME";
        videgameID = Data.Instance.videogamesData.actualID;
        Data.Instance.voicesManager.PlaySpecificClipFromList(Data.Instance.voicesManager.UIItems, 0);

        InitButton(diskette1, 0);
        InitButton(diskette2, 1);
        InitButton(diskette3, 2);

        Data.Instance.multiplayerData.player1 = true;
       // Data.Instance.isReplay = true;

    }
    void InitButton(MissionButton diskette, int id)
    {
        VideogameData data = Data.Instance.videogamesData.all[id];
        diskette.Init(data);
        diskette.GetComponent<MissionSelector>().LoadVideoGameData(id);    
        diskette.GetHiscore();
    }
    public void Back()
    {
        Data.Instance.videogamesData.actualID = videgameID;
        Data.Instance.LoadLevel("MainMenu");
    }
    void SetSelected()
    {
        List<VoicesManager.VoiceData> list = Data.Instance.voicesManager.videogames_names;
        Data.Instance.voicesManager.PlaySpecificClipFromList(list, videgameID);
        videogameData = Data.Instance.videogamesData.all[videgameID];
        missionSelector.LoadVideoGameData(videgameID);
    }
    public void Go()
    {
        Data.Instance.LoadLevel("MainMenu");
    }
    public void ResetAll()
    {
        PlayerPrefs.DeleteAll();
        Start();
    }
}
