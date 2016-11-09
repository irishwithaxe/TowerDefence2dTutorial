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

	private void Update() {
		Move();
	}

	public void Spawn() {
		IsActive = false;
		transform.position = LevelManager.Instance.BluePortal.transform.position;

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
		GridPosition = path[pos].GridPosition;
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
	}
}