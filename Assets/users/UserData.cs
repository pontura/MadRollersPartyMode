using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    const string PREFAB_PATH = "UserData";
    static UserData mInstance = null;
    public string userID;
    public string username;
    public Sprite sprite;
    public bool RESET_ALL_DATA;
	public string path;
	
    public static UserData Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<UserData>();

                if (mInstance == null)
                {
                    GameObject go = Instantiate(Resources.Load<GameObject>(PREFAB_PATH)) as GameObject;
                    mInstance = go.GetComponent<UserData>();
                }
            }
            return mInstance;
        }
    }
    void Awake()
    {
        mInstance = this;
		
        if (RESET_ALL_DATA)
            PlayerPrefs.DeleteAll();

		path = Application.persistentDataPath + "/";

        username = PlayerPrefs.GetString("username");
        if(username != "")
            userID = PlayerPrefs.GetString("userID");

        DontDestroyOnLoad(this);
        LoadUserPhoto();
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
        sprite = LoadSprite(Application.persistentDataPath + "/" + UserData.Instance.userID + ".png");
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
