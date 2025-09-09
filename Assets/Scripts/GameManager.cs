using UnityEngine;

public class GameManager : MonoBehaviour
{

    public Ball ball;

    private bool gameStarted;

    void Start()
    {
        gameStarted = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!gameStarted)
            {
                ball.Launch();
            }
        }
    }

}
