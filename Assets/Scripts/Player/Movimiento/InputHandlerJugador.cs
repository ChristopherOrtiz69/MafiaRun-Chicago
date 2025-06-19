using UnityEngine;

public class InputHandlerJugador : MonoBehaviour, IInputHandler
{
    public float GetHorizontal()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    public bool GetJump()
    {
        return Input.GetButtonDown("Jump");
    }

    public bool GetDropDown()
    {
        return Input.GetKeyDown(KeyCode.S) || Input.GetAxisRaw("Vertical") < -0.1f;
    }
}
