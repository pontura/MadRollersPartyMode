using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

public class Hiscores : MonoBehaviour {

    public List<LevelHiscore> levels;

    [Serializable]
    public class LevelHiscore
    {
        [SerializeField]
        public List<Hiscore> hiscore;
        public int id;
        public int myScore;
    }

    [Serializable]
    public class Hiscore
    {
        [SerializeField]
        public int id;
        public string username;
        public string facebookID;
        public int score;
        public Texture2D profilePicture;
    }

    void Start()
    {
//        
//        SocialEvents.OnFinalDistance += OnFinalDistance;
//        SocialEvents.OnFacebookImageLoaded += OnFacebookImageLoaded;
//        SocialEvents.LoadMyHiscoreIfNotExistesInRanking += LoadMyHiscoreIfNotExistesInRanking;
//        loadLocalSavedScores();
    }
    public Texture2D GetPicture(string facebookID)
    {
		
        foreach (Hiscore hiscore in levels[0].hiscore)
        {
            if (facebookID == hiscore.facebookID && hiscore.profilePicture)
                return hiscore.profilePicture;
        }
        return null;
    }
    void OnFacebookImageLoaded(string facebookID, Texture2D texture2d)
    {
		return;
        foreach(Hiscore hiscore in levels[0].hiscore)
        {
            if (facebookID == hiscore.facebookID)
                hiscore.profilePicture = texture2d;
        }
    }
    void loadLocalSavedScores()
    {
		return;
        for (int a = 1; a < levels.Count+1; a++)
        {
            int myScore = PlayerPrefs.GetInt("scoreLevel_" + a);
            levels[a-1].myScore = myScore;
        }
    }
    void OnFinalDistance(float score)
    {
		return;
      //  if (Data.Instance.playMode == Data.PlayModes.STORY) return;
//        int competitionID = Data.Instance.competitions.GetCurrentCompetition();
//        checkToSaveHiscore(competitionID, score);
    }
    void checkToSaveHiscore(int competitionID, float score)
    {
		return;
        Debug.Log("Check to save HISCORE: competitionID: " + competitionID + " oldScore: " + levels[competitionID - 1].myScore + " new score: " + score);
        if (levels[competitionID - 1].myScore >= score) return;

        bool isNew = false;
        if (levels[competitionID - 1].myScore == 0)
            isNew = true;

        levels[competitionID - 1].myScore = (int)score;

        PlayerPrefs.SetInt("scoreLevel_" + competitionID, (int)score);
        SocialEvents.OnCompetitionHiscore(competitionID, (int)score, isNew);
    }
    void OnHiscoresLoaded(string receivedData)
    {
		return;
        levels[0].hiscore.Clear();
        Debug.Log("Sores Init: " + receivedData);

        string[] allData = Regex.Split(receivedData, "</n>");

        for (var i = 0; i < allData.Length - 1; i++)
        {
            string[] userData = Regex.Split(allData[i], ":");
            Hiscore hiscore = new Hiscore();
            hiscore.id = int.Parse(userData[0]);
            hiscore.score = int.Parse(userData[1]);
            hiscore.facebookID = userData[2];
            hiscore.username = Data.Instance.userData.GetUserNameSmaller(userData[3]);
            levels[0].hiscore.Add(hiscore);

            //por las dudas uqe habia otro score grabado local
            if (hiscore.facebookID == Data.Instance.userData.facebookId)
            {
                Debug.Log(":______________ hiscore.facebookID " + hiscore.facebookID + " " + Data.Instance.userData.facebookId + " score: " + hiscore.score);
                levels[0].myScore = hiscore.score;
            }
        }

        //por ahora lo hago aca...
        if (levels[0].myScore > 0)
        {
            LoadMyHiscoreIfNotExistesInRanking(Data.Instance.userData.facebookId, levels[0].myScore);
        }
    }
    void LoadMyHiscoreIfNotExistesInRanking(string facebookID, int score)
    {
		return;
        bool exists = false;
        foreach (Hiscore hiscore in levels[0].hiscore)
        {
            if (facebookID == hiscore.facebookID)
                exists = true;
        }
        if (!exists)
        {
            Hiscore hiscore = new Hiscore();
            hiscore.id = Data.Instance.userData.userId;
            hiscore.score = score;
            hiscore.facebookID = facebookID;
            hiscore.username = "Yo";
            levels[0].hiscore.Add(hiscore);
        }
        ArrengeHiscoresByScore();
    }
    public void ArrengeHiscoresByScore()
    {
		return;
        levels[0].hiscore = levels[0].hiscore.OrderBy(x => x.score).ToList();
        levels[0].hiscore.Reverse();
    }
    public Hiscore GetMyNextGoal()
    {
        string facebookID = Data.Instance.userData.facebookId;

        if (levels[0].hiscore.Count == 0) return null;

        Hiscore lastHiscore = levels[0].hiscore[0];

        foreach (Hiscore hiscore in levels[0].hiscore)
        {
            if (levels[0].myScore > hiscore.score || facebookID == hiscore.facebookID)
                return lastHiscore;
            lastHiscore = hiscore;
        }
        return lastHiscore;
    }
    //mientras corres le ganas a un contrincante y te graba tu score provisorio
    public void SetMyScoreWhenPlaying(int newScore)
    {
		return;
        print("SetMyScoreWhenPlaying newScore: " + newScore + " oldScore: " + levels[0].myScore);
        foreach (Hiscore hiscore in levels[0].hiscore)
        {
            if (Data.Instance.userData.facebookId == hiscore.facebookID)
                hiscore.score = newScore;
        }
        levels[0].myScore = newScore;
        ArrengeHiscoresByScore();
    }
    public int GetMyScore()
    {
        return levels[0].myScore;
    }
    public void Reset()
    {
        levels[0].myScore = 0;
    }
}
