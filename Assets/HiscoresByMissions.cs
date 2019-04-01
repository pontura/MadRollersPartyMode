using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HiscoresByMissions : MonoBehaviour
{
    private string secretKey = "pontura";
    string saveNewHiscore = "http://madrollers.com/game/saveHiscore.php";
    string getHiscore = "http://madrollers.com/game/getHiscore.php";

    public bool loaded;

    public List<MissionHiscoreData> all;

    [Serializable]
    public class MissionHiscoreData
    {
        public int mission;
        public int videogame;
        public List<MissionHiscoreUserData> all;
    }
    [Serializable]
    public class MissionHiscoreUserData
    {
        [HideInInspector]
        public int mission;
        [HideInInspector]
        public int videogame;
        public string userID;
        public int score;
    }
    public void Init()
    {
        Data.Instance.events.OnMissionComplete += OnMissionComplete;
        LoadHiscore(1, 0, OnHiscoresLoaded);
    }
    private void OnDestroy()
    {
        Data.Instance.events.OnMissionComplete -= OnMissionComplete;
    }
    void OnHiscoresLoaded(string result)
    {
        print("OnHiscoresLoaded " + result);
        all = new List<MissionHiscoreData>();
        MissionHiscoreData missionHiscoreData = JsonUtility.FromJson<MissionHiscoreData>(result);
        missionHiscoreData.mission = missionHiscoreData.all[0].mission;
        missionHiscoreData.videogame = missionHiscoreData.all[0].videogame;
        all.Add(missionHiscoreData);
    }
    void OnMissionComplete(int missionID)
    {
        Save(missionID, Data.Instance.multiplayerData.score);
        Data.Instance.multiplayerData.score = 0;
    }
    public void LoadHiscore(int videogame, int mission, System.Action<string> OnDone)
    {
        string post_url = getHiscore;
        post_url += "?videogame=" + videogame;
        post_url += "&mission=" + mission;
        post_url += "&limit=10";

        StartCoroutine(Send(post_url, OnDone));
    }
    public void Save(int mission, int score)
    {
        int videogame = Data.Instance.videogamesData.actualID;
        string hash = Utils.Md5Sum(UserData.Instance.userID + videogame + mission + score + secretKey);
        string post_url = saveNewHiscore + "?userID=" + WWW.EscapeURL(UserData.Instance.userID);
        post_url += "&username=" + UserData.Instance.username;
        post_url += "&videogame=" + videogame;
        post_url += "&mission=" + mission;
        post_url += "&score=" + score;
        post_url += "&hash=" + hash;

        StartCoroutine( Send(post_url, Saved) );
    }
    void Saved(string result)
    {
        UsersEvents.OnPopup("Saved " + result);
    }
    IEnumerator Send(string post_url, System.Action<string> OnReady)
    {
        print(post_url);
        WWW www = new WWW(post_url);
        yield return www;

        if (www.error != null)
            UsersEvents.OnPopup("Error sending al server: " + www.error);
        else
        {
            OnReady( www.text );
        }
    }
   


}
