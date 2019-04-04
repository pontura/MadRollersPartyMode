using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserUIRegistrationPanel : MonoBehaviour
{
    public RawImage image;
    public AspectRatioFitter aspectRatioFilter;

    public AvatarThumb avatarThumb;
    public InputField field;
    UserDataUI userDataUI;
    public GameObject PhotoPanel;
    public GameObject PhotoTakenPanel;
    public GameObject[] hideOnScreenshot;
    bool userExists;

    public void Init(UserDataUI userDataUI, string _username)
    {
        PhotoPanel.SetActive(false);
        this.userDataUI = userDataUI;
        field.text = _username;
        if (_username != "")
        {
            field.text = _username;
            userExists = true;
        }
        ShowEditPanel();
    }
    void ShowNewPhoto()
    {
        PhotoPanel.SetActive(true);
        PhotoTakenPanel.SetActive(false);
        userDataUI.webcamPhoto.InitWebcam(image, aspectRatioFilter);
    }
    void ShowEditPanel()
    {
        PhotoPanel.SetActive(false);
        PhotoTakenPanel.SetActive(true);
        avatarThumb.Init( UserData.Instance.userID );
    }
    public void TakeSnapshot()
    {
        UserData.Instance.avatarImages.ResetAvatar(UserData.Instance.userID);
        foreach (GameObject go in hideOnScreenshot)
            go.SetActive(false);
        userDataUI.webcamPhoto.TakeSnapshot(OnPhotoTaken);
    }
    void OnPhotoTaken()
    {
        foreach (GameObject go in hideOnScreenshot)
            go.SetActive(true);

        ShowEditPanel();
        userDataUI.userRegistrationForm.SavePhoto();
       
    }
    public void ClickedNewPhoto()
    {
        ShowNewPhoto();
    }
    public void OnSubmit()
    {
        if (userExists)
        {
            userDataUI.OnUpload(field.text);
        }
        else
        {
         //   if (UserData.Instance.sprite == null)
         //       userDataUI.DebbugText.text = "Falta la foto!";
         //   else 
            if (field.text == "")
                UsersEvents.OnPopup( "Falta un nombre de usuario" );
            else
                userDataUI.OnSubmit(field.text);
        }
    }
}
