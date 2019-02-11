using UnityEngine;
using System.Collections;

public class RegistryScreen : MonoBehaviour {

    void Start()
    {
        SocialEvents.OnFacebookUserLoaded += OnFacebookUserLoaded;
        SocialEvents.OnFacebookNewUserLogged += OnFacebookNewUserLogged;
    }
    void OnDestroy()
    {
        SocialEvents.OnFacebookUserLoaded -= OnFacebookUserLoaded;
        SocialEvents.OnFacebookNewUserLogged -= OnFacebookNewUserLogged;
    }
    void OnFacebookUserLoaded(string id, string username)
    {
        Social.Instance.dataController.CheckIfFacebookIdExists(id);
        // trae: SocialEvents.OnFacebookUserLoaded += OnFacebookNewUserLogged;

        Back();
    }
    void OnFacebookNewUserLogged(string facebookID)
    {
        print("__________OnFacebookNewUserLogged");
       // Social.Instance.dataController.CreateUserByFacebookID(facebookID);
    }
    public void FacebookLogin()
    {
        // Si estas en la web y te llegó el facebook id anteriormente:        
        //  if (Application.isWebPlayer)
        //  Application.ExternalCall("FBLogin");
        // else 
        if (Data.Instance.userData.facebookId != "")
            OnFacebookNewUserLogged(Data.Instance.userData.facebookId);
     //   else
        //    Data.Instance.GetComponentInChildren<FBHolder>().Login();

    }
    public void SimpleLogin()
    {
        Data.Instance.LoadLevel("SimpleLogin");
    }
    public void Back()
    {
        Data.Instance.LoadLevel("MainMenu");
    }
}
