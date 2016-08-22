//By Joseph Giordano.
using UnityEngine;
using System.Collections;

//Attach this class to the GameObject you want the arrow to be pointing at.
public class ScreenIndicator : MonoBehaviour {

	public Texture2D icon; //The icon. Preferably an arrow pointing upwards.
	public Texture2D targetTexture;
	public float iconSize = 50f;
	[HideInInspector]
	public GUIStyle gooey; //GUIStyle to make the box around the icon invisible. Public so that everything has the default stats.
	Vector2 indRange;
	float scaleRes = Screen.width / 500; //The width of the screen divided by 500. Will make the GUI automatically
	//scale with varying resolutions.
	Camera cam;
	bool visible = true; //Whether or not the object is visible in the camera.
	private Bounds collisionBounds;
	private BoxCollider boxCollider;

	private bool onScreen;

	void Start () {
		//visible = GetComponent<SpriteRenderer> ().isVisible;

		cam = Camera.main; //Don't use Camera.main in a looping method, its very slow, as Camera.main actually
		//does a GameObject.Find for an object tagged with MainCamera.

		indRange.x = Screen.width;
		indRange.y = Screen.height;
		indRange /= 2f;

		gooey.normal.textColor = new Vector4 (0, 0, 0, 0); //Makes the box around the icon invisible.
		boxCollider = GetComponent<BoxCollider>();
		collisionBounds = boxCollider.bounds;

		GUI.enabled = false;

		collisionBounds = boxCollider.bounds;
	}

	void Update() {
		Vector3 screenPoint = cam.WorldToViewportPoint(this.transform.position);
		onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
		collisionBounds = boxCollider.bounds;
	}

	void OnGUI () {
		if (onScreen) {
			GUI.DrawTexture (GUIRectWithObject (), targetTexture);
		} else {
			Vector3 dir = transform.position - cam.transform.position;
			dir = Vector3.Normalize (dir);
			dir.y *= -1f;

			Vector2 indPos = new Vector2 (indRange.x * dir.x, indRange.y * dir.y);
			indPos = new Vector2 ((Screen.width / 2) + indPos.x,
				(Screen.height / 2) + indPos.y);

			Vector3 pdir = transform.position - cam.ScreenToWorldPoint (new Vector3 (indPos.x, indPos.y,
				transform.position.z));
			pdir = Vector3.Normalize (pdir);

			float angle = Mathf.Atan2 (pdir.x, pdir.y) * Mathf.Rad2Deg;

			GUIUtility.RotateAroundPivot(angle, indPos); //Rotates the GUI. Only rotates GUI drawn after the rotate is called, not before.
			GUI.Box (new Rect (indPos.x, indPos.y, scaleRes * iconSize, scaleRes * iconSize), icon,gooey);
			GUIUtility.RotateAroundPivot(0, indPos); //Rotates GUI back to the default so that GUI drawn after is not rotated.
		}
	}

	public Rect GUIRectWithObject()
	{
		Vector3 cen = collisionBounds.center;
		Vector3 ext = collisionBounds.extents;
		Vector2[] extentPoints = new Vector2[8]
		{
			WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z-ext.z)),
			WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z-ext.z)),
			WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z+ext.z)),
			WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z+ext.z)),
			WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z-ext.z)),
			WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z-ext.z)),
			WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z+ext.z)),
			WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z+ext.z))
		};
		Vector2 min = extentPoints[0];
		Vector2 max = extentPoints[0];
		foreach (Vector2 v in extentPoints)
		{
			min = Vector2.Min(min, v);
			max = Vector2.Max(max, v);
		}
		return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
	}

	public Vector2 WorldToGUIPoint(Vector3 world)
	{
		Vector2 screenPoint = cam.WorldToScreenPoint(world);
		screenPoint.y = (float) Screen.height - screenPoint.y;
		return screenPoint;
	}

	void OnBecameInvisible() {
		visible = false;
	}
	//Turns off the indicator if object is onscreen.
	void OnBecameVisible() {
		visible = true;
	}
}