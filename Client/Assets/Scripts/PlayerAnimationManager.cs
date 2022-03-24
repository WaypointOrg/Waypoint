using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    // Start is called before the first frame update
    public ExanimationManager animator;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Vertical") > 0)
        {
            //back
            animator.Run("backwards");
        }else
        if(Input.GetAxis("Vertical") < 0)
        {
            //forw
            animator.Run("forward");
        }else
        if(Input.GetAxis("Horizontal") > 0)
        {
            //right
            animator.Run("side");
            if(!spriteRenderer.flipX)
            {
                spriteRenderer.flipX = true;
            }

        }else
        if(Input.GetAxis("Horizontal") < 0)
        {
            //left
            animator.Run("side");
            if(spriteRenderer.flipX)
            {
                spriteRenderer.flipX = false;
            }
        }else
        {
            animator.Run("idle");
        }
    }
}
