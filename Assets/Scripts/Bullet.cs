using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public GameObject explosion;
    public Enemy currentTarget;

    public bool isMissile = false;

    public float missileSpeed = 50f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(isMissile)
        {
            if (!currentTarget)
            {
                this.gameObject.SetActive(false);
            }
            else {
                transform.LookAt(currentTarget.transform);
                transform.Translate(Vector3.forward * missileSpeed * Time.deltaTime);
            }
            //GetComponent<Rigidbody>().AddForce(currentTarget.transform.position - transform.position * 5);
        }
	}

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
}
