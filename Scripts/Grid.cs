using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
	public static Grid Instance = null;

	// grid specifies
	[SerializeField]
	private int rows;
	[SerializeField]
	private int cols;
	[SerializeField]
	private Vector2 gridSize;
	[SerializeField]
	private Vector2 gridOffset;
	//about cells
	[SerializeField]
	private Sprite cellSprite;
	private Vector2 cellSize;
	private Vector2 cellScale;

	public GameObject nodes;
	public GameObject prefab;
	public List<Vector3> gridPos;
	public List<Nodes> intersectionNodes;
	internal int intersectionSizeX;
	internal int intersectionSizeY;




	void Awake(){
		Instance = this;
//		intersectionSizeX = cols ;
//		intersectionSizeY = rows ;
		intersectionSizeX = cols -1 ;
		intersectionSizeY = rows -1;
		InitCells ();
	}
	void Start () {


	}

	void InitCells()
	{
		GameObject cellObj = new GameObject ();

		cellObj.AddComponent<SpriteRenderer> ().sprite = cellSprite;

		cellSize = cellSprite.bounds.size;

		Vector2 newCellSize = new Vector2 (gridSize.x / (float)cols, gridSize.y / (float)rows);
		cellScale.x = newCellSize.x / cellSize.x;
		cellScale.y = newCellSize.y / cellSize.y;

		cellSize = newCellSize;

		cellObj.transform.localScale = new Vector2 (cellScale.x,cellScale.y);

		gridOffset.x = -(gridSize.x / 2) + cellSize.x/2;
		gridOffset.y = -(gridSize.y / 2) + cellSize.y/2;


		for (int row = 0; row < rows; row++) {
			for (int col = 0; col < cols; col++) {

				Vector2 pos = new Vector2 (col * cellSize.x + gridOffset.x, row * cellSize.y + gridOffset.y);

				GameObject go = Instantiate (cellObj, pos, Quaternion.identity) as GameObject;

				go.transform.parent = transform;
				gridPos.Add (go.transform.position);
			}
			
		}
		CreateIntersection ();
		Destroy (cellObj);
	}

	void CreateIntersection()
	{
		
		Transform t = null;
		GameObject go = null;


		for (int i = 1; i <= intersectionSizeY; i++) {
			for (int j = 1; j <= intersectionSizeX ; j++) {		
				go = Instantiate (prefab);
				if (i == 1 && j == 1) {
					go.transform.position = new Vector3 (gridPos [0].x + (cellSize.x / 2), gridPos [0].y + (cellSize.y / 2), gridPos [0].z);

				}else if(i > 1 && j ==1){
					if(go != null)
						go.transform.position = new Vector3 (gridPos[0].x+ (cellSize.x/2),t.position.y+cellSize.x,t.position.z);
				} else {					
					go.transform.position = new Vector3 (t.position.x+cellSize.x,t.position.y,t.position.z);

				}
				t = go.transform;
				//go.transform.SetParent (nodes.transform);
				intersectionNodes.Add (go.GetComponent<Nodes>());
			}
		}

		for (int i = 0; i < intersectionNodes.Count; i++) {
			intersectionNodes [i].SetNeighbor ();
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube (transform.position, gridSize);

	}


}
