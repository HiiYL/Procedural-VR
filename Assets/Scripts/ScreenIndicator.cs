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


	private bool onScreen;

	void Start () {
		//visible = GetComponent<SpriteRenderer> ().isVisible;
        indicatorCanvas = GameObject.FindWithTag("IndicatorCanvas");
        indicator = Instantiate(indicatorPrefab, indicatorCanvas.transform.position, indicatorCanvas.transform.rotation) as GameObject;
        indicator.transform.SetParent(indicatorCanvas.transform); 
        player = GameObject.FindWithTag("Player");
        print("Added indicator");

    }

	void Update() {
        //Vector3 aimVector = transform.position - player.transform.position;

        //aimVector.y = 0;

        indicator.transform.LookAt(transform.position);

        //print(aimVector);

        indicator.transform.localEulerAngles = new Vector3(0, 0, indicator.transform.localEulerAngles.z);
    }

    void OnDestroy()
    {
        print("Script was destroyed");
    }

}