using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionButtonMobile : MonoBehaviour
{
    public Color color1;
    public Color color2;
    public Text field;
    public int videoGameID;
    public int missionID;
    public bool isBlocked;
    public Image blockedImage;
    public Image background;
    public MissionSelectorMobile missionSelectorMobile;
   

    public void Init(MissionSelectorMobile missionSelectorMobile, int videoGameID, int missionID, Missions.MissionsData data)
    {
        this.missionSelectorMobile = missionSelectorMobile;
        this.videoGameID = videoGameID;
        this.missionID = missionID;

        if (missionID <= Data.Instance.missions.MissionActiveID)
            isBlocked = false;
        else
            isBlocked = true;

        if (isBlocked)
        {
            blockedImage.enabled = true;
            Vector2 pos = field.transform.localPosition;
            pos.x = 31;
            field.transform.localPosition = pos;
        }
        else
        {
            blockedImage.enabled = false;
        }

        int id = missionID + 1;
        field.text = "MISSION " + id + " " + data.title;

        
    }
    public void SetSelected(bool isSelected)
    {
        if (isSelected)
        { 
            background.color = color1;
            field.color = color2;
        } else
        {
            background.color = color2;
            field.color = color1;
        }
    }
    public void Clicked()
    {
        missionSelectorMobile.Clicked(missionID);
    }
}
