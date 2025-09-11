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
    public TextMeshProUGUI StageUI;
    public TextMeshProUGUI InstructionsUI;
    public GameObject GameOverUI;

    [Header("Bricks Parameters")]
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;
    public int rows;
    public int columns;
    public Color upColor;
    public Color downColor;

    [Header("Instructions")]

    [TextArea]
    public string arrowKeysInst = "Use the right and left arrows on the keyboard to move the paddle.";

    [TextArea]
    public string spaceKeyInst = "Press space to start the game.";


    private bool gameStarted;
    private bool gameOver;
    private bool stageCompleted;
    private List<GameObject> bricks;
    private int stage;
    private int score;
    private int combo;

    private bool instMovePaddlePassed;
    private bool leftKeyPressed;
    private bool rightKeyPressed;

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

    public int Score { get { return score; } set { score = value; } }

    private void DisplayGameOverUI()
    {
        combo = 0;
        ComboUpdate();
        GameOverUI.SetActive(true);
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
            // Display UI here.
            Destroy(ball.gameObject);
            racket.Lock();
            stageCompleted = true;
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

    private void StageUpdate()
    {
        StageUI.text = "STAGE " + stage.ToString(); 
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
        stageCompleted = false;
        SetupBricks();
        gameOver = false;
        combo = 0;
        stage = SessionManager.instance.Stage;
        score = SessionManager.instance.Score;
        racket.speed = SessionManager.instance.OverallSpeed;
        ball.speed = SessionManager.instance.OverallSpeed;
        StageUpdate();
        ScoreUpdate();
        ComboUpdate();
        GameOverUI.SetActive(false);
        InstructionsUI.text = SessionManager.instance.FirstSession ? arrowKeysInst : spaceKeyInst;
        instMovePaddlePassed = SessionManager.instance.FirstSession ? false : true;
        InstructionsUI.gameObject.SetActive(true);

        if (SessionManager.instance.FirstSession)
            SessionManager.instance.FirstSession = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (instMovePaddlePassed && !gameStarted)
            {
                InstructionsUI.gameObject.SetActive(false);
                gameStarted = true;
                ball.Launch();
            }

            if (gameOver)
            {
                racket.Lock();
                SessionManager.instance.StartNewSession();
            }

            if (stageCompleted)
            {
                SessionManager.instance.LoadNextstage();
            }
        }

        if (!gameStarted && !instMovePaddlePassed)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                leftKeyPressed = true;

            if (Input.GetKeyDown(KeyCode.RightArrow))
                rightKeyPressed = true;

            instMovePaddlePassed = leftKeyPressed & rightKeyPressed;
            if (instMovePaddlePassed)
            {
                InstructionsUI.text = spaceKeyInst;
            }
        }
    }

}
