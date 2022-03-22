using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform gun;
    private void FixedUpdate()
    {
        LookAtMouse();
        SendInputToServer();
    }

    private void LookAtMouse()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        gun.rotation = Quaternion.Euler(0f, 0f, rotation_z);
    }

    private void SendInputToServer()
    {
        bool[] _inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D),
        };

        ClientSend.PlayerMovement(_inputs);
    }
}