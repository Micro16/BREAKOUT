using UnityEngine;
using UnityEngine.SceneManagement;

public class SessionManager : MonoBehaviour
{
    public static SessionManager instance = null;

    public float startingOverallSpeed;
    
    private float overallSpeed;
    public float OverallSpeed { get { return overallSpeed; } }

    private int stage;
    public int Stage { get { return stage; } }

    private int score;
    public int Score { get { return score; } }

    private bool firstSession;
    public bool FirstSession { get { return firstSession; } set { firstSession = value; } }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        stage = 1;
        score = 0;
        overallSpeed = startingOverallSpeed;
        firstSession = true;
    }

    void Update()
    {
        
    }

    public void LoadNextstage()
    {
        score = GameManager.instance.Score;
        overallSpeed += 0.5f;
        stage++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartNewSession()
    {
        stage = 1;
        score = 0;
        overallSpeed = startingOverallSpeed;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
