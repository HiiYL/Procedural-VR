//By Joseph Giordano.
using UnityEngine;
using System.Collections;
using System;

//Attach this class to the GameObject you want the arrow to be pointing at.
public class ScreenIndicator : MonoBehaviour {

	public Texture2D icon; //The icon. Preferably an arrow pointing upwards.
	public Texture2D targetTexture;
	public float iconSize = 50f;

    public GameObject indicatorPrefab;
	bool visible = true; //Whether or not the object is visible in the camera.
	private Bounds collisionBounds;
	private BoxCollider boxCollider;

    private GameObject indicatorCanvas, indicator, player;

    Quaternion rotation;


    private bool onScreen;

	void Start () {
		//visible = GetComponent<SpriteRenderer> ().isVisible;
        indicatorCanvas = GameObject.FindWithTag("IndicatorCanvas");
        player = GameObject.FindWithTag("Player");
        indicator = Instantiate(indicatorPrefab, indicatorCanvas.transform.position, indicatorCanvas.transform.rotation) as GameObject;
        indicator.transform.SetParent(indicatorCanvas.transform);

        print("Added indicator");

    }

	void Update() {

        Debug.DrawLine(transform.position, player.transform.position);


        // fast rotation
        //var dir = transform.position - player.transform.position;

        //dir.y = 0;

        //Debug.Log(((player.transform.forward - transform.forward) * 6.28318530718F));

        //var correctedForward = ((player.transform.forward - transform.forward) * 6.28318530718F);

        //var correctedAngle = Mathf.Atan2(correctedForward.z, correctedForward.x) * Mathf.Rad2Deg;

        Vector3 relative = player.transform.InverseTransformPoint(transform.position);

        var angle = Mathf.Atan2(relative.z,relative.x) * Mathf.Rad2Deg;
        Debug.Log(relative);
        Debug.Log(angle);
        indicator.transform.localEulerAngles = new Vector3(0, 0, angle - 90);

        //Apply the rotation 
        //transform.rotation = rot;

        // put 0 on the axys you do not want for the rotation object to rotate
        //transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
        /*

        indicator.transform.LookAt(transform.position);
        Debug.DrawLine(transform.position, indicator.transform.position);
        indicator.transform.localEulerAngles = new Vector3(0, 0, indicator.transform.localEulerAngles.z);
        */
    }

    void OnDestroy()
    {
        print("Script was destroyed");
        Destroy(indicator);
    }

}