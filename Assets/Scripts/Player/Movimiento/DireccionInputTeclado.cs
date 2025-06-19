using UnityEngine;

public class DireccionInputTeclado : MonoBehaviour, IDireccionInput
{
    public Vector2 ObtenerDireccion()
    {
        if (Input.GetKey(KeyCode.W)) return Vector2.up;
        if (Input.GetKey(KeyCode.S)) return Vector2.down;
        if (Input.GetKey(KeyCode.D)) return Vector2.right;
        if (Input.GetKey(KeyCode.A)) return Vector2.left;
        return Vector2.zero;
    }
}
