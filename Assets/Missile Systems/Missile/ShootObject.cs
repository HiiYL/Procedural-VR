using UnityEngine;
using System.Collections;

public class ShootObject : MonoBehaviour {

	public GameObject TargetPos;
	public GameObject Miss;
	public int MissileLimit;
	public int MissileCount;
	public GameObject[] Missiles;
	
	void Start (){
		//Find the target object that already exists in the sceene
		TargetPos = GameObject.FindGameObjectWithTag("Target");
	}
	
	void Update (){
		//make a list of all the missiles in the scene
		Missiles = GameObject.FindGameObjectsWithTag("Missile");
		//find the length of the list of missiles
		MissileCount = Missiles.Length;
		if (Input.GetKeyDown(KeyCode.Mouse0)){
			//setup raycast
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit Hit;
			if(Physics.Raycast(ray, out Hit)){
				//find out if there are too many missiles already in the scene
				if (MissileCount <= MissileLimit - 1){
					//set the target position
					TargetPos.transform.position = Hit.point;
					//instantiate the missile Prefab
					Instantiate(Miss, transform.position, transform.rotation);
				}
			}
		}
	}

}
