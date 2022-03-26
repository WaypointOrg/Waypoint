using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float rotation;

    public Transform gun;
    private void FixedUpdate()
    {
        LookAtMouse();
        SendInputToServer();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClientSend.Shoot();
        }
    }

    private void LookAtMouse()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        rotation = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        gun.rotation = Quaternion.Euler(0f, 0f, rotation);
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

        ClientSend.PlayerMovement(_inputs, rotation);
    }
}