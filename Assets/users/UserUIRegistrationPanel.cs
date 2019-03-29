using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserUIRegistrationPanel : MonoBehaviour
{
    public Image image;
    public Image imageTaken;
    public InputField field;
    UserDataUI userDataUI;
    public GameObject PhotoPanel;
    public GameObject PhotoTakenPanel;
    public Text buttonField;
    bool userExists;

    public void Init(UserDataUI userDataUI, string _username)
    {
        this.userDataUI = userDataUI;
        field.text = _username;
        if (_username == "")
        {
            buttonField.text = "Registrar";
        }
        else
        {
            userExists = true;
            buttonField.text = "Modificar";
        }
        if (UserData.Instance.sprite == null)
        {
            ShowNewPhoto();
        }
        else
        {            
            ShowPhotoTaken();
        }
    }
    void ShowNewPhoto()
    {
        PhotoPanel.SetActive(true);
        PhotoTakenPanel.SetActive(false);
        userDataUI.webcamPhoto.InitWebcam(image);
    }
    void ShowPhotoTaken()
    {
        PhotoPanel.SetActive(false);
        PhotoTakenPanel.SetActive(true);
        imageTaken.sprite = UserData.Instance.sprite;
    }
    public void TakeSnapshot()
    {
        userDataUI.webcamPhoto.TakeSnapshot(OnPhotoTaken);
    }
    void OnPhotoTaken()
    {
        ShowPhotoTaken();
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
