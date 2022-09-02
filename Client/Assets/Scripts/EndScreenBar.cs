using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenBar : MonoBehaviour
{
    public Transform topTransform;
    public TextMesh scoreText;
    public TextMesh usernameText;
    public Transform rectangleTransform;
    public float targetHeight;
    public float startHeight;
    public float animationSpeed;
    private float halfHeight;
    public bool isBest;
    public GameObject confettisPrefab;


    void Start()
    {
        halfHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
        rectangleTransform.transform.position = new Vector3(rectangleTransform.transform.position.x, -halfHeight, 0);
        rectangleTransform.transform.localScale = new Vector3(rectangleTransform.transform.localScale.x, startHeight, rectangleTransform.transform.localScale.z);
    
        if (isBest) StartCoroutine(spawnConfettis());
    }
    void Update()
    {
        rectangleTransform.transform.position = Vector3.Lerp(rectangleTransform.transform.position, new Vector3(rectangleTransform.transform.position.x, targetHeight/2 - halfHeight, 0), animationSpeed);
        rectangleTransform.transform.localScale = Vector3.Lerp(rectangleTransform.transform.localScale, new Vector3(rectangleTransform.transform.localScale.x, targetHeight, rectangleTransform.transform.localScale.z), animationSpeed);
        topTransform.position = new Vector3(topTransform.position.x, rectangleTransform.transform.position.y + rectangleTransform.transform.localScale.y/2, 0);
    } 

    IEnumerator spawnConfettis()
    {
        yield return new WaitForSeconds(5f);
        Instantiate(confettisPrefab, topTransform);
    }
}
