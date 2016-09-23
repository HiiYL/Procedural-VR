using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public GameObject explosion;
    public GameObject currentTarget;

    public bool isMissile = false;

    public float missileSpeed = 1f;

    public float homingSensitivity = 5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(isMissile)
        {

            if (currentTarget && currentTarget.activeInHierarchy)
            {

                transform.LookAt(currentTarget.transform);

                transform.Translate(Vector3.forward * Time.deltaTime * missileSpeed);
            }
            else {
                transform.Translate(Vector3.forward * missileSpeed * Time.deltaTime);
            }
        }
	}
}
