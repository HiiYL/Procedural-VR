using UnityEngine;
using System.Collections;

public class Tracer : MonoBehaviour {

	public float Speed;
	public int LookSpeed;
	public float TimeTillTrack;
	public float Timer;
	public float DistanceTillStopLooking;
	public float CalculatedDistance;
	public Vector3 Target;
	public GameObject Shooter;
	public GameObject TargetObject;
	public Quaternion targetRotation;
	public GameObject Explosion;
	public bool stopTurning;
	public int TimeTillExpire;
	public bool Die;
	//what happens when the code first runs
	void Start (){
		//find the spawn object
		Shooter = GameObject.FindGameObjectWithTag("Shooter");
		//set the target object = to the target object in the shoot tracer code
		TargetObject = Shooter.GetComponent<ShootTracer>().TargetObject;
	}
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update (){
		//set up timer
		Timer += Time.deltaTime;
		//set target Vector = to the target object's position
		Target = TargetObject.transform.position;
		//die if times out...
		if (Timer > TimeTillExpire){
			Die = true;
		}
		//find distance from gameObject to Target Vector
		CalculatedDistance = Vector3.Distance(gameObject.transform.position, Target);
		//move the transform
		transform.Translate(0,0,Speed/100);
		//delay tracking for a certain amount of time...
		if (Timer > TimeTillTrack){
			if (stopTurning == false){
				//look at target Vector
				targetRotation = Quaternion.LookRotation(Target - transform.position);
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * LookSpeed);
			}
		}
		//kill if close enough
		if (CalculatedDistance < DistanceTillStopLooking){
			stopTurning = true;
			Die = true;
		}
		//what happens when die == true
		//instantiate boom and destroy gameObject...
		if (Die == true){
			Instantiate(Explosion, transform.position, transform.rotation);
			Destroy(gameObject,0);
		}
	}
	//if it hits anything
	void OnCollisionEnter (){
		Die = true;
	}
	/// <summary>
	/// Raises the draw gizmos selected event.
	/// </summary>
	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, DistanceTillStopLooking);
		Debug.DrawLine (transform.position, Shooter.transform.position, Color.red);
		Debug.DrawLine (transform.position, TargetObject.transform.position, Color.red);
		Debug.DrawLine (TargetObject.transform.position, Shooter.transform.position, Color.blue);
	}
}
