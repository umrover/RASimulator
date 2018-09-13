using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGUI : MonoBehaviour {

    private Joint[] joints;
	private Joint[] handJoints;
	private Joint selectedJoint;
	private int additionalJoints;
	private int prevAdditionalJoints;

	public GUISkin skin;

	public GameObject joint1;
	public GameObject jointN;
	public GameObject hands;

	private GameObject arm;

	private const string jointLetters = "ERTYUIOPASDFGHJKLZXCVBNM";

	void Start() {
		prevAdditionalJoints = additionalJoints = PlayerPrefs.GetInt("additionalJoints", 4);
		createArm();
		foreach (Joint j in joints) {
			ConfiguredJoint cj = j.GetComponent<ConfiguredJoint>();
			if (PlayerPrefs.HasKey(j.gameObject.name + " RS")) {
				j.rotationSpeed = PlayerPrefs.GetFloat(j.gameObject.name + " RS");
			}
			if (PlayerPrefs.HasKey(j.gameObject.name + " DOF")) {
				int index = PlayerPrefs.GetInt(j.gameObject.name + " DOF");
				switch (index) {
					case 0:
						cj.dof = ConfiguredJoint.DegreeOfFreedom.YAW;
						break;
					case 1:
						cj.dof = ConfiguredJoint.DegreeOfFreedom.PITCH;
						break;
					case 2:
						cj.dof = ConfiguredJoint.DegreeOfFreedom.ROLL;
						break;
				}
			}
			if (PlayerPrefs.HasKey(j.gameObject.name + " LEN")) {
				cj.length = PlayerPrefs.GetFloat(j.gameObject.name + " LEN");
			}
		}
		if(PlayerPrefs.HasKey("Joint "+(additionalJoints+2)+" RS")){
			foreach(Joint fJ in handJoints){
				fJ.rotationSpeed = PlayerPrefs.GetFloat("Joint "+ (additionalJoints + 2));
			}
		}
	}

	private void createArm(){
		if(arm!=null){
			Destroy(arm);
		}
		joints = new Joint[additionalJoints + 1];
		arm = Instantiate<GameObject>(joint1);
		arm.name = "Joint 1";
		joints[0] = arm.GetComponent<Joint>();

		Transform p = getLowestChild(arm.transform);
		for (int i = 0; i < additionalJoints; i++){
			GameObject newJoint = Instantiate<GameObject>(jointN);
			joints[i + 1] = newJoint.GetComponent<Joint>();
			newJoint.name = "Joint " + (i + 2);
			newJoint.transform.GetChild(0).name = (i + 2) + "-" + (i+3);
			newJoint.transform.parent = p;
			newJoint.transform.localPosition = Vector3.zero;
			p = getLowestChild(p);

			Joint joint = newJoint.GetComponent<Joint>();
			joint.positive = (KeyCode)System.Enum.Parse(typeof(KeyCode), ""+jointLetters[2 * i]);
			joint.negative = (KeyCode)System.Enum.Parse(typeof(KeyCode), ""+jointLetters[2 * i + 1]);
		}
		GameObject hand = Instantiate<GameObject>(hands);
		hand.transform.parent = p;
		hand.transform.localPosition = Vector3.zero;

		handJoints = hand.GetComponentsInChildren<Joint>();
		foreach(Joint h in handJoints){
			h.positive = (KeyCode)System.Enum.Parse(typeof(KeyCode), "" + jointLetters[2 * additionalJoints]);
			h.negative = (KeyCode)System.Enum.Parse(typeof(KeyCode), "" + jointLetters[2 * additionalJoints + 1]);
		}

	}

	private Transform getLowestChild(Transform t){
		while(t.childCount > 0) {
			t = t.GetChild(0);
		}
		return t;
	}

	void OnGUI(){
		GUI.skin = skin;

		GUILayout.Label("Press Escape to Quit");


		GUILayout.BeginHorizontal();
		GUILayout.Label("Number of joints: ");
		additionalJoints = Mathf.RoundToInt(GUILayout.HorizontalSlider(additionalJoints, 0, 8, GUILayout.Width(300)));
		GUILayout.Label("(" + (additionalJoints + 2) + ")", GUILayout.Width(300));
		GUILayout.EndHorizontal();

		foreach(Joint fJoint in handJoints) {
			if(fJoint == selectedJoint) {
				float fSpeed = handJoints[0].rotationSpeed;

				GUILayout.BeginHorizontal();
				GUILayout.Label("Joint F rotation speed: ");
				fSpeed = GUILayout.HorizontalSlider(fSpeed, 0, 180, GUILayout.Width(300));
				GUILayout.Label("(" + (int)(fSpeed) + " degrees per second)", GUILayout.Width(300));
				GUILayout.EndHorizontal();

				foreach (Joint fJ in handJoints) {
					fJ.rotationSpeed = fSpeed;
				}
				return;
			}
		}

		if (selectedJoint != null) {
			GUILayout.BeginHorizontal();
			GUILayout.Label(selectedJoint.gameObject.name + " rotation speed: ");
			selectedJoint.rotationSpeed = GUILayout.HorizontalSlider(selectedJoint.rotationSpeed, 0, 180, GUILayout.Width(300));
			GUILayout.Label("(" + (int)(selectedJoint.rotationSpeed) + " degrees per second)", GUILayout.Width(300));
			GUILayout.EndHorizontal();

			ConfiguredJoint cj = selectedJoint.GetComponent<ConfiguredJoint>();
			if (cj != null) {
				GUILayout.BeginHorizontal();
				GUILayout.Label(selectedJoint.gameObject.name + " length: ");
				cj.length = GUILayout.HorizontalSlider(cj.length, 0.1F, 5, GUILayout.Width(300));
				GUILayout.Label("(" + (int)(cj.length*100)/100F + " units)", GUILayout.Width(300));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label(selectedJoint.gameObject.name + " DOF: ");
				int index = 0;
				switch(cj.dof){
					case ConfiguredJoint.DegreeOfFreedom.YAW:
						index = 0;
						break;
					case ConfiguredJoint.DegreeOfFreedom.PITCH:
						index = 1;
						break;
					case ConfiguredJoint.DegreeOfFreedom.ROLL:
						index = 2;
						break;
				}
				index = Mathf.RoundToInt(GUILayout.HorizontalSlider(index, 0, 2, GUILayout.Width(300)));

				switch (index) {
					case 0:
						cj.dof = ConfiguredJoint.DegreeOfFreedom.YAW;
						break;
					case 1:
						cj.dof = ConfiguredJoint.DegreeOfFreedom.PITCH;
						break;
					case 2:
						cj.dof = ConfiguredJoint.DegreeOfFreedom.ROLL;
						break;
				}
				GUILayout.Label("(" + cj.dof.ToString() + ")", GUILayout.Width(300));
				GUILayout.EndHorizontal();
			}
		}



	}

	public void Update() {
		if(prevAdditionalJoints != additionalJoints) {
			prevAdditionalJoints = additionalJoints;
			createArm();
		}
		if (Input.GetKey(KeyCode.Escape)) {
			PlayerPrefs.SetInt("additionalJoints", additionalJoints);
			foreach (Joint j in joints) {
				PlayerPrefs.SetFloat(j.gameObject.name + " RS", j.rotationSpeed);
				ConfiguredJoint cj = j.GetComponent<ConfiguredJoint>();
				if (cj != null){
					int index = 0;
					switch (cj.dof) {
						case ConfiguredJoint.DegreeOfFreedom.YAW:
							index = 0;
							break;
						case ConfiguredJoint.DegreeOfFreedom.PITCH:
							index = 1;
							break;
						case ConfiguredJoint.DegreeOfFreedom.ROLL:
							index = 2;
							break;
					}
					PlayerPrefs.SetInt(j.gameObject.name + " DOF", index);
					PlayerPrefs.SetFloat(j.gameObject.name + " LEN", cj.length);
				}
			}
			PlayerPrefs.SetFloat("Joint " + (additionalJoints + 2) + " RS", handJoints[0].rotationSpeed);
			PlayerPrefs.Save();
			Application.Quit();
		}

		if (Input.GetMouseButtonDown(0)) { // if left button pressed...
			Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				Joint j = hit.transform.GetComponent<Joint>();
				if (j==null)
					j= hit.transform.parent != null ? hit.transform.parent.GetComponent<Joint>() : null;
				if (j != null) {
					this.selectedJoint = j;
				}
			}
		}
	}
}
