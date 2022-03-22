using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
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
            animator.SetInteger("State", 1);
        }else
        if(Input.GetAxis("Vertical") < 0)
        {
            //forw
            animator.SetInteger("State", 0);
        }else
        if(Input.GetAxis("Horizontal") > 0)
        {
            //right
            animator.SetInteger("State", 2);
            if(!spriteRenderer.flipX)
            {
                spriteRenderer.flipX = true;
            }

        }else
        if(Input.GetAxis("Horizontal") < 0)
        {
            //left
            animator.SetInteger("State", 2);
            if(spriteRenderer.flipX)
            {
                spriteRenderer.flipX = false;
            }
        }else
        {
            animator.SetInteger("State", 3);
        }
    }
}
