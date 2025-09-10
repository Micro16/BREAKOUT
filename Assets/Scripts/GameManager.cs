using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;

    [Header("Game Components")]
    public Ball ball;
    public GameObject brickPrefab;

    [Header("Bricks Parameters")]
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;
    public int rows;
    public int columns;

    private bool gameStarted;
    private bool gameOver;
    private List<GameObject> bricks;

    public bool GameOver { get { return gameOver; } set { gameOver = value; } }

    private void SetupBricks()
    {
        bricks = new List<GameObject>();

        float prWidth = brickPrefab.GetComponent<Renderer>().bounds.size.x; 
        float prHeight = brickPrefab.GetComponent<Renderer>().bounds.size.y;
        float marginX = ((xMax - xMin) - (columns * prWidth) + prWidth) / (columns - 1); 
        float marginY = ((yMax - yMin) - (rows * prHeight) + prHeight) / (rows - 1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                float x = xMin + (j * (prWidth + marginX));
                float y = yMax - (i * (prHeight + marginY));
                float z = 10f;
                bricks.Add(Instantiate(brickPrefab, new Vector3(x, y, z), Quaternion.identity));
            }
        }
    }

    public void RemoveBrick(GameObject go)
    {
        bricks.Remove(go);
        if (bricks.Count == 0)
        {
            Debug.Log("VICTOIRE !!");
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void Start()
    {
        gameStarted = false;
        SetupBricks();
        gameOver = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!gameStarted)
            {
                gameStarted = true;
                ball.Launch();
            }

            if (gameOver)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

}
