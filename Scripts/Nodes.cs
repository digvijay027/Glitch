using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodes : MonoBehaviour {

	public List <Nodes> neighbors;
	public Vector2[] validDirections;


	// Use this for initialization
	void Start () {
		//Invoke("SetNeighbor",.1f);
	}
	
	public void SetNeighbor()
	{
		RaycastHit hit;

		if (Physics.Raycast (gameObject.transform.position, Vector3.left, out hit, 1f))
			neighbors.Add (hit.transform.GetComponent<Nodes>());
		if (Physics.Raycast(gameObject.transform.position, Vector3.right, out hit, 1f))
			neighbors.Add (hit.transform.GetComponent<Nodes>());
		if (Physics.Raycast(gameObject.transform.position, Vector3.up,out hit, 1f))
			neighbors.Add (hit.transform.GetComponent<Nodes>());
		if (Physics.Raycast(gameObject.transform.position, Vector3.down,out hit, 1f))
			neighbors.Add (hit.transform.GetComponent<Nodes>());


		validDirections = new Vector2[neighbors.Count];

		for (int i = 0; i < neighbors.Count; i++) {

			Nodes neighbor = neighbors [i]; 

			Vector2 temVector = neighbor.transform.localPosition - transform.position;

			validDirections [i] = temVector.normalized;

		}
	}
}
