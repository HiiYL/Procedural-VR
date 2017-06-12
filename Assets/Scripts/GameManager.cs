using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Aeroplane;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    // public List<GameObject> enemyShipTypes;
    // public List<int> enemyCountWave;
    public GameObject GameOverCanvas;
    public List<Material> skyBoxes;

	public GameObject exitGameObject;

    public List<GameObject> powerUps;

    [System.Serializable]
    public struct WaveInputPair
    {
        public GameObject unit;
        public int count;
    }

    [System.Serializable]
    public struct WaveInputPairs
    {
        public List<WaveInputPair> waveInputs;
    }

    public List<WaveInputPairs> wavesInputs;



    public int roundWaitTime = 3;
    public int spawnRange = 5000;
    public GameObject player;

    public Text enemiesLeftText;
    public Text currentWaveText;


    
    private int enemiesLeft = 0;



    private Player playerComponent;
    private bool CursorLockedVar;
    private int currentWave = 1;
    private float roundWaitTimeLeft;
	private bool isSpawnedExit = false;
    private bool roundEnded = false;
    private bool isSpawningWave = false;
    private bool isGameOver = false;
    private bool isNavigatingToExit = false;

    private List<GameObject> activePowerups;
    private List<GameObject> activeEnemies;

    private Camera mainCamera;





    private static GameManager _instance;

    // Use this for initialization
    void Start()
    {
        setRandomSkyBox();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = (false);
        CursorLockedVar = (true);
        playerComponent = player.GetComponent<Player>();
        roundWaitTimeLeft = 999;
        StartCoroutine(SpawnWave(currentWave));
        activePowerups = new List<GameObject>();
        activeEnemies = new List<GameObject>();

        mainCamera = Camera.main;
    }

    public static GameManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void enemyDestroyed(Enemy enemy)
    {
        //Spawn powerups
        var powerUp = Instantiate(powerUps[Random.Range(0, powerUps.Count)], enemy.transform.position, Quaternion.identity) as GameObject;
        activePowerups.Add(powerUp);
        enemiesLeft--;
        if (enemiesLeft <= 0)
        {
            print("SPAWNED EXIT!");
            enemiesLeftText.text = "Navigate to Exit";
            Vector3 offset = new Vector3(Random.Range(-300, 300), Random.Range(400, 500), Random.Range(-300, 300));
            GameObject obj = (GameObject)Instantiate(exitGameObject, player.transform.position + offset, Quaternion.identity);
            isSpawnedExit = true;
            isNavigatingToExit = true;
        }
    }

    public void playerDestroyed()
    {
        isGameOver = true;
        GameOverCanvas.SetActive(true);
        GameOverCanvas.transform.position = mainCamera.transform.position;
        GameOverCanvas.transform.rotation = mainCamera.transform.rotation;
    }

    public void onContinue()
    {
        isGameOver = false;
        isSpawnedExit = false;
        roundEnded = false;
        isSpawningWave = false;
        isNavigatingToExit = false;
        print("CONTINUE CALLED!");
        GameOverCanvas.SetActive(false);
        foreach (GameObject enemy in activeEnemies)
        {
            Destroy(enemy);
        }
        activeEnemies.Clear();
        for (int i = 0; i < activePowerups.Count; i++)
        {
            Destroy(activePowerups[i]);
        }
        player.transform.position = new Vector3(0, 500, 0);
        player.SetActive(true);

        player.GetComponent<Player>().restoreHealth(999);

        currentWave--;
        StartCoroutine(SpawnWave(currentWave));

    }

    public void setRandomSkyBox()
    {
        RenderSettings.skybox = skyBoxes[Random.Range(0, skyBoxes.Count)];
    }

    public void reachedExit()
    {
        setRandomSkyBox();
        isNavigatingToExit = false;
        for(int i = 0; i < activePowerups.Count; i++)
        {
            Destroy(activePowerups[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            if (roundWaitTimeLeft > 0)
            {
                roundWaitTimeLeft -= Time.deltaTime;
                enemiesLeftText.text = "Enemies Arrives In ... " + (int)roundWaitTimeLeft;
            }
            else if (enemiesLeft <= 0)
            {
				activeEnemies.Clear ();
                if (currentWave <= wavesInputs.Count)
                {
                    if (!isNavigatingToExit && !isSpawningWave)
                    {
                        StartCoroutine(SpawnWave(currentWave));
                    }
                }
                else
                {
                    currentWaveText.text = "Congratulations!";
                    enemiesLeftText.text = "You Win!";
                }
            }
            else
            {
                enemiesLeftText.text = enemiesLeft + " Enemies Left";
            }
            if (Input.GetKeyDown("escape") && !CursorLockedVar)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = (false);
                CursorLockedVar = (true);
            }
            else if (Input.GetKeyDown("escape") && CursorLockedVar)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = (true);
                CursorLockedVar = (false);
            }
        }

    }

    IEnumerator SpawnWave(int wave)
    {
        isSpawningWave = true;
        currentWaveText.text = "Wave " + currentWave;
        roundWaitTimeLeft = roundWaitTime;
        enemiesLeftText.text = "Enemies Arrives In ... " + (int)roundWaitTimeLeft;
        yield return new WaitForSeconds(roundWaitTime);
        intializeWave(currentWave);
        currentWave++;
        isSpawningWave = false;
    }
    void intializeWave(int wave)
    {
        print("INITIALIZING WAVE " + wave);
        WaveInputPairs waveInputPairs = wavesInputs[wave - 1];
        foreach (WaveInputPair waveInputPair in waveInputPairs.waveInputs)
        {
            spawnEnemy(waveInputPair.unit, waveInputPair.count);
        }
    }
    void spawnEnemy(GameObject toSpawn, int count)
    {
        for (int x = 0; x < count; x++)
        {
            float angle = Random.Range(0.0f, Mathf.PI * 2);

            // create a vector with length 1.0
            Vector3 offset = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));

            // scale it to the desired length
            offset *= spawnRange;

            offset += new Vector3(0, 500, 0);


            GameObject obj = (GameObject)Instantiate(toSpawn, player.transform.position + offset, Quaternion.identity);
            if(obj.GetComponent<AeroplaneAiControl>())
                obj.GetComponent<AeroplaneAiControl>().SetTarget(player.transform);
            obj.transform.parent = transform;
            obj.transform.LookAt(player.transform);
            EventTrigger trigger = obj.GetComponentInParent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((eventData) => { playerComponent.startFiringBullets(obj.GetComponent<Enemy>()); });
            trigger.triggers.Add(entry);
            EventTrigger.Entry entry2 = new EventTrigger.Entry();
            entry2.eventID = EventTriggerType.PointerExit;
            entry2.callback.AddListener((eventData) => { playerComponent.stopFiringBullets(); });
            trigger.triggers.Add(entry2);
            activeEnemies.Add(obj);
        }
		enemiesLeft = activeEnemies.Count;

    }
}
