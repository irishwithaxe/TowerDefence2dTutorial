using System;
using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour {

	[SerializeField]
	private float speed = 1f;

	private Node[] path;

	public Point GridPosition { get; set; }
	public bool IsActive { get; private set; }

	private int pathPos = 0;
	private Vector2 destination;

	private Animator myAnimator;

	private void Update() {
		Move();
	}

	public void Spawn() {
		IsActive = false;
		transform.position = LevelManager.Instance.BluePortal.transform.position;

		myAnimator = GetComponent<Animator>();

		StartCoroutine(Scale(new Vector3(0.1f, 0.1f), new Vector3(1f, 1f)));

		SetPath(LevelManager.Instance.Path);
	}

	public IEnumerator Scale(Vector3 from, Vector3 to) {
		float progress = 0;

		while (progress <= 1) {
			transform.localScale = Vector3.Lerp(from, to, progress);
			progress += Time.deltaTime;
			yield return null;
		}

		transform.localScale = to;
		IsActive = true;
	}

	private void Move() {
		if (!IsActive)
			return;

		transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);

		if (transform.position.x == destination.x && transform.position.y == destination.y) {
			pathPos++;
			if (pathPos < path.Length)
				StepToPos(pathPos);
		}
	}

	private void StepToPos(int pos) {
		var newPos = path[pos].GridPosition;
		Animate(GridPosition, newPos);
		GridPosition = newPos;

		destination = path[pos].WorldPosition;
	}

	private void SetPath(Node[] newpath) {
		if (newpath == null)
			return;

		path = newpath;
		pathPos = 0;
		StepToPos(pathPos);
	}

	private void Animate(Point oldPosition, Point newPosition) {

		Action<int, int> _setDirection = (vertical, horisontal) => {
			myAnimator.SetInteger("Vertical", vertical);
			myAnimator.SetInteger("Horisontal", horisontal);
		};

		if (oldPosition.Y < newPosition.Y) {
			// down
			Debug.Log("down to " + newPosition.ToString());
			_setDirection(1, 0);
		}
		else if (oldPosition.Y > newPosition.Y) {
			// up
			Debug.Log("up to " + newPosition.ToString());
			_setDirection(-1, 0);
		}
		else if (oldPosition.X < newPosition.X) {
			// right
			Debug.Log("right to " + newPosition.ToString());
			_setDirection(0, 1);
		}
		else {
			// left
			Debug.Log("left to " + newPosition.ToString());
			_setDirection(0, -1);
		}
	}
}