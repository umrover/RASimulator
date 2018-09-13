using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Joint))]
public class ConfiguredJoint : MonoBehaviour {

	public float length;
	private float prevLength;

	public enum DegreeOfFreedom {
		YAW, PITCH, ROLL
	}
	public DegreeOfFreedom dof = DegreeOfFreedom.YAW;
	private DegreeOfFreedom prevDof;

	private Vector3 initialLocalRot;

	void Start () {
		prevDof = DegreeOfFreedom.YAW;
		if (prevDof == dof)
			prevDof = DegreeOfFreedom.ROLL;
		
		initialLocalRot = transform.localEulerAngles;
		prevLength = -1;
	}

	void Update () {
		if(prevDof != dof){
			prevDof = dof;
			transform.localEulerAngles = initialLocalRot;
			Joint j = GetComponent<Joint>();
			switch(dof){
				case DegreeOfFreedom.YAW:
					j.localRotationAxis = Vector3.up;
					break;
				case DegreeOfFreedom.PITCH:
					j.localRotationAxis = Vector3.right;
					break;
				case DegreeOfFreedom.ROLL:
					j.localRotationAxis = Vector3.forward;
					break;
			}
		}

		if (prevLength != length){
			prevLength = length;
			Transform child = transform.GetChild(0);
			child.localScale = new Vector3(0.5F, 0.5F, length);
			child.transform.localPosition = Vector3.back * length / 2;

			child = child.GetChild(0);
			child.localScale = new Vector3(2, 2, 1.0F / length);
		}
	}
}
