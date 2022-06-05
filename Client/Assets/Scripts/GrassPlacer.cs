using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassPlacer : MonoBehaviour
{
    public GameObject grass;
    public int limx;
    public int limy;
    int currentx = 0;
    int currenty = 0;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < limy; i++)
        {
            for(int j = 0; j < limx; j++)
            {
                Instantiate(grass, transform.position + new Vector3(currentx, currenty, 0), Quaternion.identity);
                currentx += 1;
            }

            currenty += 1;
            currentx = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
