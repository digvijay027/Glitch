using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public float speed = 4.0f;
	private Vector2 direction = Vector2.zero;
	private Vector2 nextDirection;

	private Nodes currentNode; 
	private Nodes previousNode; 
	private Nodes targetNode; 
	private Vector2 startPos;
	public float minSwipeDistY;

	public float minSwipeDistX;

	// Use this for initialization
	void Start ()
	{
		
		transform.position = Grid.Instance.intersectionNodes[Random.Range (0,Grid.Instance.intersectionNodes.Count)].transform.position;
		Invoke ("Delay",.2f);

	}

	void Delay()
	{
		Nodes node = GetNodeAtPosition (transform.position);

		if (node != null) {

			currentNode = node;
			//Debug.Log (currentNode);
		}

		direction = Vector2.zero;
		ChangePosition (direction);
	}

	// Update is called once per frame
	void Update ()
	{
		CheckInput ();
		Move ();
		//UpdateOrientation ();

	}

	void CheckInput()
	{
		#if UNITY_EDITOR 
		if (Input.GetKeyDown (KeyCode.LeftArrow)) 
		{
			ChangePosition(Vector2.left);

		}
		else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			ChangePosition(Vector2.right);

		}
		else if (Input.GetKeyDown (KeyCode.UpArrow)) {
			ChangePosition(Vector2.up);
		}
		else if (Input.GetKeyDown (KeyCode.DownArrow)) {
			ChangePosition(Vector2.down);
		}

		#else

		if (Input.touchCount > 0) 

		{
			Touch touch = Input.touches[0];

			switch (touch.phase) 

		{

			case TouchPhase.Began:

			startPos = touch.position;

			break;

			case TouchPhase.Ended:

			float swipeDistVertical = (new Vector3(0, touch.position.y, 0) - new Vector3(0, startPos.y, 0)).magnitude;

		if (swipeDistVertical > minSwipeDistY) 

		{

			float swipeValue = Mathf.Sign(touch.position.y - startPos.y);

			if (swipeValue > 0)//up swipe
					{
						ChangePosition(Vector2.up);
					}

			else if (swipeValue < 0)//down swipe
					{
						ChangePosition(Vector2.down);

					}

		}

		float swipeDistHorizontal = (new Vector3(touch.position.x,0, 0) - new Vector3(startPos.x, 0, 0)).magnitude;

		if (swipeDistHorizontal > minSwipeDistX) 

		{

			float swipeValue = Mathf.Sign(touch.position.x - startPos.x);

			if (swipeValue > 0)//right swipe
			{
						ChangePosition(Vector2.right);
				
			}

			else if (swipeValue < 0)//left swipe
			{
						ChangePosition(Vector2.left);

			}

		}
			break;
		}
	
	}
		#endif
	}
		

	void ChangePosition(Vector2 d)
	{
		if (d != direction)
			nextDirection = d;
		if (currentNode != null) {

			Nodes moveToNode = CanMove (d);
			if (moveToNode != null) {

				direction = d;
				targetNode = moveToNode;
				previousNode = currentNode;
				currentNode = null;
			}
		}
	}


	void Move()
	{
		if(targetNode!=currentNode && targetNode!=null)
		{
			if(nextDirection == direction * -1)
			{
				direction *= -1;

				Nodes tempNode = targetNode;
				targetNode = previousNode;
				previousNode = tempNode;
			}

			if (OverShotTarget ()) {

				currentNode = targetNode;
				transform.localPosition = currentNode.transform.position;

				Nodes moveToNode = CanMove (nextDirection);
				if (moveToNode != null)
					direction = nextDirection;
				if (moveToNode == null)
					moveToNode = CanMove (direction);
				if (moveToNode != null) {

					targetNode = moveToNode;
					previousNode = currentNode;
					currentNode = null;
				} else {

					direction = Vector2.zero;
				}


			} else {
				transform.localPosition += (Vector3)(direction * speed) * Time.deltaTime;

			}
		}

	}

	void MoveToNode(Vector2 d)
	{
		Nodes moveToNode = CanMove (d);
		if (moveToNode != null) {

			transform.localPosition = moveToNode.transform.position;
			currentNode = moveToNode;
		}
	}

//	void UpdateOrientation()
//	{
//		if (direction == Vector2.left) {
//
//			transform.localScale = new Vector3 (-1,1,1);
//			transform.localRotation = Quaternion.Euler (0, 0, 0);
//		}
//		else if (direction == Vector2.right) {
//			
//			transform.localScale = new Vector3 (1,1,1);
//			transform.localRotation = Quaternion.Euler (0, 0, 0);
//		}
//		else if (direction == Vector2.up) {
//			
//			transform.localScale = new Vector3 (1,1,1);
//			transform.localRotation = Quaternion.Euler (0, 0, 90);
//		}
//		else if (direction == Vector2.down) {
//			
//			transform.localScale = new Vector3 (1,1,1);
//			transform.localRotation = Quaternion.Euler (0, 0, 270);
//		}
//	}

	Nodes CanMove(Vector2 d)
	{
		Nodes moveToNode = null;

		for (int i = 0; i < currentNode.neighbors.Count; i++) {

			if (currentNode.validDirections [i] == d) {

				moveToNode = currentNode.neighbors[i];
				break;
			}
		}

		return moveToNode;
	}

	Nodes GetNodeAtPosition(Vector2 pos)
	{
		Vector3 p = new Vector3 (pos.x, pos.y, 0);
		for (int i = 0; i < Grid.Instance.intersectionNodes.Count; i++) {
			if (p == Grid.Instance.intersectionNodes [i].transform.position) {
				return Grid.Instance.intersectionNodes [i];
			}
		}
		return null;
	}

	bool OverShotTarget()
	{
		float nodeToTarget = LengthFromNode (targetNode.transform.position);
		float nodeToSelf = LengthFromNode (transform.localPosition);
		return nodeToSelf > nodeToTarget;
	}

	float LengthFromNode(Vector2 targetPosition)
	{
		Vector2 vec = targetPosition - (Vector2)previousNode.transform.position;
		return vec.sqrMagnitude;
	}

}
