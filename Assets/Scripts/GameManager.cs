using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;

    [Header("Game Components")]
    public Ball ball;
    public Racket racket;
    public GameObject brickPrefab;
    public TextMeshProUGUI ScoreUI;
    public TextMeshProUGUI ComboUI;

    [Header("Bricks Parameters")]
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;
    public int rows;
    public int columns;
    public Color upColor;
    public Color downColor;

    private bool gameStarted;
    private bool gameOver;
    private List<GameObject> bricks;
    private int score;
    private int combo;

    public bool GameOver 
    { 
        get 
        { 
            return gameOver; 
        } 
        set 
        { 
            gameOver = value;
            if (gameOver)
            {
                racket.Lock();
                DisplayGameOverUI();
            }
        } 
    }

    private void DisplayGameOverUI()
    {
        combo = 0;
        ComboUpdate();
    }

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
                GameObject brick = Instantiate(brickPrefab, new Vector3(x, y, z), Quaternion.identity);
                float t = (float)i / rows;
                brick.GetComponent<Brick>().SetColor(Color.Lerp(upColor, downColor, t));
                bricks.Add(brick);
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

    private void ScoreUpdate()
    {
        ScoreUI.text = "SCORE : " + score.ToString();
    }

    private void ComboUpdate()
    {
        if (combo == 0)
            ComboUI.text = "";
        else if (combo >= 2)
            ComboUI.text = "   COMBO : x" + combo.ToString();
    }

    public void ScoreIncrease()
    {
        score++;
        ScoreUpdate();
    }

    public void ComboApplyAndReset()
    {
        if (combo >= 2)
            score += combo;
        ScoreUpdate();
        combo = 0;
        ComboUpdate();
    }

    public void ComboIncrease()
    {
        combo++;
        ComboUpdate();
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
        score = 0;
        ScoreUpdate();
        ComboUI.text = "";
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
