using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RankingLine : MonoBehaviour {

    public Text username;
    public Text score;
    public ProfilePicture profilePicture;

    public void Init(string _username, int _score, string _facebookID)
    {
        username.text = _username;
        score.text = _score.ToString() + " Mts.";
        profilePicture.SetPicture(_facebookID);
    }

}
