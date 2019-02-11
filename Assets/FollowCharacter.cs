using UnityEngine;
using System.Collections;

public class FollowCharacter : MmoCharacter {

	float activaTionDistance = 7;
	float speed = 4;
	CharacterBehavior characterBehavior;
	float realPositionZ = 0;
	CharactersManager charactersManager;

	public override void OnRestart(Vector3 pos)
	{
		charactersManager = Game.Instance.level.charactersManager;
		transform.localEulerAngles = Vector3.zero;
		realPositionZ = 0;
		base.OnRestart(pos);
		Repositionate ();
	}
	void Repositionate()
	{
		CharacterBehavior ch = charactersManager.getMainCharacter ();
		Vector3 myPos = transform.position;
		myPos.z = ch.transform.position.z - activaTionDistance;
		transform.position = myPos;
	}
	void Update()
	{
		if (!isActive)
			return;	
		CharacterBehavior cb = charactersManager.getMainCharacter ();
		if (cb == null)
			return;
		
		realPositionZ +=  (speed * Time.deltaTime);

        Vector3 pos = transform.position;
		pos.z = cb.transform.position.z - activaTionDistance + realPositionZ;
		transform.position = pos;

		if (transform.position.z > cb.transform.position.z + 40)
			Pool ();
    }
}

