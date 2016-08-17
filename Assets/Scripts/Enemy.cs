using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public GameObject explosion;
    public AudioClip explosionSound;
    private AudioSource audio;

	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
		GameManager.enemiesLeft--;
	}
}
