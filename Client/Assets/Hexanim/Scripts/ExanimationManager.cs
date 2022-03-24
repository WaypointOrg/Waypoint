using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExanimationManager : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public Hexanimation[] clips;

    Hexanimation currentClip;

    public float spacing = 0.1f;

    bool run;

    public void Run(string clipName)
    {   
        if(currentClip != null)
        {
            if(currentClip._name == clipName)
            {
                return;
            }
        }

        foreach (Hexanimation clip in clips)
        {
            if(clip._name == clipName)
            {
                currentClip = clip;
                run = true;
                StartCoroutine(Animate());
                return;
            }
        }

        Debug.LogError("Invoked non-existing clip: " + clipName);
    }

    IEnumerator Animate()
    {
        int nextFrame = 0;

        while(run)
        {
            spriteRenderer.sprite = currentClip.sprites[nextFrame];
            nextFrame = nextFrame + 1;

            if(nextFrame >= (currentClip.sprites.Length - 1))
            {
                if(currentClip.loop)
                {
                    nextFrame = 0;
                }else
                {
                    run = false;
                    break;
                }
            }

            yield return new WaitForSeconds(spacing);
        }
    }

    public void Stop()
    {
        run = false;
    }
}
