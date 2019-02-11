using UnityEngine;
using System.Collections;

public class InputMobile : InputType {

    private int id;

    public InputMobile(int playerID)
    {
        this.id = playerID;
    }
    public override bool getStart()
    {
        return Input.GetButton("Start" + id);
    }
    public override bool getOpenMenu()
    {
        return Input.GetButton("MainMenu");
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
        return Input.GetButtonDown("Fire" + id );
    }
	public override bool getFireUp()
	{
		return Input.GetButtonUp("Fire" + id );
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
