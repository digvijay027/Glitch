using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour {

	Grid g ;
	private static int gridWidth ;
	private static int gridHeight;



	// Use this for initialization
	void Awake () {

		//g = Grid.Instance;
	

//		Object[] objs = GameObject.FindObjectsOfType (typeof(GameObject));
//
//		foreach(GameObject o in objs)
//		{
//			Vector2 pos = o.transform.position;	
////			print (pos);
//			if (o.name != "Player") {
//				board [Mathf.Abs((int)pos.x), Mathf.Abs((int)pos.y)] = o;
//			}
//			else {
//				//Debug.Log ("Found Player at " +pos);
//			}
//		}
	}
	

}
