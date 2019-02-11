using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SimpleLogin : MonoBehaviour {

    [SerializeField]
    Text nameLabel;
    [SerializeField]
    Text passLabel;
    [SerializeField]
    Text emailField;
    

    void Start()
    {
        SocialEvents.OnSetUserData += OnSetUserData;
    }
    void OnDestroy()
    {
        SocialEvents.OnSetUserData -= OnSetUserData;
    }
    void OnSetUserData(string id, int facebookId, int hiscore, bool saveIt)
    {
        Data.Instance.LoadLevel("MainMenu");
    }
    public void Registry()
    {
        if (nameLabel.text.Length > 1 && passLabel.text.Length > 1)
            RegistryOk();
    }
    void RegistryOk()
    {
        Social.Instance.dataController.CreateUser(nameLabel.text, "0", Data.Instance.userData.hiscore, emailField.text, passLabel.text);        
    }
    public void Back()
    {
        Data.Instance.LoadLevel("Registry");
    }
}
