using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public int spawnLocationBuffer = 1;

    [Header("UI Components")]
    public Text scoreGuiText;
    public GameObject gameOverMenu;

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

        if (gameOverMenu != null)
        {
            gameOverMenu.SetActive(false);
        }
    }

    public void Update()
    {
        if (bGameOver)
        {
            gameOverMenu.SetActive(true);
        }

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

        Vector3 spawnLocation;

        do
        {
            spawnLocation = new Vector3(topRight.x, Random.Range(bottomRight.y + spawnLocationBuffer, topRight.y - spawnLocationBuffer), 0.0f);
        }
        while (willCollideWithObject(spawnLocation));


        GameObject enemy = ObjectPool.SharedInstance.GetPooledObject(enemyDetails.gameObject.tag);
        if (enemy != null)
        {
            enemy.transform.position = spawnLocation;
            enemy.SetActive(true);
            generateEnemiesMovement(enemy, enemyDetails.minSpeed, enemyDetails.maxSpeed);
        }
    }

    private bool willCollideWithObject(Vector3 startLocation)
    {
        RaycastHit2D hit = Physics2D.Raycast(startLocation, Vector3.left * 100);

        if (hit.collider != null)
        {
            return hit.collider.tag == "Meteor";
        }

        return false;
    }
}
