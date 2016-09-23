using UnityEngine;
using System.Collections;

public class HealthPickup : MonoBehaviour {

    public float healAmount = 5;

    private bool isTriggered = false;

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (!isTriggered && other.gameObject.tag == "Player")
        {
            print("TRIGGERED!");
            print("HEALED!");
            other.gameObject.GetComponent<Player>().restoreHealth(healAmount);
            Destroy(gameObject);
            isTriggered = true;
        }
    }
}
