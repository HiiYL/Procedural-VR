using UnityEngine;
using System.Collections;

public class ShootImprovedTracer : MonoBehaviour {
	public GameObject TargetObject;
	public GameObject Miss;
	public GameObject Target_;
	public GameObject SelectedObject;
	public int MissileLimit;
	public int MissileCount;
	public GameObject[] Missiles;
	
	void Update (){
		//make a list of all missiles in the scene
		Missiles = GameObject.FindGameObjectsWithTag("Missile");
		//find the length of the list of missiles
		MissileCount = Missiles.Length;

		if (Input.GetKeyDown(KeyCode.Mouse0)){
			//setup raycast with the screen
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit Hit;
			if(Physics.Raycast(ray, out Hit)){
				//check if there arent too many missiles in the scene
				if (MissileCount <= MissileLimit - 1){
					//find the object you clicked.
					SelectedObject = Hit.transform.gameObject;
					//instantiate the target object that the tracer will follow
					TargetObject = (GameObject)Instantiate(Target_, Hit.point, transform.rotation);
					//set the target object's parent as the selected object
					TargetObject.transform.parent = Hit.transform;
					//instantiate the tracer
					//the tracer will use variables set in this code which is why it is important it has the tag of "Shooter"
					//there are many ways to do this, i just used tags because they are easiest...
					Instantiate(Miss, transform.position, transform.rotation);
					//print that something just happened to the console
					Debug.Log ("You Selected " + Hit.transform.gameObject.name + ", A Tracking Missile Has Been Deployed.",gameObject);
				}
			}
		}
	}
}
