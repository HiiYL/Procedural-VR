using UnityEngine;
using System.Collections;

public class CanvasMovementCorrector : MonoBehaviour {

	private Vector3 initialTransform, currTransform;
	private Canvas canvas;
	// Use this for initialization
	void Start () {
		initialTransform = transform.position;
		canvas = GetComponentInChildren<Canvas> ();
	
	}
	
	// Update is called once per frame
	void Update () {
		currTransform = transform.position;
		//canvas.transform.position -= (currTransform - initialTransform);
	}
	void LateUpdate() {
		//canvas.transform.position += (currTransform - initialTransform);
		initialTransform = currTransform;
	}
}
