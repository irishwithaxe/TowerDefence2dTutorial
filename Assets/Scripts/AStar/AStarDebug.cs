using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class AStarDebug : MonoBehaviour {

	private TileScript _start;
	private TileScript _goal;

	[SerializeField]
	private Sprite startPosMarker;

	[SerializeField]
	private Sprite goalPosMarker;

	[SerializeField]
	private GameObject arrowPrefab;

	private List<GameObject> placedGOs = new List<GameObject>();

	public TileScript StartPos {
		get {
			return _start;
		}

		set {
			PrintPosValue(value, "Start -> ");
			startPosMarker = SelectPos(value, _start, startPosMarker);

			if(value == null) {
				foreach (var go in placedGOs)
					Destroy(go);
				placedGOs.Clear();
			}

			_start = value;
		}
	}

	public TileScript GoalPos {
		get {
			return _goal;
		}

		set {
			PrintPosValue(value, " -> Goal");
			goalPosMarker = SelectPos(value, _goal, goalPosMarker);

			if (value != null) 
				AStar.GetPath(StartPos.GridPosition);
			
			_goal = value;
		}
	}

	private static void PrintPosValue(TileScript value, string posName) {
		if (value != null)
			Debug.Log("Set " + posName + " to gridpos x = " + value.GridPosition.X + " y = " + value.GridPosition.Y);
		else
			Debug.Log("Set " + posName + " to null");
	}

	private Sprite SelectPos(TileScript newValue, TileScript oldValue, Sprite newSprite) {
		Sprite oldSprite = null;
		if (newValue == null) {
			oldSprite = oldValue.SpriteRdr.sprite;
			oldValue.SpriteRdr.sprite = newSprite;
		}
		else {
			oldSprite = newValue.SpriteRdr.sprite;
			newValue.SpriteRdr.sprite = newSprite;
		}

		return oldSprite;
	}

	// Use this for initialization
	void Start() {
		AStar.OnChangeLists += DebugPath;
	}

	// Update is called once per frame
	void Update() {
		CLickTile();
	}

	private void CLickTile() {
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0)) {

			var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (hit.collider == null)
				return;

			var ts = hit.collider.GetComponent<TileScript>();
			if (ts == null)
				return;

			if (StartPos == null)
				StartPos = ts;
			else if (GoalPos == null)
				GoalPos = ts;
			else {
				StartPos = null;
				GoalPos = null;
			}
		}
	}

	public void DebugPath(HashSet<Node> openList) {
		foreach (var node in openList) {
			node.TileRef.SpriteRdr.color = Color.cyan;
			PlaceArrow(node);
		}
	}

	public void PlaceArrow(Node node) {
		if (node.Parent == null)
			return;

		var arrow = Instantiate(arrowPrefab, node.TileRef.WoldPosition, Quaternion.identity) as GameObject;
		placedGOs.Add(arrow);

		var direction = node.GridPosition - node.Parent.GridPosition;
		if (direction == new Point(1, 1)) // parent at top left
			arrow.transform.eulerAngles = new Vector3(0, 0, 135);
		else if(direction == new Point(-1,-1)) // parent at down right
			arrow.transform.eulerAngles = new Vector3(0, 0, -45);
		else if (direction == new Point(-1, 1)) // parent at top right
			arrow.transform.eulerAngles = new Vector3(0, 0, 45);
		else if (direction == new Point(1, -1)) // parent at down left
			arrow.transform.eulerAngles = new Vector3(0, 0, -135);
		else if (direction == new Point(0, 1)) // parent at Up
			arrow.transform.eulerAngles = new Vector3(0, 0, 90);
		else if (direction == new Point(0, -1)) // parent at down
			arrow.transform.eulerAngles = new Vector3(0, 0, -90);
		else if (direction == new Point(1, 0)) // parent at left
			arrow.transform.eulerAngles = new Vector3(0, 0, 180);
		else if (direction == new Point(-1, 0)) // parent at right
			arrow.transform.eulerAngles = new Vector3(0, 0, 0);
	}
}
