using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	
	public float panSpeed = 20f;
	public float scrollSpeed = 20f;
	public float minY = 5f;
	public float maxX = 120f;
	public float panBorderThickness = 10f;
	
	public Vector2 panLimit;
	void Update () {
		Vector3 cameraPosition = transform.position;
		if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness) {
			cameraPosition.z += panSpeed * Time.deltaTime;
		}
		if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness) {
			cameraPosition.z -= panSpeed * Time.deltaTime;
		}
		if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness) {
			cameraPosition.x -= panSpeed * Time.deltaTime;
		}
		if (Input.GetKey("d") || Input.mousePosition.x >= Screen.height - panBorderThickness) {
			cameraPosition.x += panSpeed * Time.deltaTime;
		}
		
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		//Debug.Log("SCROLL" + scroll);
		cameraPosition.y -= scroll * scrollSpeed * Time.deltaTime;
		if (Input.GetKey("[-]")) {
			Debug.Log("+");
			cameraPosition.y += scrollSpeed * Time.deltaTime;
		}

		if (Input.GetKey("[+]")) {
			cameraPosition.y -= scrollSpeed * Time.deltaTime;
		}
		
		cameraPosition.x = Mathf.Clamp(cameraPosition.x, -panLimit.x, panLimit.x);
		cameraPosition.y = Mathf.Clamp(cameraPosition.y, minY, maxX);
		cameraPosition.z = Mathf.Clamp(cameraPosition.z, -panLimit.y, panLimit.y);

		transform.position = cameraPosition;
	}
}
