#pragma strict

var renderTextureCams : GameObject[];
var portals : GameObject[];

var portalIndex : int;

//How To Use
//------------
//
//1. Attach this script to your main camera.
//
//2. Make a camera that is parented to an empty game object. (This empty game object will be your camera's root position)
//	 Do step 3 for each portal you have in your scene. Make sure these cameras do not have audio listeners.
//
//3. On this script set the number of render cameras and portals you have in your scene.
//
//4. Set the cameras (not the camera's root objects) to the open "render texture cams" slots on this script.
//
//5. Make a render texture for each camera with at least 1024 x 576 resolution.
//	 The resolution has to match your screen's aspect ratio.
//
//6. Apply the render textures to each of the portal's materials.
//	 The materials must use the provided render texture portal shaders
//
//7. Finaly, Set the portals to the open "portals" slots on this script.
//	 make sure that the render texture cam slots and portal slots corrospond to each other.
//	 for example, the render texture camera in "render texture cams Element 0" goes with the portal in "portals Element 0".
//

function Start () {

}

function Update () {
	
	portalIndex = 0;
	
	for (var cam : GameObject in renderTextureCams) {
		
		cam.transform.localPosition = transform.position - portals[portalIndex].transform.position;
		cam.transform.localRotation = transform.localRotation;
		
		portalIndex += 1;
		
	}
	
}