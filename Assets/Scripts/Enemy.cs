using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public GameObject explosion;
    public AudioClip explosionSound;
    public GameObject player;

    public float fullHealth = 1;

    protected AudioSource audio;
    protected ObjectPooling pool;
    protected GameObject obj;

    protected float currentHealth;

    public float rateOfFire = 0.75f;
    public float timeLeftToFire=0;

    public static float missileRateOfFire = 30.0f;
    protected static float missileTimeLeftToFire = 0;


    // Use this for initialization
    protected void Start () {
        audio = GetComponent<AudioSource>();

        player = GameObject.FindGameObjectsWithTag("Player")[0];
        pool = GameObject.FindGameObjectWithTag("BulletPool").GetComponent<ObjectPooling>();

        currentHealth = fullHealth;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 directionToTarget = player.transform.position - transform.position;
        
        float angle = Vector3.Angle(transform.forward, directionToTarget);
        if (Mathf.Abs(angle) < 10)
        {
            float dist = Vector3.Distance(player.transform.position, transform.position);
            
            if (timeLeftToFire > rateOfFire && dist < 2000)
            {

                obj = pool.RetrieveInstance();
                if (obj)
                {
                    timeLeftToFire = 0;
                    obj.transform.position = transform.position + transform.forward * 50;
                    obj.transform.rotation = transform.rotation;
                    Bullet bullet = obj.GetComponent<Bullet>();
                    //bullet.isMissile = true;
                    //bullet.currentTarget = player;
                    obj.layer = LayerMask.NameToLayer("Enemy");
                    //obj.GetComponent<AutoDevolvePool>().time = 15;
                    obj.GetComponent<Rigidbody>().velocity = transform.forward * 1000;
                }
            }
            else
            {
                timeLeftToFire += Time.deltaTime;
            }
            
            if(missileTimeLeftToFire > missileRateOfFire)
            {
                obj = pool.RetrieveInstance();
                if (obj)
                {
                    missileTimeLeftToFire = 0;
                    obj.GetComponent<AutoDevolvePool>().time = 5;
                    obj.transform.rotation = transform.rotation;
                    obj.GetComponent<Bullet>().isMissile = true;
                    obj.layer = LayerMask.NameToLayer("Enemy");
                    obj.GetComponent<Bullet>().currentTarget = player.gameObject;
                }
            }else
            {
                missileTimeLeftToFire += Time.deltaTime;
            }
        }
    }

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "BulletPool" && isActiveAndEnabled)
		{
            //pool.DevolveInstance(other.gameObject);
            currentHealth--;
            if (currentHealth <= 0)
            {
                destroyEnemy();
            }
		}
	}


    public void destroyEnemy() {
        AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        audio.PlayOneShot(explosionSound);
        Instantiate (explosion,transform.position,transform.rotation);
        gameObject.SetActive(false);
        Destroy (this.gameObject,explosionSound.length);
		GameManager.Instance.enemyDestroyed(this);
	}
}
