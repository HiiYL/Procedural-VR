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

    }

	void Update() {

        Vector3 relative = player.transform.InverseTransformPoint(transform.position);

        var angle = Mathf.Atan2(relative.z,relative.x) * Mathf.Rad2Deg;
        indicator.transform.localEulerAngles = new Vector3(0, 0, angle - 90);
    }

    void OnDestroy()
    {
        Destroy(indicator);
    }

}