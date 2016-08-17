using UnityEngine;
using System.Collections;

public class Improved_Tracer : MonoBehaviour {
	
	public float Speed;
	public int LookSpeed;
	public float TimeTillTrack;
	public float Timer;
	public float DistanceTillStopLooking;
	public float CalculatedDistance;
	public Vector3 Target;
	public GameObject Shooter;
	public GameObject TargetObject;
	public GameObject SelectedObject;
	public Quaternion targetRotation;
	public GameObject Explosion;
	public bool stopTurning;
	public int TimeTillExpire;
	public bool Die;
	
	void Start (){
		//Find the spawn object, this is needed because this code will be using some of the other code's variables
		Shooter = GameObject.FindGameObjectWithTag("Shooter");
		//find the Spawned target object in the <ShootImprovedTracer> code
		//this however can be done in the spawn code after instantiating using the code snippet below:
		//Missile = (GameObject)Instantiate(Miss, transform.position, transform.rotation);
		//Missile.GetComponent<Improved_Tracer>().TargetObject = TargetObject;
		TargetObject = Shooter.GetComponent<ShootImprovedTracer>().TargetObject;
		//this is used to show how to print what is happening to the console
		SelectedObject = Shooter.GetComponent<ShootImprovedTracer>().SelectedObject;
	}
	
	void Update (){
		//create a timer
		Timer += Time.deltaTime;
		//check that the shooter is there, technically you dont need this, but it is best because otherwise an error pops up in the console
		if (Shooter != null) {
				//set Target Vector to the TargetObject position
			if (TargetObject != null){
				Target = TargetObject.transform.position;
			}
				//kill if runs out of time
			if (Timer > TimeTillExpire) {
				Debug.Log ("The Tracer Has Been Termniated due to a Failure to Hit it's Target on Time");
				Die = true;
			}
				//find distance from tracer to target
						CalculatedDistance = Vector3.Distance (gameObject.transform.position, Target);
				//move the tracer
						transform.Translate (0, 0, Speed / 100);
				//delay tracking for a certain amount of time...
						if (Timer > TimeTillTrack) {
								if (stopTurning == false) {
				//look at the target position...
										targetRotation = Quaternion.LookRotation (Target - transform.position);
										transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, Time.deltaTime * LookSpeed);
								}
						}
				//kill if is close enough
						if (CalculatedDistance < DistanceTillStopLooking) {
								stopTurning = true;
								Debug.Log ("The Tracer Has Been Termniated Successfully Hitting the Specified Target: <" + SelectedObject.name + ">", gameObject);
								Die = true;
						}
				} else {
					Die = true;
					Debug.Log ("Tracer Has Been Terminated Because It's Spawner Has Returned A Null",gameObject);
				}

		if (TargetObject == null){
			Die = true;
		}
		//what happens when die = true...
		//instantiate boom
		//destroy both the gameObject and the Target Object...
		if (Die == true){
			Instantiate(Explosion, transform.position, transform.rotation);
			Destroy(gameObject,0);
			Destroy(TargetObject,0);
		}
	}
	//when the tracer hits an object...
	void OnCollisionEnter (Collision coll){
		//check if the shooter is still around.
		if (Shooter != null) {
			//check if the tracer hit the target object or something else.
			//it really doesn't do much in this code, it only prints what happened to the console, but if you need to see how, this is it...
						if (coll.transform.gameObject == SelectedObject) {
								Debug.Log ("The Tracer Has Been Termniated Successfully Hitting the Specified Target: <" + SelectedObject.name + ">", gameObject);
						} else {
								Debug.Log ("The Tracer Has Been Termniated Failing to Hit the Specified Target <" + SelectedObject.name + "> Because " + coll.transform.gameObject.name + " Obstructed it's Path.", gameObject);
						}
						Die = true;
				} 
	}
	//used to visualize what is going on in a paused scene
	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, DistanceTillStopLooking);
		Debug.DrawLine (transform.position, Target, Color.red);
		Debug.DrawLine (transform.position, Shooter.transform.position, Color.red);
		Debug.DrawLine (Target, Shooter.transform.position, Color.blue);
	}

}
