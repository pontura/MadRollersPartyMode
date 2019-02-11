using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class DataController : MonoBehaviour
{
    private string secretKey = "mySecretKey";
    const string URL = "http://www.pontura.com/madrollers/";
    private string getUserIdByFacebookId_URL = URL + "getUserIdByFacebookId.php?";
    private string createUser_URL = URL + "createUser.php?";
    private string addFacebookId = URL + "addFacebookId.php?";
    private string addScore_URL = URL + "setHiscore.php?";
    private string getHighscores_URL = URL + "getHiscores.php?";

    public void Init()
    {
		return;
        SocialEvents.OnCompetitionHiscore += OnCompetitionHiscore;
        SocialEvents.OnGetHiscores += OnGetHiscores;
    }
    void OnCompetitionHiscore(int levelID, int score, bool isNew)
    {
		return;
        int userId = GetComponent<UserData>().userId;
        if (userId < 1)
        {
            Debug.Log("Usuario no registrado como para grabar score");
            return;
        }
        Debug.Log("save hiscore: " + score + " userid: " + GetComponent<UserData>().userId);
       // StartCoroutine(PostScores(GetComponent<UserData>().userId, score));
        StartCoroutine(PostCompetitionScore(GetComponent<UserData>().userId, levelID, score, isNew));
    }
    public void CheckIfFacebookIdExists(string facebookId)
    {
		return;
        StartCoroutine(CheckIfUserExistsOnLocalDB(facebookId));
    }
    //creo el user en la base local:
    IEnumerator CheckIfUserExistsOnLocalDB(string facebookId)
    {
        if (facebookId == "0")
            yield break;
        
        //print("__________sigio: facebookId: " + facebookId);

        string post_url = getUserIdByFacebookId_URL + "facebookId=" + facebookId;
        print("OnFacebook : " + post_url);
        WWW receivedData = new WWW(post_url);
        yield return receivedData;
        if (receivedData.error != null)
            print("There was an error in CheckIfUserExistsOnLocalDB: " + receivedData.error);
        else
        {
            try
            {
                print("recibo CheckIfUserExistsOnLocalDB data: " + receivedData.text);

                string[] userData = Regex.Split(receivedData.text, ":");
                string userName = userData[1];
                int userId = System.Int32.Parse(userData[2]);
                int hiscore = System.Int32.Parse(userData[3]);
                string email = userData[4];

                SetUserData(userName, facebookId, userId, hiscore, email);
                
            }
            catch
            {
                Social.Instance.dataController.CreateUserByFacebookID(facebookId);
                Debug.Log("New user!");
              //  CreateUser(GetComponent<UserData>().username, facebookId, GetComponent<UserData>().hiscore);
            }
        }
    }
    public void CreateUserByFacebookID(string facebookId)
    {
		return;
        UserData userData = GetComponent<UserData>();
        StartCoroutine(CreateUserRoutine(userData.username, facebookId, userData.hiscore, userData.email, userData.password));
    }
    //creo el user en la base local:
    public void CreateUser(string username, string facebookId, int hiscore, string email, string password)
    {
		return;
        StartCoroutine(CreateUserRoutine(username, facebookId, hiscore, email, password));
    }
    IEnumerator CreateUserRoutine(string username, string facebookId, int hiscore, string email, string password)
    {
        username = username.Replace(" ", "_");
        string hash = Md5Test.Md5Sum(username + facebookId + secretKey);
        string post_url = createUser_URL + "facebookId=" + facebookId + "&username=" + username + "&hiscore=" + hiscore.ToString() + "&email=" + email + "&password=" + password + "&hash=" + hash;
        print("CreateUser : " + post_url);
        WWW hs_post = new WWW(post_url);
        yield return hs_post;
        if (hs_post.error != null)
            print("No pudo crear el nuevo user: " + hs_post.error);
        else
        {
            print("user agregado: " + hs_post.text);
            int userId = int.Parse(hs_post.text);
            SetUserData(Data.Instance.userData.username, facebookId, userId, hiscore, email);
        }
    }
    
    void SetUserData(string userName, string facebookID, int userId, int hiscore, string email)
    {
		return;
        SocialEvents.OnSetUserData(userName, userId, hiscore, true);
    }

    IEnumerator PostCompetitionScore(int userId, int levelId, int score, bool isNew)
    {
        Debug.Log("SEt hiscore id: " + userId + " levelId: " + levelId + " score= " + score);
        string hash = Md5Test.Md5Sum(userId.ToString() + levelId.ToString() + score.ToString() + secretKey);
        string post_url = addScore_URL + "id=" + userId.ToString() + "&levelID=" + levelId.ToString() + "&score=" + score + "&new=" + isNew + "&hash=" + hash;
        print("Post Competition Score : " + post_url);
        WWW hs_post = new WWW(post_url);
        yield return hs_post;
        if (hs_post.error != null)
            print("There was an error posting the high score: " + hs_post.error);
        else
            print("SAVED_ " + hs_post.text);
    }
    IEnumerator PostScores(int userId, int hiscore)
    {
        Debug.Log("SEt hiscore id: " + userId + " score= " + hiscore);
        string hash = Md5Test.Md5Sum(userId.ToString() + hiscore.ToString() + secretKey);
        string post_url = addScore_URL + "id=" + userId.ToString() + "&hiscore=" + hiscore + "&hash=" + hash;
        print("PostScores : " + post_url);
        WWW hs_post = new WWW(post_url);
        yield return hs_post;
        if (hs_post.error != null)
            print("There was an error posting the high score: " + hs_post.error);
        else
            print(hs_post.text);
    }

    public void AddFacebookIdToExistingAccount(int userId, string facebookId)
    {
		return;
        print("AddFacebookIdToExistingAccount userId: " + userId + " facebookId " + facebookId);

        StartCoroutine(AddFacebookId(userId, facebookId));
    }
    IEnumerator AddFacebookId(int userId, string facebookId)
    {
        string hash = Md5Test.Md5Sum(userId.ToString() + facebookId + secretKey);
        string post_url = addFacebookId + "facebookId=" + facebookId + "&id=" + userId + "&hash=" + hash;
        print("AddFacebookId : " + post_url);
        WWW hs_post = new WWW(post_url);
        yield return hs_post;
        if (hs_post.error != null)
            Debug.Log("addFacebookID ERROR" + hs_post.error);
        else
        {
            SocialEvents.OnFacebookIdAdded();
        }
    }

    void OnGetHiscores(int levelID)
    {
		return;
        StartCoroutine(GetHiscoresRoutine(levelID));
    }
    IEnumerator GetHiscoresRoutine(int levelID)
    {
        string post_url = getHighscores_URL + "levelID=" + levelID;
        print("GetHiscoresRoutine : " + post_url);
        WWW receivedData = new WWW(post_url);
        yield return receivedData;
        if (receivedData.error != null)
            print("There was an error in getting hiscores: " + receivedData.error);
        else
        {
            SocialEvents.OnHiscoresLoaded(receivedData.text);
        }
    }

    //IEnumerator GetScores()
    //{
    //    WWW receivedData = new WWW(highscore_URL);
    //    yield return receivedData;

    //    if (receivedData.error != null)
    //        print("There was an error getting the high score: " + receivedData.error);
    //    else
    //    {
    //        string[] allData = Regex.Split(receivedData.text, "</n>");

    //        for (var i = 0; i < allData.Length - 1; i++)
    //        {
    //            string[] userData = Regex.Split(allData[i], ":");
    //            string facebookId = userData[0];
    //            string userName = userData[1];
    //            string score = userData[2];
    //        }

    //        SocialEvents.OnHiscoresLoaded();
    //    }

    //}

}

