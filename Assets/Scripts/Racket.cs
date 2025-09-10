using UnityEngine;

public class Racket : MonoBehaviour
{

    public float speed = 0f;
    public float maxMovement = 4f;

    private GameInput gameInput;
    private bool locked;
    
    private void OnDisable()
    {
        gameInput.Disable();
    }

    private void OnEnable()
    {
        gameInput = new GameInput();
        gameInput.Enable();
    }

    public void Lock()
    {
        locked = true;
    }

    void Start()
    {
        locked = false;
    }

    void Update()
    {
        if (!locked)
        {
            Vector3 move = new Vector3(gameInput.Gameplay.Move.ReadValue<float>() * Time.deltaTime * speed, 0f, 0f);
            transform.position += move;

            if (transform.position.x > maxMovement || transform.position.x < -maxMovement)
            {
                Vector3 v = transform.position;
                v.x = v.x > 0 ? maxMovement : -maxMovement;
                transform.position = v;
            }
        }
    }

}
