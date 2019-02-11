using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProfilePicture : MonoBehaviour {

   public void Init(Hiscores.Hiscore data)
    {
		return;
        SetPicture(data.facebookID);
    }
   public void SetPicture(string facebookID)
   {
		return;
       Texture2D texture2d = Social.Instance.hiscores.GetPicture(facebookID);
       if (texture2d)
           SetLoadedPicture(texture2d);
       else
            StartCoroutine(GetPicture(facebookID));
   }
   public void SetLoadedPicture(Texture2D texture2d)
   {
		return;
       GetComponent<Image>().sprite = Sprite.Create(texture2d, new Rect(0, 0, 128, 128), Vector2.zero);
   }
   IEnumerator GetPicture(string facebookID)
    {
        if (facebookID == "") 
            yield break;

       // print("Busca " + facebookID);

        WWW receivedData = new WWW("https" + "://graph.facebook.com/" + facebookID + "/picture?width=128&height=128");
        yield return receivedData;
        if (receivedData.error == null)
        {
            SetLoadedPicture(receivedData.texture);
            SocialEvents.OnFacebookImageLoaded(facebookID, receivedData.texture);
        }
        else
        {
            Debug.Log("ERROR trayendo imagen");
        }
    }
}
