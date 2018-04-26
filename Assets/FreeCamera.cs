using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamera : MonoBehaviour {

	// Use this for initialization
    private float phi=120, theta=40;
    public float phiSpeed, thetaSpeed;
	
	// Update is called once per frame
    void Update () {
        if(Input.GetKey(KeyCode.LeftArrow))
            phi+=phiSpeed*Time.deltaTime;
        if(Input.GetKey(KeyCode.RightArrow))
            phi-=phiSpeed*Time.deltaTime;
        if(Input.GetKey(KeyCode.UpArrow))
            theta+=thetaSpeed*Time.deltaTime;
        if(Input.GetKey(KeyCode.DownArrow))
            theta-=thetaSpeed*Time.deltaTime;

        transform.position=Quaternion.AngleAxis(phi, Vector3.up)*Vector3.forward*15;
        transform.LookAt(Vector3.zero);
        transform.position = Quaternion.AngleAxis(theta, transform.right)*transform.position;
        transform.LookAt(Vector3.zero);
        transform.position+=Vector3.back*3;
	}
}
