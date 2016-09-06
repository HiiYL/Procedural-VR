using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public GameObject explosion;
    public AudioClip explosionSound;
    public GameObject player;
    private AudioSource audio;
    private ObjectPooling pool;
    private GameObject obj;

    public float rateOfFire = 0.25f;
    public float timeLeftToFire=0;

    // Use this for initialization
    void Start () {
        audio = GetComponent<AudioSource>();

        player = GameObject.FindGameObjectsWithTag("Player")[0];
        pool = GameObject.FindGameObjectWithTag("BulletPool").GetComponent<ObjectPooling>();

    }
	
	// Update is called once per frame
	void Update () {
        Vector3 directionToTarget = player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToTarget);
        if (Mathf.Abs(angle) < 15)
        {
            if (timeLeftToFire > rateOfFire)
            {
                timeLeftToFire = 0;
                obj = pool.RetrieveInstance();
                if (obj)
                {
                    obj.transform.position = transform.position + transform.forward * 25;
                }
                //Vector3 direction = (ray.GetPoint(100000.0f) - transform.position);
                //obj.GetComponent<Rigidbody> ().velocity = direction * 100;
                obj.GetComponent<Rigidbody>().velocity = transform.forward * 1000;
                obj.transform.rotation = transform.rotation;
            }
            else
            {
                timeLeftToFire += Time.deltaTime;
            }
        }
    }

    void FixedUpate()
    {

    }

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "BulletPool" && isActiveAndEnabled)
		{

            destroyEnemy();
		}
	}


    public void destroyEnemy() {
        AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        audio.PlayOneShot(explosionSound);
        Instantiate (explosion,transform.position,transform.rotation);
        gameObject.SetActive(false);
        Destroy (this.gameObject,explosionSound.length);
		GameManager.Instance.enemyDestroyed();
	}
}
