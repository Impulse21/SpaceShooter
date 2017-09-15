using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct EnemyDetails
{
    public int numOfEnemies;
    public GameObject gameObject;

    public float minSpeed;
    public float maxSpeed;

    public float maxRotation;
}

[System.Serializable]
public struct WaveDetails
{
    public List<EnemyDetails> enemies;
}

public class GameController : MonoBehaviour 
{
    public static GameController sharedInstance;

    [Header("Spawn Details")]
    public int delayStart;
    public int spawnInterval;
    public int delayWave;
    public List<WaveDetails> waves;

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

        if (scoreGuiText != null)
        {
            scoreGuiText.text = "Score: " + playerScore.ToString();
        }
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
            yield return spawnEnemies();

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
    IEnumerator spawnEnemies()
    {
        WaveDetails waveDetail = getWave();

        foreach (EnemyDetails enemy in waveDetail.enemies)
        {
            for (int i = 0; i < enemy.numOfEnemies; i++)
            {
                spawnObject(enemy);

                yield return new WaitForSeconds(spawnInterval);
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
        if (scoreGuiText != null)
        {
            scoreGuiText.text = "Score: " + playerScore.ToString();
        }
    }

    private void generateEnemiesMovement(GameObject gameObj, float minSpeed, float maxSpeed)
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

    private WaveDetails getWave()
    {
        int waveIndex = Random.Range(0, waves.Count);

        return waves[waveIndex];
    }

    private void spawnObject(EnemyDetails enemyDetails)
    {
        Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight));
        Vector3 bottomRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0.0f));

        Vector3 spawnLocation = new Vector3(topRight.x, Random.Range(bottomRight.y, topRight.y), 0.0f);
        GameObject enemy = ObjectPool.SharedInstance.GetPooledObject(enemyDetails.gameObject.tag);

        if (enemy != null)
        {
            enemy.transform.position = spawnLocation;
            enemy.SetActive(true);

            generateEnemiesMovement(enemy, enemyDetails.minSpeed, enemyDetails.maxSpeed);
        }
    }
}
