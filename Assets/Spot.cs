using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour {

	// Create the kind of spots on the map.
	public enum SPOT_TYPE {FLOOR, START, POINT, WALL, BOXON, BOXOFF};
	public SPOT_TYPE spotType = SPOT_TYPE.FLOOR;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		if (spotType == SPOT_TYPE.START) {
			this.GetComponent<MeshRenderer>().material.color = Color.yellow;

		} else if (spotType == SPOT_TYPE.FLOOR) {
			this.GetComponent<MeshRenderer>().material.color = Color.white;

		} else if (spotType == SPOT_TYPE.WALL) {
			this.GetComponent<MeshRenderer>().material.color = Color.red;

		} else if (spotType == SPOT_TYPE.POINT) {
			this.GetComponent<MeshRenderer>().material.color = Color.blue;

		} else if (spotType == SPOT_TYPE.BOXON) {
			this.GetComponent<MeshRenderer>().material.color = Color.green;

		} else if (spotType == SPOT_TYPE.BOXOFF) {
			this.GetComponent<MeshRenderer>().material.color = Color.gray;

		}

	}
}
