using System;
using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour {

	[SerializeField]
	private float speed = 1f;

	[SerializeField]
	private int movingType = 0;

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
		transform.position = LevelManager.Instance.BluePortal.transform.position;
		GridPosition = LevelManager.Instance.BlueSpawn;

		myAnimator = GetComponent<Animator>();

		StartCoroutine(Scale(new Vector3(0.1f, 0.1f), new Vector3(1f, 1f)));

		SetPath(LevelManager.Instance.Path);
	}

	public IEnumerator Scale(Vector3 from, Vector3 to, bool destroyAfterScaling = false) {
		float progress = 0;

		while (progress <= 1) {
			transform.localScale = Vector3.Lerp(from, to, progress);
			progress += Time.deltaTime;
			yield return null;
		}

		transform.localScale = to;
		if (destroyAfterScaling) {
			Release();
		}
		else
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

		if (movingType == 0)
			Animate(GridPosition, newPos);
		if (movingType == 1)
			Rotate(GridPosition, newPos);

		GridPosition = newPos;
		destination = path[pos].WorldPosition;
	}

	private void Rotate(Point oldPosition, Point newPosition) {
		var direction = newPosition - oldPosition;

		if (direction == new Point(1, 1)) // goal at down right
			transform.eulerAngles = new Vector3(0, 0, -135);
		else if (direction == new Point(-1, -1)) // goal at top left
			transform.eulerAngles = new Vector3(0, 0, 45);
		else if (direction == new Point(-1, 1)) // goal at down left
			transform.eulerAngles = new Vector3(0, 0, 135);
		else if (direction == new Point(1, -1)) // goal at top right
			transform.eulerAngles = new Vector3(0, 0, -45);
		else if (direction == new Point(0, 1)) // goal at down
			transform.eulerAngles = new Vector3(0, 0, -180);
		else if (direction == new Point(0, -1)) // goal at up
			transform.eulerAngles = new Vector3(0, 0, 0);
		else if (direction == new Point(1, 0)) // goal at right
			transform.eulerAngles = new Vector3(0, 0, -90);
		else if (direction == new Point(-1, 0)) // goal at left
			transform.eulerAngles = new Vector3(0, 0, 90);
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

		if (oldPosition.Y < newPosition.Y)
			_setDirection(1, 0);

		else if (oldPosition.Y > newPosition.Y)
			_setDirection(-1, 0);

		else if (oldPosition.X < newPosition.X)
			_setDirection(0, 1);

		else
			_setDirection(0, -1);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		switch (other.tag) {
			case "RedPortal":
				StartCoroutine(Scale(new Vector3(1, 1), new Vector3(.1f, .1f), true));
				var redPortal = other.GetComponent<Portal>();
				if (redPortal != null)
					redPortal.Open();
				break;
			default:
				break;
		}
	}

	public void Release() {
		IsActive = false;
		GameManager.Instance.ObjPool.ReleaseGameObject(gameObject);
	}
}