using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LagCompTest : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] Transform target;
    [SerializeField] bool follow = false;
    [SerializeField] float speed;
    Vector2 lastFramePos;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(follow)
        {
            Vector2 dPos = new Vector2(lastFramePos.x - transform.position.x, lastFramePos.y - transform.position.y);
            Debug.Log(dPos);

            //if(dist.magnitude > 3)
            Vector2 dist = new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y);
            rb.AddForce(dist * speed);
            

            lastFramePos = new Vector2(transform.position.x, transform.position.y);
        }
    }
}
