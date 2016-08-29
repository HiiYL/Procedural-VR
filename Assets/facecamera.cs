using UnityEngine;
using System.Collections;

public class facecamera : MonoBehaviour {

    private Camera mainCamera;

	// Use this for initialization
	void Start () {
        mainCamera = Camera.main;
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 v = mainCamera.transform.position - transform.position;
        v.x = v.z = 0.0f;
        transform.LookAt(mainCamera.transform.position - v);
        transform.Rotate(0, 180, 0);

    }
}
