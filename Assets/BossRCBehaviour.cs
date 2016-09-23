using UnityEngine;
using System.Collections;

public class BossRCBehaviour : Enemy {

    public GameObject projectilePrefab;

	// Use this for initialization
	void Start () {
        base.Start();
        rateOfFire = 3.0f;
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(player.transform);


        timeLeftToFire += Time.deltaTime;

        if (timeLeftToFire > rateOfFire)
        {
            timeLeftToFire -= rateOfFire;
            var projectile = Instantiate(projectilePrefab);
            projectile.transform.position = transform.position + transform.forward * 50;
            projectile.transform.rotation = transform.rotation;
            projectile.layer = LayerMask.NameToLayer("Enemy");
            projectile.tag = "BulletPool";

            projectile.GetComponent<Projectile>().target = player;

        }
    }

    new void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BulletPool" && isActiveAndEnabled)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
            audio.PlayOneShot(explosionSound);
            Instantiate(explosion, transform.position, transform.rotation);
            //pool.DevolveInstance(other.gameObject);
            currentHealth--;
            if (currentHealth <= 0)
            {
                destroyEnemy();
            }
        }
    }
}
