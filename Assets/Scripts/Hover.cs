using UnityEngine;

public class Hover : Singletone<Hover> {
	private SpriteRenderer spriteRenderer = null;

	// Use this for initialization
	private void Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.enabled = false;
	}

	// Update is called once per frame
	private void Update() {
		FollowMouse();
	}

	private void FollowMouse() {
		if (!spriteRenderer.enabled)
			return;

		var newpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		newpos.z = 0;
		transform.position = newpos;
	}

	public void Activate(Sprite sprite) {
		spriteRenderer.sprite = sprite;
		spriteRenderer.enabled = true;
	}

	public void Deactivate() {
		spriteRenderer.enabled = false;
	}
}