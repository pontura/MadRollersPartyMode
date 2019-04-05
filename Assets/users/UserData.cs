using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    public string URL = "http://madrollers.com/game/";
    public string setUserURL = "setUser.php";
    public string setUserURLUpload = "updateUser.php";
    public string imageURLUploader = "uploadPhoto.php";
    public string imagesURL = "users/";

    const string PREFAB_PATH = "UserData";
    static UserData mInstance = null;
    public string userID;
    public string username;
    public Sprite sprite;
    public bool RESET_ALL_DATA;
	public string path;
    public HiscoresByMissions hiscoresByMissions;
    public AvatarImages avatarImages;
    public int playerID;

    public static UserData Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<UserData>();

                if (mInstance == null)
                {
                    return null;
                }
            }
            return mInstance;
        }
    }
    void Awake()
    {
        if (!mInstance)
            mInstance = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this);

        if (RESET_ALL_DATA)
            PlayerPrefs.DeleteAll();

        playerID = PlayerPrefs.GetInt("playerID");

        username = PlayerPrefs.GetString("username");
#if UNITY_EDITOR
        path = Application.persistentDataPath + "/";
#else
        path = Application.persistentDataPath + "/";
#endif

        avatarImages = GetComponent<AvatarImages>();
        hiscoresByMissions = GetComponent<HiscoresByMissions>();
        hiscoresByMissions.Init();    
       
        if (username != "")
        {
            userID = PlayerPrefs.GetString("userID");
            print("userID " + userID);
            avatarImages.GetImageFor(userID, null);
        }
    }
    public bool IsLogged()
    {
        if (userID == "")
            return false;
        return true;
    }
    public void SetUserID(string userID)
    {
        this.userID = userID;
        PlayerPrefs.SetString("userID", userID);
    }

    public void UserCreation()
    {

        PlayerPrefs.SetString("username", username);
        PlayerPrefs.SetString("userID", userID);
    }

	System.Action func;
	public void LoopUntilPhotoIsLoaded(System.Action func)
	{
		this.func = func;
		LoopUntilPhotoIsLoadedLoop();
	}
	public void LoopUntilPhotoIsLoadedLoop()
	{
		Debug.Log("Loading image from local...");
		if(sprite == null)
			Invoke("LoopUntilPhotoIsLoadedLoop", 1);
		else
			func();
		LoadUserPhoto();
	}
    void LoadUserPhoto()
    {
#if UNITY_EDITOR
        sprite = LoadSprite(UserData.Instance.path + UserData.Instance.userID + ".png");
#else
         sprite = LoadSprite(UserData.Instance.userID + ".png");
#endif
    }
    private Sprite LoadSprite(string path)
    {
        Debug.Log("Searching for image in " + path);
        if (string.IsNullOrEmpty(path)) return null;
        if (System.IO.File.Exists(path))
        {
            Debug.Log("Image exists in local");
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(300, 300);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }
        return null;
    }
    public void UpdateData()
    {
        print("UpdateData");
      //  Data.Instance.serverManager.LoadUserData(userID);
    }
}
