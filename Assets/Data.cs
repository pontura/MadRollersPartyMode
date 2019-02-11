using UnityEngine;
using System.Collections;

public class Data : MonoBehaviour {

	public bool isArcadeMultiplayer;

	public bool DEBUG;
	int forceVideogameID;
	int forceMissionID;
	[HideInInspector]
	public string testAreaName;

	public bool canContinue;
	public int totalCredits;
	public int credits;
	public bool voicesOn;
	public bool soundsFXOn;
	public bool madRollersSoundsOn;
    public bool musicOn;
    public bool switchPlayerInputs;
	public int timeToRespawn;
	public bool isReplay;
	public int totalJoysticks;
	public bool RESET;

    public int competitionID = 1;
    //public bool isArcade;
    

    public int levelUnlocked_level_1 = 0;
	public int levelUnlocked_level_2 = 0;

    public float volume;
    public int scoreForArcade;

	public bool webcamOff;
   // public int WebcamID;

    public UserData userData;
    [HideInInspector]
    public Events events;
    public ObjectPool sceneObjectsPool;
    [HideInInspector]
    public Missions missions;
    [HideInInspector]
    public Competitions competitions;
    [HideInInspector]
    public MultiplayerData multiplayerData;
	[HideInInspector]
	public VideogamesData videogamesData;
	[HideInInspector]
	public InputSaver inputSaver;
	[HideInInspector]
	public InputSavedAutomaticPlay inputSavedAutomaticPlay;
	[HideInInspector]
	public HandWriting handWriting;
	[HideInInspector]
	public Texts texts;

    static Data mInstance = null;

    public modes mode;

	[HideInInspector]
	public bool isEditor;

    public VoicesManager voicesManager;
	public VersusManager versusManager;

	public LoadingAsset loadingAsset;
   
    public int FORCE_LOCAL_SCORE;

    public PlayModes playMode;
    public enum PlayModes
    {
        STORYMODE,
		PARTYMODE,
		VERSUS
	//	GHOSTMODE
    }
    public enum modes
    {
        ACCELEROMETER,
        KEYBOARD,
        JOYSTICK
    }
    public bool hasContinueOnce;

    public static Data Instance
    {
        get
        {
            if (mInstance == null)
            {
                Debug.LogError("Algo llama a DATA antes de inicializarse");
            }
            return mInstance;
        }
    }
	void Awake () {

		if (RESET)
			PlayerPrefs.DeleteAll ();
      //  Cursor.visible = false;

        if (FORCE_LOCAL_SCORE > 0 )
            PlayerPrefs.SetInt("scoreLevel_1", FORCE_LOCAL_SCORE);

        mInstance = this;
		DontDestroyOnLoad(this);
        
		if (LevelDataDebug.Instance) {
			playMode = PlayModes.STORYMODE;
			DEBUG = LevelDataDebug.Instance.isDebbug;
			this.isArcadeMultiplayer = LevelDataDebug.Instance.isArcadeMultiplayer;

			if (isArcadeMultiplayer)
				playMode = PlayModes.PARTYMODE;
			
			this.forceVideogameID = LevelDataDebug.Instance.videogameID;
			this.forceMissionID = LevelDataDebug.Instance.missionID;
			this.testAreaName =  LevelDataDebug.Instance.testArea;
		}

		//setAvatarUpgrades();
       // levelUnlockedID = PlayerPrefs.GetInt("levelUnlocked_0");
        events = GetComponent<Events>();
        missions = GetComponent<Missions>();
		missions.Init ();
        competitions = GetComponent<Competitions>();
        multiplayerData = GetComponent<MultiplayerData>();
		videogamesData = GetComponent<VideogamesData> ();
		inputSaver = GetComponent<InputSaver> ();
		inputSavedAutomaticPlay = GetComponent<InputSavedAutomaticPlay> ();
		versusManager = GetComponent<VersusManager> ();
		handWriting = GetComponent<HandWriting> ();
		texts = GetComponent<Texts> ();

//		if (totalJoysticks > 0)
//			multiplayerData.player1 = true;
//		if (totalJoysticks > 1)
//			multiplayerData.player2 = true;
//		if (totalJoysticks > 2)
//			multiplayerData.player3 = true;
//		if (totalJoysticks > 3)
//			multiplayerData.player4 = true;

       // competitions.Init();
        if(userData)
            userData.Init();
		
        GetComponent<Tracker>().Init();
        GetComponent<CurvedWorldManager>().Init();

       // GetComponent<DataController>().Init();
		//levelUnlocked_level_1 = PlayerPrefs.GetInt("levelUnlocked_level_1");
		//levelUnlocked_level_2 = PlayerPrefs.GetInt("levelUnlocked_level_2");

		levelUnlocked_level_1 = 100;
		levelUnlocked_level_2 = 100;

        voicesManager.Init();

        events.SetVolume += SetVolume;
	}
	void Start()
	{

#if UNITY_EDITOR
		isEditor= true;
#endif
		loadingAsset.SetOn (false);
		GetComponent<PhotosManager>().LoadPhotos();
	}
    void SetVolume(float vol)
    {
        volume = vol;
    }
	public void setMission(int num)
	{
		missions.MissionActiveID = num;
		int idByVideogame = missions.GetActualMissionByVideogame ();
	}
    public void LoadLevel(string levelName)
    {
		Data.Instance.events.ForceFrameRate (1);
		float delay = 0.1f;
		if(DEBUG && forceVideogameID != -1 && forceMissionID != -1 && levelName == "LevelSelector")
		{
			levelName = "Game";
			missions.MissionActiveID = forceMissionID;
			videogamesData.actualID = forceVideogameID;

		}
		if (!isReplay && levelName == "Game") {
			loadingAsset.SetOn (true);
			return;
		}
		GetComponent<Fade> ().LoadLevel (levelName, 1f, Color.black);
	}
	public void LoadingReady()
	{
		loadingAsset.SetOn (false);
	}
	public void LoadLevelNotFading(string levelName)
	{
		Data.Instance.events.ForceFrameRate (1);
		GetComponent<Fade>().LoadSceneNotFading (levelName);
	}
	public void RefreshCredits()
	{
		credits = totalCredits;
	}
	public void LoseCredit()
	{
		credits--;
		if (credits < 1)
			credits = 0;
	}
	public void WinCredit()
	{
		credits++;
	}
}
