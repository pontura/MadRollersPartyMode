using UnityEngine;
using System.Collections;

public class Social : MonoBehaviour {

    
    static Social mInstance = null;

    [HideInInspector]
    public DataController dataController;
    [HideInInspector]
    public Hiscores hiscores;

    public static Social Instance
    {
        get
        {
            if (mInstance == null)
            {
                Debug.LogError("Algo llama a Social antes de inicializarse");
            }
            return mInstance;
        }
    }
    void Start()
    {
        mInstance = this;
        DontDestroyOnLoad(this);
        dataController = GetComponent<DataController>();
        hiscores = GetComponent<Hiscores>();
        dataController.Init();

      //  SocialEvents.OnGetHiscores(1);
    }
}
