using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Aeroplane;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public List<GameObject> enemyShipTypes;

	public GameObject exitGameObject;
    public int numberOfEnemiesLevel1 = 5;
    public int numberOfEnemiesLevel2 = 15;

    public int roundWaitTime = 3;
    public GameObject player;
    private Player playerComponent;
    private bool CursorLockedVar;

    public Text enemiesLeftText;
    public Text currentWaveText;

    public static int enemiesLeft = 0;

    private int currentWave = 1;
    private float roundWaitTimeLeft;

	public static bool isNavigatingToExit= false;
	private bool isSpawnedExit = false;
    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = (false);
        CursorLockedVar = (true);
        playerComponent = player.GetComponent<Player>();
        roundWaitTimeLeft = 999;
        StartCoroutine(SpawnWave(currentWave));
        print("CALLED m8");
    }
    void intializeWave(int wave)
    {
        print("INITIALIZING WAVE " + wave);
        switch (wave)
        {
            case 1:
                for (int x = 0; x < numberOfEnemiesLevel1; x++)
                {
                    Vector3 offset = new Vector3(Random.Range(-3000, 3000), Random.Range(400,500), Random.Range(-3000, 3000));
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
                enemiesLeft += numberOfEnemiesLevel1;
                break;
            case 2:
                for (int x = 0; x < numberOfEnemiesLevel2; x++)
                {
                    Vector3 offset = new Vector3(Random.Range(-3000, 3000), Random.Range(400, 500), Random.Range(-3000, 3000));
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
                enemiesLeft += numberOfEnemiesLevel2;
                break;
            default:
                for (int x = 0; x < currentWave * numberOfEnemiesLevel1; x++)
                {
                    Vector3 offset = new Vector3(Random.Range(-3000, 3000), Random.Range(400, 500), Random.Range(-3000, 3000));
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
                enemiesLeft += currentWave * numberOfEnemiesLevel1;
                break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (roundWaitTimeLeft > 0)
        {
            roundWaitTimeLeft -= Time.deltaTime;
            enemiesLeftText.text = "Enemies Arrives In ... " + (int)roundWaitTimeLeft;
			print ("CALLED");
        }
        else if(enemiesLeft <= 0)
        {
			if (!isSpawnedExit) {
				print ("SPAWNED EXIT!");
				Vector3 offset = new Vector3(Random.Range(-3000, 3000), Random.Range(400,500), Random.Range(-3000, 3000));
				GameObject obj = (GameObject)Instantiate(exitGameObject, player.transform.position + offset, Quaternion.identity);
				isSpawnedExit = true;
				isNavigatingToExit = true;
			}
			if (!isNavigatingToExit) {
				print ("Round Wait Time is " + roundWaitTimeLeft);
				if (currentWave != 1) {
					StartCoroutine(SpawnWave(currentWave));
				}
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
        print("CALLED!!!!!");
        currentWaveText.text = "Wave " + currentWave;
        roundWaitTimeLeft = roundWaitTime;
        enemiesLeftText.text = "Enemies Arrives In ... " + (int)roundWaitTimeLeft;
        yield return new WaitForSeconds(roundWaitTime);
        intializeWave(currentWave);
        currentWave++;
    }
}
