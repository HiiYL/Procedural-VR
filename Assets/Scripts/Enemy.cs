using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public GameObject explosion;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "BulletPool" && isActiveAndEnabled)
		{
			destroyEnemy ();
		}
	}
	public void destroyEnemy() {
		Instantiate (explosion,transform.position,transform.rotation);
		Destroy (this.gameObject);
		GameManager.enemiesLeft--;
	}
}
