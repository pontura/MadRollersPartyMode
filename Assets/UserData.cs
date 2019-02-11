using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class UserData : MonoBehaviour {

    public int userId;
    public int hiscore;
    public string username;
    public string facebookId;
    public string email;
    public string password;

    public int score;
    public int missionScore;
    private Data data;
    public List<int> stars;

	public void Init () {
		return;
//        data = Data.Instance;
//        SocialEvents.OnSetUserData += OnSetUserData;
//      //  data.events.OnHiscore += OnHiscore;
//        data.events.OnListenerDispatcher += OnListenerDispatcher;
//       // data.events.OnScoreOn += OnScoreOn;
//        data.events.OnGameStart += OnGameStart;
//        data.events.OnSetStarsToMission += OnSetStarsToMission;
//        SocialEvents.OnFacebookUserLoaded += OnFacebookUserLoaded;

        //  RESET ID:
        //  PlayerPrefs.SetInt("userId", 0);
        //  PlayerPrefs.SetString("facebookId", "");
       // PlayerPrefs.SetInt("hiscore", 0);

       // if (!Application.isWebPlayer)
//       if(3==2)
//        {
//            Debug.Log("CARGA DATOS de PlayerPrefs");
//            username = PlayerPrefs.GetString("username");
//            userId = PlayerPrefs.GetInt("userId");
//            hiscore = PlayerPrefs.GetInt("hiscore");
//            facebookId = PlayerPrefs.GetString("facebookId");
//        }
//
//        SetStars();
	}
    private int fromWebLogs = 0;
    public void ReceiveFacebookUserNameFromWeb(string _username)
    {
        this.username = _username;
        fromWebLogs++;
        if (fromWebLogs > 2) BothUserIdAndNameFromWeb();
    }
    public void ReceiveFacebookUserIdFromWeb(string _facebookId)
    {
        this.facebookId = _facebookId;
        fromWebLogs++;
        if (fromWebLogs > 2) BothUserIdAndNameFromWeb();
    }
    public void ReceiveFacebookEmailFromWeb(string _email)
    {
        this.email = _email;
        fromWebLogs++;
        if (fromWebLogs > 2) BothUserIdAndNameFromWeb();
    }
    void BothUserIdAndNameFromWeb()
    {
        Social.Instance.dataController.CheckIfFacebookIdExists(facebookId);
    }
    public bool isPlayerDataLogged()
    {
        if (userId > 0 ) return true;
        return false;
    }
    void SetStars()
    {
//		List<Mission> missions = data.GetComponent<Missions>().allMissionsByVideogame[Data.Instance.videogamesData.actualID].missions;
//        int a = 0;
//        foreach (Mission mission in missions)
//        {
//            a++;
//            int _stars = PlayerPrefs.GetInt("stars_level_" + a);
//            stars.Add(_stars);
//        }
    }
    void OnSetUserData(string _username, int _userId, int _hiscore, bool saveIt)
    {
        Debug.Log("OnSetUserData username: " + _username + " id: " + _userId + " hiscore: " + _hiscore + " save: " + saveIt);
        this.username = _username;
        this.userId = _userId;
        this.hiscore = _hiscore;
        if (saveIt)
        {
            PlayerPrefs.SetString("username", _username);
            PlayerPrefs.SetInt("userId", _userId);
            PlayerPrefs.SetInt("hiscore", _hiscore);
        }
    }
    void OnFacebookUserLoaded(string _facebookID, string _username)
    {
        this.username = _username;
        this.facebookId = _facebookID;
        PlayerPrefs.SetString("facebookId", _facebookID);
    }

    void OnSetStarsToMission(int missionId, int starsQty)
    {
//		if (Data.Instance.playMode == Data.PlayModes.COMPETITION) return;
//
//        if (stars[missionId] < starsQty)
//        {
//            PlayerPrefs.SetInt("stars_level_" + missionId, starsQty);
//            stars[missionId] = starsQty;
//        }
    }
    public int GetStars(int MissionID)
    {
        return stars[MissionID];
    }
//    void OnListenerDispatcher(string message)
//    {
//        if (message == "ShowMissionName")
//            missionScore = 0;
//    }
    void OnGameStart()
    {
        score = 0;
    }
    private void OnScoreOn(int playerID, Vector3 pos, int _score)
    {
		
//		int newScore = _score + (data.missions.MissionActiveID * 2);
//        score += newScore;
//        missionScore += newScore;
       // data.events.OnSetFinalScore(pos, score);
    }
    public void OnHiscore(int _hiscore)
    {
        this.hiscore = _hiscore;
        PlayerPrefs.SetInt("hiscore", _hiscore);    
    }
    public void resetProgress()
    {
       // FB.Logout();
        userId = 0;
        facebookId = "";
        username = "";
        int a = 1;
        foreach (int star in stars)
        {
            stars[a-1] = 0;
            a++;
        }
        Data.Instance.LoadLevel("MainMenu");
    }
    public string GetUserNameSmaller(string username)
    {
        string[] userData = Regex.Split(username, "_");
        if (userData.Length > 0)
        {
            if (userData.Length > 1)
                return userData[0] + " " + userData[1];
            return userData[0];
        }
        return username;
    }
}
