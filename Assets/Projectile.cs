using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public GameObject target;

    public float speed = 250f;

    public float damage = 5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (target && target.activeInHierarchy)
        {
            transform.LookAt(target.transform);

            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
    }
}
