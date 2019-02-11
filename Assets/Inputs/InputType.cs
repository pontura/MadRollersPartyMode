using UnityEngine;
using System.Collections;

public abstract class InputType {
    public abstract bool getStart();
    public abstract float getHorizontal();
	public abstract float getVertical();
    public abstract bool getFireDown();
	public abstract bool getFireUp();
    public abstract bool getJump();
	public abstract bool getJumpUp ();
	public abstract bool getJumpDown ();
    public abstract bool getOpenMenu();
	public abstract bool getDash();
	public abstract bool getWeapon();
	public abstract float getDH();
}
