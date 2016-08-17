using UnityEngine;
using System.Collections;

public class ShootTracer : MonoBehaviour {

	public GameObject TargetObject;
	public GameObject Miss;
	public int MissileLimit;
	public int MissileCount;
	public GameObject[] Missiles;
	
	void Update (){
		//make a list of all the missiles in the scene
		Missiles = GameObject.FindGameObjectsWithTag("Missile");
		//find the length of the list
		MissileCount = Missiles.Length;
		if (Input.GetKeyDown(KeyCode.Mouse0)){
			//setup raycast
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit Hit;
			if(Physics.Raycast(ray, out Hit)){
				//check there aren't too many missiles already in the scene
					if (MissileCount <= MissileLimit - 1){
						TargetObject = Hit.transform.gameObject;
						Instantiate(Miss, transform.position, transform.rotation);
				}
			}
		}
	}
}
