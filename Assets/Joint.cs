using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal.Input;
using System;

public class Joint : MonoBehaviour {

    public Vector3 localRotationAxis;
    public float minAngle, maxAngle;
    public float rotationSpeed;
    public KeyCode positive, negative;
    public string posJoy, negJoy, posButton, negButton;

    private float angle;

	// Use this for initialization
	void Start () {
        angle=0;
	}
	
	// Update is called once per frame
	void Update () {
        float da = 0;
        if(Input.GetKey(positive))
            da+=1;
        if(Input.GetKey(negative))
            da+=-1;

        if(posJoy!=""){
        	float axis=Input.GetAxis(posJoy);
        	axis = axis*axis *Mathf.Sign(axis);
        	if(Mathf.Abs(axis)<=0.3)
        		axis=0;
        	else{
        		axis = Mathf.Sign(axis)*(Mathf.Abs(axis)-0.3F)/0.7F;
        	}
            da+=axis;
        }
        if(negJoy!=""){
        	float axis=Input.GetAxis(negJoy);
        	axis = axis*axis *Mathf.Sign(axis);
        	if(Mathf.Abs(axis)<=0.3)
        		axis=0;
        	else{
        		axis = Mathf.Sign(axis)*(Mathf.Abs(axis)-0.3F)/0.7F;
        	}
            da-=axis;
        }
        if(posButton != "")
            da+=Input.GetButton(posButton)?1:0;
        if(negButton != "")
            da-=Input.GetButton(negButton)?1:0;
        
        da = rotationSpeed*Time.deltaTime*da;
        if(angle+da > maxAngle)
            da=maxAngle-angle;
        else if(angle + da < minAngle)
            da = minAngle-angle;

        angle+=da;
        transform.Rotate(localRotationAxis, da, Space.Self);
	}
}
