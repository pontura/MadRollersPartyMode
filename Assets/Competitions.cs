using UnityEngine;
using System.Collections;
using System;

public class Competitions : MonoBehaviour {

    public CompetitionData[] competitions;
    public int current = 1;

    [Serializable]
    public class CompetitionData
    {
        public string name;
        public int id;
        //public int myScore;
       // public int levelUnlockedID;
      //  public Mission[] missions;
        
    }
    public void Init()
    {
        SocialEvents.OnMissionReady += OnMissionReady;
    }
//    public Mission[] GetMissions()
//    {
//        return competitions[current - 1].missions;
//    }
    public int GetCurrentCompetition()
    {
        return current;
    }
    //public int GetUnlockedLevel()
    //{
    //    return competitions[current - 1].levelUnlockedID;
    //}
    public void OnMissionReady(int num)
    {
        print("OnMissionReady  " + num);
        //if (competitions[current - 1].levelUnlockedID >= num) return;
        //competitions[current - 1].levelUnlockedID = num;
        //PlayerPrefs.SetInt("levelUnlocked_" + current + "_", num);

        //hack para no jugar el tutorial 2 veces:
       // int storyNum = PlayerPrefs.GetInt("levelUnlocked_0");
      //  if (storyNum < num && num < 5)
        //    PlayerPrefs.SetInt("levelUnlocked_0", num);

    }
}
