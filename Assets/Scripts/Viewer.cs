using UnityEngine;
using System.Collections;

public class Viewer : MonoBehaviour {
	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
	public float minimumX = -360F;
	public float maximumX = 360F;
	public float minimumY = -60F;
	public float maximumY = 60F;

    private float initialOrientationX;
    private float initialOrientationY;
    private float initialOrientationZ;

    float rotationX = 0F;
	float rotationY = 0F;
	Quaternion originalRotation;

	void Update ()
	{
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            if (axes == RotationAxes.MouseXAndY)
            {
                // Read the mouse input axis

                rotationX += Input.GetAxis("Mouse X") * sensitivityX;
                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                rotationX = ClampAngle(rotationX, minimumX, maximumX);
                rotationY = ClampAngle(rotationY, minimumY, maximumY);
                Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
                Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);
                transform.localRotation = originalRotation * xQuaternion * yQuaternion;
            }
            else if (axes == RotationAxes.MouseX)
            {
                rotationX += Input.GetAxis("Mouse X") * sensitivityX;
                rotationX = ClampAngle(rotationX, minimumX, maximumX);
                Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
                transform.localRotation = originalRotation * xQuaternion;
            }
            else
            {
                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                rotationY = ClampAngle(rotationY, minimumY, maximumY);
                Quaternion yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);
                transform.localRotation = originalRotation * yQuaternion;
            }
        }else
        {
            rotationX = (initialOrientationX - Input.gyro.rotationRateUnbiased.x);
            rotationX *= (sensitivityX/ 10f);
            rotationY = (initialOrientationY - Input.gyro.rotationRateUnbiased.y);
            rotationX *= (sensitivityY / 10f);
            transform.Rotate(initialOrientationX - Input.gyro.rotationRateUnbiased.x,  rotationY ,0);
        }
	}
	void Start ()
	{
        /*
		// Make the rigid body not change rotation
		if (rigidbody)
			rigidbody.freezeRotation = true;
		*/
        Input.gyro.enabled = true;

        // Save the firsts values
        initialOrientationX = Input.gyro.rotationRateUnbiased.x;
        initialOrientationY = Input.gyro.rotationRateUnbiased.y;
        initialOrientationZ = -Input.gyro.rotationRateUnbiased.z;
        originalRotation = transform.localRotation;
	}
	public static float ClampAngle (float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp (angle, min, max);
	}
}

