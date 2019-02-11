using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuActions : MonoBehaviour {

    public Text PlayLabel;
    public ProfilePicture WinnerPicture;
    private bool rankingLoaded;

	void Start () {
     //   Data.Instance.events.VoiceFromResources("welcome");
      //  DebugText.text = "username: " + Data.Instance.userData.username + " - id: " + Data.Instance.userData.facebookId + " - id: " + Data.Instance.userData.userId;
      //  Data.Instance.GetComponent<Tracker>().TrackScreen("Main Menu");
      //  Data.Instance.events.OnInterfacesStart();

        if (Application.platform == RuntimePlatform.Android)
            Data.Instance.mode = Data.modes.ACCELEROMETER;
        
    }
    void Update()
    {
        if (rankingLoaded) return;
        if (Social.Instance.hiscores.levels.Count == 0) return;
        if (Social.Instance.hiscores.levels[0].hiscore.Count == 0) return;

        rankingLoaded = true;
        if (Social.Instance.hiscores.levels[0].hiscore[0].profilePicture)
        {
            WinnerPicture.SetLoadedPicture(Social.Instance.hiscores.levels[0].hiscore[0].profilePicture);
        }
        else
        {
            WinnerPicture.SetPicture(Social.Instance.hiscores.levels[0].hiscore[0].facebookID);
        }
    }
    public void Compite()
    {
     //   Data.Instance.playMode = Data.PlayModes.COMPETITION;
        Data.Instance.LoadLevel("Competitions");
    }
    public void MissionsScene()
    {
      //  Data.Instance.playMode = Data.PlayModes.STORY;
        Data.Instance.LoadLevel("LevelSelector");
      //  Data.Instance.levelUnlockedID = PlayerPrefs.GetInt("levelUnlocked_0");
    }
    public void Registry()
    {
        if (Data.Instance.userData.userId == 0)
            Data.Instance.LoadLevel("Registry");
    }
    public void About()
    {
        Data.Instance.LoadLevel("About");
    }
    public void AddFacebookIDToMyAccount()
    {
        if (Data.Instance.GetComponent<UserData>().facebookId == "")
        {
            Data.Instance.LoadLevel("AddFacebookLogin");
        }
    }
    public void Logout()
    {
        //if(FB.IsLoggedIn)
        //    FB.Logout();
        //Data.Instance.LoadLevel("MainMenu");
    }

}
