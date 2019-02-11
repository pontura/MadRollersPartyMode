using UnityEngine;
using System.Collections;

public class EnemyCar : SceneObject {

    public override void OnRestart(Vector3 pos)
    {
       // base.OnRestart(pos);
      //  SendMessage("OnSceneObjectRestarted", SendMessageOptions.DontRequireReceiver);
    }

	public void Die() {
        //Missions missions = GameObject.Find("data").GetComponent<Missions>();
        //missions.SendMessage ("killCar", 1);
        //Destroy(gameObject);
	}
	void Update()
	{
		if (!isActive)
			return;
      //  SendMessage("OnSceneObjectUpdated", SendMessageOptions.DontRequireReceiver);
    }
}
