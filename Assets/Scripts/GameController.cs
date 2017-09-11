using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour 
{
    public static GameController sharedInstance;

    [Header("Spawn Details")]
    public int delayStart;
    public int spawnInterval;
    public int delayWave;
    public int enemiesPerWave = 5;

    [Header("Enemies Details")]
    public float maxSpeed = 10.0f;
    public float minSpeed = 0.1f;
    public float maxRotationSpeed = 20.0f;

    [Header("UI Components")]
    public GUIText scoreGuiText;
    public GUIText gameOverGuiText;
    public GUIText restartGuiText;

    private int playerScore = 0;
    private bool bGameOver = false;
    private bool bRestart = false;

    public void Start()
    {
        sharedInstance = this;

        Random.InitState( (int) System.DateTime.Now.Ticks);

        StartCoroutine(SpawnEnemies());

        scoreGuiText.text = "Score: " + playerScore.ToString();
    }

    public void Update()
    {
        if (bRestart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(delayStart);

        while (true)
        {
            spawnEnemies();

            yield return new WaitForSeconds(delayWave);

            if (bGameOver)
            {
                restartGuiText.text = "Press 'R' for Restart";
                bRestart = true;

                break;
            }
        }
    }

    // Coroutine to spawn elements
    void spawnEnemies()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight));
            Vector3 bottomRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0.0f));

            Vector3 spawnLocation = new Vector3(topRight.x, Random.Range(bottomRight.y, topRight.y), 0.0f);
            GameObject enemies = ObjectPool.SharedInstance.GetPooledObject("Meteor");

            if (enemies != null)
            {
                enemies.transform.position = spawnLocation;
                enemies.SetActive(true);
               
                generateEnemiesMovement(enemies);
            }
        }
    }
        
    public void addScore(int incScore)
    {
        playerScore += incScore;

        updateScore();
    }

    public void gameOver()
    {
        gameOverGuiText.text = "Game Over";
        bGameOver = true;
    }

    private void updateScore()
    {
        scoreGuiText.text = "Score: " + playerScore.ToString();
    }

    private void generateEnemiesMovement(GameObject gameObj)
    {
        Rigidbody2D rigBody = gameObj.GetComponent<Rigidbody2D>();

        if (rigBody != null)
        {
            float speed = Random.Range(minSpeed, maxSpeed);
            rigBody.velocity = new Vector2(-Mathf.Abs(Random.value), 0.0f) * speed;
        }
        else
        {
            Debug.Log("Unable to get Enemies Rigid Body 2D");
        } 
    }
}
