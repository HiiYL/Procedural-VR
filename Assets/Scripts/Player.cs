using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Aeroplane;
using System;

public class Player : MonoBehaviour {

    public GameObject crossHair, explosion;

    //public Text heightText;
    private ObjectPooling pool;
    private GameObject obj;
    private AeroplaneController aeroplaneController;
    public AudioClip explosionSound;



    private AudioSource audio;
    private bool isFiringBullets;

    public float fullHealth = 15;
    private float currentHealth = 15;

    public Image healthBarCircle;


    private Enemy currentTarget;

    private bool invincible = false;
    private Renderer[] _renderers;


    // Use this for initialization
    void Start () {
        pool = GameObject.FindGameObjectWithTag("BulletPool").GetComponent<ObjectPooling>();
        audio = GetComponent<AudioSource>();
        aeroplaneController = GetComponent<AeroplaneController> ();
		isFiringBullets = false;

        currentHealth = fullHealth;
        _renderers = GetComponentsInChildren<Renderer>();
    }

    public void restoreHealth(float health)
    {
        currentHealth = Mathf.Max(fullHealth, currentHealth + health);
    }

    // Update is called once per frame
    void Update () {
        //healthSlider.value = currentHealth / fullHealth;

        healthBarCircle.fillAmount = currentHealth / fullHealth;

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
        }
        /*
        else
        {
            int i = 0;
            while (i < Input.touchCount)
            {
				fireBullet ();
                break;

            }
        }
        */
    }
	public void fireRaycastBullet() {
		print ("Raycast bullets go pew pew");
        fireMissile();
	}
	public void fireBullet() {
		print ("Player Goes Pew Pew!");

		//Ray ray = Camera.main.ScreenPointToRay(crossHair.transform.position);
		obj = pool.RetrieveInstance();
		if (obj)
		{
			obj.transform.position = transform.position + transform.forward;
		}
		//Vector3 direction = (ray.GetPoint(100000.0f) - transform.position);
		//obj.GetComponent<Rigidbody> ().velocity = direction * 100;
		//obj.GetComponent<Rigidbody>().velocity = transform.forward * 1000;
		obj.transform.rotation = transform.rotation;
        obj.layer = LayerMask.NameToLayer("Player");
        obj.GetComponent<Rigidbody>().velocity = transform.forward * 1000;
    }

    public void fireMissile()
    {
        obj = pool.RetrieveInstance();
        if (obj)
        {
            obj.transform.position = transform.position + transform.forward * 50;
            obj.GetComponent<AutoDevolvePool>().time = 15;
            obj.transform.rotation = transform.rotation;
            obj.GetComponent<Bullet>().isMissile = true;
            obj.layer = LayerMask.NameToLayer("Player");
            obj.GetComponent<Bullet>().currentTarget = currentTarget.gameObject;
        }
    }
	public void startFiringBullets(Enemy enemy) {
        currentTarget = enemy;
        fireMissile();
    }
	public void stopFiringBullets() {
        currentTarget = null;

    }

    void OnTriggerEnter(Collider other)
    {
       
        
        if (other.gameObject.tag == "BulletPool" || other.gameObject.tag == "Terrain")
        {
            pool.DevolveInstance(other.gameObject);
            if (!invincible)
            {
                AudioSource.PlayClipAtPoint(explosionSound, transform.position);
                audio.PlayOneShot(explosionSound);
                Instantiate(explosion, transform.position, transform.rotation);
                if (currentHealth > 0)
                {
                    //GetComponent<Rigidbody>().AddExplosionForce(25, transform.position, 5);
                    currentHealth--;
                    print(currentHealth);
                    invincible = true;
                    StartCoroutine(Blink(5f, 0.2f));
                }
                else
                {
                    destroyPlayer();
                }
            }
        }else
        {
            print("Player - Collision - Misc Tag - Triggered!");
        }
        
    }

    IEnumerator Blink(float time, float intervalTime)
    {
        invincible = true;
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            foreach (var renderer in _renderers)
            {
                renderer.enabled = !renderer.enabled;
            }
            elapsedTime += intervalTime;
            yield return new WaitForSeconds(intervalTime);
        }
        foreach (var renderer in _renderers)
        {
            renderer.enabled = true;
        }
        invincible = false;
    }

    public void destroyPlayer()
    {

        gameObject.SetActive(false);
        //Destroy(this.gameObject, explosionSound.length);
    }
}
