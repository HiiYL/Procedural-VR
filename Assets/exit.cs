using UnityEngine;
using System.Collections;

public class exit : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player" && isActiveAndEnabled)
		{

			Vector3 offset = new Vector3(Random.Range(-3000, 3000), 0, Random.Range(-3000, 3000));
			other.gameObject.transform.position += offset;
			GameManager.isNavigatingToExit = false;
			Destroy (this.gameObject);
		}
	}
}
