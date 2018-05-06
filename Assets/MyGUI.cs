using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGUI : MonoBehaviour {

    public Joint[] joints;
	public Joint[] fJoints;
	public GUISkin skin;

	void Start() {
		foreach (Joint j in joints) {
			if (PlayerPrefs.HasKey(j.gameObject.name)) {
				j.rotationSpeed = PlayerPrefs.GetFloat(j.gameObject.name);
			}
		}
		if(PlayerPrefs.HasKey("Joint F")){
			foreach(Joint fJ in fJoints){
				fJ.rotationSpeed = PlayerPrefs.GetFloat("Joint F");
			}
		}
	}

	void OnGUI(){
		GUI.skin = skin;
		foreach(Joint j in joints){
			GUILayout.BeginHorizontal();

			GUILayout.Label(j.gameObject.name + " rotation speed: ");
			j.rotationSpeed=GUILayout.HorizontalSlider(j.rotationSpeed, 0, 180, GUILayout.Width(300));
			GUILayout.Label("(" + (int)(j.rotationSpeed) +" degrees per second)", GUILayout.Width(300));
			GUILayout.EndHorizontal();
		}

		float fSpeed = fJoints[0].rotationSpeed;

		GUILayout.BeginHorizontal();
		GUILayout.Label("Joint F rotation speed: ");
		fSpeed = GUILayout.HorizontalSlider(fSpeed, 0, 180, GUILayout.Width(300));
		GUILayout.Label("(" + (int)(fSpeed) + " degrees per second)", GUILayout.Width(300));
		GUILayout.EndHorizontal();

		foreach (Joint fJ in fJoints) {
			fJ.rotationSpeed = fSpeed;
		}


		GUILayout.Label("Press Escape to Quit");
	}

	void Update() {
		if(Input.GetKey(KeyCode.Escape)){
			foreach (Joint j in joints) {
				PlayerPrefs.SetFloat(j.gameObject.name, j.rotationSpeed);
			}
			PlayerPrefs.SetFloat("Joint F", fJoints[0].rotationSpeed);
			PlayerPrefs.Save();
			Application.Quit();
		}
	}
}
