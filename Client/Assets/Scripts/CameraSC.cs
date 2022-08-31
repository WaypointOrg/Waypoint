using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSC : MonoBehaviour
{
    [SerializeField] Transform mainPlayer;
    
    [SerializeField] Vector2 mapSize = new Vector2(16, 10);
    [SerializeField] float speed = 1;
    [SerializeField] Rigidbody2D rb;

    Camera cam;
    public static float width;

    void Start()
    {
        width = Camera.main.orthographicSize * Camera.main.aspect * 2;
        cam = this.GetComponent<Camera>();
    }

    public void SetMainPlayer(Transform t)
    {
        mainPlayer = t;
    }

    public void ResetMP()
    {
        mainPlayer = null;
    }

    void Update()
    {
        if(mainPlayer != null)
        {
            float height_ = 2f * cam.orthographicSize;

            Vector3 UpperRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            Vector3 targetPos = new Vector3(mainPlayer.position.x, mainPlayer.position.y, -10);

            float halfcamx = Mathf.Abs(UpperRight.x - transform.position.x);
            float halfcamy = Mathf.Abs(UpperRight.y - transform.position.y);

            if((mainPlayer.position.x + halfcamx) > (mapSize.x))
            {
                targetPos = new Vector3((mapSize.x) - halfcamx, targetPos.y, -10);
            }else if((mainPlayer.position.x - halfcamx) < (- mapSize.x))
            {
                targetPos = new Vector3((- mapSize.x) + halfcamx, targetPos.y, -10);
            }

            if((mainPlayer.position.y + halfcamy) > (mapSize.y))
            {  
                targetPos = new Vector3(targetPos.x, (mapSize.y) - halfcamy, -10);
            }else if((mainPlayer.position.y - halfcamy) < (- mapSize.y))
            {
                targetPos = new Vector3(targetPos.x, (- mapSize.y) + halfcamy, -10);
            }

            Vector2 dist = targetPos - transform.position;
            rb.AddForce(dist * speed);
            
        }else
        {
            transform.position = new Vector3(0,0,-10);
        }
    }
}
