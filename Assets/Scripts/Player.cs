using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Aeroplane;

public class Player : MonoBehaviour {

	public GameObject crossHair,explosion;

	//public Text heightText;
    private ObjectPooling pool;
    private GameObject obj;
	private AeroplaneController aeroplaneController;
	private bool isFiringBullets;

    private Enemy currentTarget;


    // Use this for initialization
    void Start () {
        pool = GameObject.FindGameObjectWithTag("BulletPool").GetComponent<ObjectPooling>();
		aeroplaneController = GetComponent<AeroplaneController> ();
		isFiringBullets = false;

    }
	
	// Update is called once per frame
	void Update () {
		//print (isFiringBullets);
		if (isFiringBullets == true) {
			fireRaycastBullet ();
			//fireBullet ();
		} else {
			//print ("NOT FIRING BULLETS!");
		}
		//heightText.text = aeroplaneController.Altitude + "m";
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            if (Input.GetKey(KeyCode.Space))
            {
				fireBullet ();
            }
        }else
        {
            int i = 0;
            while (i < Input.touchCount)
            {
				fireBullet ();
                break;

            }
        }
    }
	public void fireRaycastBullet() {
		print ("Raycast bullets go pew pew");
		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward, out hit, 100000)) {
            print("Enemy Hit!");
            Enemy hitEnemy = hit.collider.gameObject.GetComponent<Enemy>();
            if (hitEnemy != null)
            {
                print("ITS NOT NULL");
                hitEnemy.destroyEnemy();
                print("Down The Enemy Goes!");
                fireBullet();
                isFiringBullets = false;
            }
		} else {
			print ("NOTHING HIT, DEPLOYING MISSILES");
            fireMissile();
		}
	}


	public void fireBullet() {
		print ("Player Goes Pew Pew!");

		//Ray ray = Camera.main.ScreenPointToRay(crossHair.transform.position);
		obj = pool.RetrieveInstance();
		if (obj)
		{
			obj.transform.position = transform.position;
		}
		//Vector3 direction = (ray.GetPoint(100000.0f) - transform.position);
		//obj.GetComponent<Rigidbody> ().velocity = direction * 100;
		obj.GetComponent<Rigidbody>().velocity = transform.forward * 1000;
		obj.transform.rotation = transform.rotation;
	}

    public void fireMissile()
    {
        print("HEAT SEEKER OF DEATH!!");
        obj = pool.RetrieveInstance();
        if (obj)
        {
            obj.transform.position = transform.position;
        }
        //obj.GetComponent<Rigidbody>().velocity = transform.forward * 100;
        obj.GetComponent<AutoDevolvePool>().time = 99;
        obj.transform.rotation = transform.rotation;
        obj.GetComponent<Bullet>().isMissile = true;
        obj.GetComponent<Bullet>().currentTarget = currentTarget;
    }
	public void startFiringBullets(Enemy enemy) {
		//print ("START FIRING!");
		this.isFiringBullets = true;
        currentTarget = enemy;


    }
	public void stopFiringBullets() {
		//print ("STOP FIRING!");
		isFiringBullets = false;
        currentTarget = null;

    }

	void OnTriggerEnter(Collider other)
	{
		//Instantiate (explosion,transform.position,transform.rotation);
		//Destroy (this.gameObject);
	}
}
