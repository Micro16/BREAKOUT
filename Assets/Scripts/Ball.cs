using UnityEngine;

public class Ball : MonoBehaviour
{

    public float speed;
    public float deflection;

    private int combo;
    
    public void Launch()
    {
        transform.parent = null;
        
        Vector3 velocity = Vector3.up;

        float r = Random.Range(-0.25f, 0.25f);

        velocity += Vector3.right * r;

        velocity = velocity.normalized * speed;

        gameObject.GetComponent<Rigidbody>().linearVelocity = velocity;
    }
    
    void Start()
    {

    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "brick")
        {
            GameManager.instance.ScoreIncrease();
            GameManager.instance.ComboIncrease();
        }
        else if (collision.collider.tag == "racket")
        {
            GameManager.instance.ComboApplyAndReset();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Vector3 velocity = gameObject.GetComponent<Rigidbody>().linearVelocity;

        if (Mathf.Abs(Vector3.Dot(velocity.normalized, Vector3.up)) > 0.8f)
        {
            velocity += velocity.x > 0 ? Vector3.right * speed * deflection : Vector3.left * speed * deflection;
        }

        if (Mathf.Abs(Vector3.Dot(velocity.normalized, Vector3.right)) > 0.8f)
        {
            velocity += velocity.y > 0 ? Vector3.up * speed * deflection : Vector3.down * speed * deflection;
        }

        velocity = velocity.normalized * speed;

        gameObject.GetComponent<Rigidbody>().linearVelocity = velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager.instance.GameOver = true;
        Destroy(gameObject);
    }

}
