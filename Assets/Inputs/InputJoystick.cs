using UnityEngine;
using System.Collections;

public class InputJoystick : InputType {

    private int id;


    public InputJoystick(int playerID)
    {
        this.id = playerID+1;
    }
    public override bool getOpenMenu()
    {
        return Input.GetButton("MainMenu");
    }
    public override bool getStart()
    {
        return Input.GetButton("Fire" + id);
    }
	public override float getVertical()
	{
		return Input.GetAxisRaw("Vertical" + id );
	}
    public override float getHorizontal()
    {
        return Input.GetAxisRaw("Horizontal" + id );
    }
    public override bool getFireDown()
    {
		return Input.GetButtonDown("FireDown" + id );
    }
	public override bool getFireUp()
	{
		return Input.GetButtonUp("FireUp" + id );
	}
    public override bool getJump()
    {
        return Input.GetButton("Jump" + id );
    }

	public override bool getJumpDown()
	{
		return Input.GetButtonDown("Jump" + id);
	}
	public override bool getJumpUp()
	{
		return Input.GetButtonUp("Jump" + id);
	}
	public override bool getDash()
	{
		return Input.GetButtonDown("Dash" + id );
	}
	public override bool getWeapon()
	{
		return Input.GetButtonDown("Weapon" + id );
	}
	public override float getDH()
	{
		return Input.GetAxisRaw("DH" + id );
	}
}
