using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float rotation;
    public Transform gun;
    public InputField inputField;

    void Start()
    {
        GameObject _gameobject = GameObject.Find("UsernameTextField");
        inputField = _gameobject.GetComponent<InputField>();
    }

    private void FixedUpdate()
    {
        LookAtMouse();

        if (!inputField.isFocused)
        {
            SendInputToServer();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //StartCoroutine(Camera.main.GetComponent<CameraShake>().Shake(0.1f, 0.5f));
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