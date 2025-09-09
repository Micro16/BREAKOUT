using UnityEngine;

public class Brick : MonoBehaviour
{

    public Color color = Color.white;
    
    void Start()
    {
        gameObject.GetComponent<Renderer>().material.color = color;
    }

    void Update()
    {
        
    }

}
