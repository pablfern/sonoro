using UnityEngine;
using System.Collections;

public class CameraSize : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Camera cam = GetComponent<Camera>();
        cam.orthographicSize = Screen.height / 2.0f;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
