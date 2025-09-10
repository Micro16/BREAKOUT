using System.Collections;
using UnityEngine;

public class Brick : MonoBehaviour
{
    
    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(0.03f);
        GameManager.instance.RemoveBrick(gameObject);
        Destroy(gameObject);
    }

    public void SetColor(Color color)
    {
        gameObject.GetComponent<Renderer>().material.color = color;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Score");
    }

    private void OnCollisionExit(Collision collision)
    {
        StartCoroutine(WaitAndDestroy());
    }

}
