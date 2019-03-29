using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WebcamPhoto : MonoBehaviour
{

    public string deviceName;
    WebCamTexture wct;
    WebCamDevice[] devices;
    
    void OnDisable()
    {
        OnUserRegisterCanceled();
    }
    void OnUserRegisterCanceled()
    {
        if(devices == null || devices.Length==0)
            return;
         if (wct != null)
            wct.Stop();
    }
    public void InitWebcam(Image image)
    {
        devices = WebCamTexture.devices;
        if(devices.Length==0)
            return;

        deviceName = devices[devices.Length-1].name;
        wct = new WebCamTexture(deviceName, 300, 300, 12);
        image.material.mainTexture = wct;
        wct.Play();
    }
    public void TakeSnapshot(System.Action OnDone)
    {
        if(devices.Length==0)
            return;

        Texture2D snap = new Texture2D(wct.width, wct.height);
        snap.SetPixels(wct.GetPixels());
        snap.Apply();
        UserData.Instance.sprite = null;
        System.IO.File.WriteAllBytes(UserData.Instance.path + UserData.Instance.userID + ".png", snap.EncodeToPNG());

        if (wct != null)
            wct.Stop();

        UserData.Instance.LoopUntilPhotoIsLoaded(OnDone);

    }
}