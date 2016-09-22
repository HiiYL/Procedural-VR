using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public GameObject explosion;
    public Enemy currentTarget;

    public bool isMissile = false;

    public float missileSpeed = 10f;

    public float homingSensitivity = 5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(isMissile)
        {
            //_distanceFromTarget = Vector3.Distance(this.transform.position, currentTarget.transform.position);

            if (currentTarget && currentTarget.isActiveAndEnabled)
            {

                var dist = Vector3.Distance(currentTarget.transform.position, transform.position);

                //transform.LookAt(currentTarget.transform);
                float step = missileSpeed * Time.deltaTime;
                var rotation = Quaternion.LookRotation(currentTarget.transform.position - this.transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);

                transform.Translate(Vector3.forward * Time.deltaTime * missileSpeed);
                //transform.Translate(Vector3.forward * missileSpeed * Time.deltaTime);
            }
            else {
                transform.Translate(Vector3.forward * missileSpeed * Time.deltaTime);
            }
            //GetComponent<Rigidbody>().AddForce(currentTarget.transform.position - transform.position * 5);
        }
	}

    /*

	IEnumerator OnCallDestroy() {
		//		animator.SetTrigger("Destroyed");
		//		yield return new WaitForSeconds(3.0f); // The length of your animation...
		//		player.AddExperience(expOnDeath);
		//		base.Die();
		//		AudioSource.PlayClipAtPoint(blastSound, transform.position);
		Instantiate (explosion,transform.position,transform.rotation);
		yield return new WaitForSeconds(3.0f); // The length of your animation...
		Destroy(this.gameObject);
	}
    */
}
