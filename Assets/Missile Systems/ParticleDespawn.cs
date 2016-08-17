using UnityEngine;
using System.Collections;

public class ParticleDespawn : MonoBehaviour {
	public float Durration;
	public float timer;
	void Update () {
		timer += Time.deltaTime;
		float durration = gameObject.GetComponent<ParticleSystem>().duration;
		Durration = durration;
			Destroy(gameObject,Durration);
	}
}
