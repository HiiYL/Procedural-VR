using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Aeroplane;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public List<GameObject> enemyShipTypes;
    public List<int> enemyCountWave;

	public GameObject exitGameObject;

    public int roundWaitTime = 3;
    public int spawnRange = 5000;
    public GameObject player;

    public Text enemiesLeftText;
    public Text currentWaveText;

    private static int enemiesLeft = 0;
    private static bool isNavigatingToExit = false;


    private Player playerComponent;
    private bool CursorLockedVar;
    private int currentWave = 1;
    private float roundWaitTimeLeft;
	private bool isSpawnedExit = false;
    private bool roundEnded = false;
    private bool isSpawningWave = false;

    private static GameManager _instance;

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = (false);
        CursorLockedVar = (true);
        playerComponent = player.GetComponent<Player>();
        roundWaitTimeLeft = 999;
        StartCoroutine(SpawnWave(currentWave));
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

    public void enemyDestroyed()
    {
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

    public void reachedExit()
    {
        isNavigatingToExit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (roundWaitTimeLeft > 0)
        {
            roundWaitTimeLeft -= Time.deltaTime;
            enemiesLeftText.text = "Enemies Arrives In ... " + (int)roundWaitTimeLeft;
        }
        else if(enemiesLeft <= 0)
        {
			if (!isNavigatingToExit && !isSpawningWave) {
				StartCoroutine(SpawnWave(currentWave));
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
        for (int x = 0; x < enemyCountWave[wave]; x++)
        {
            float angle = Random.Range(0.0f, Mathf.PI * 2);

            // create a vector with length 1.0
            Vector3 offset = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));

            // scale it to the desired length
            offset *= spawnRange;

            offset += new Vector3(0, 500, 0);
            //Vector3 offset = new Vector3(Random.Range(-300, 300), Random.Range(400, 500), Random.Range(-300, 300));
            GameObject obj = (GameObject)Instantiate(enemyShipTypes[0], player.transform.position + offset, Quaternion.identity);
            obj.GetComponent<AeroplaneAiControl>().SetTarget(player.transform);
            obj.transform.parent = transform;
            EventTrigger trigger = obj.GetComponentInParent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((eventData) => { playerComponent.startFiringBullets(obj.GetComponent<Enemy>()); });
            trigger.triggers.Add(entry);
            EventTrigger.Entry entry2 = new EventTrigger.Entry();
            entry2.eventID = EventTriggerType.PointerExit;
            entry2.callback.AddListener((eventData) => { playerComponent.stopFiringBullets(); });
            trigger.triggers.Add(entry2);
        }
        enemiesLeft += enemyCountWave[wave];
    }
}
