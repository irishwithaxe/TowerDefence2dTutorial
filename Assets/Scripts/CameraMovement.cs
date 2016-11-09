using UnityEngine;

public class CameraMovement : MonoBehaviour {

	[SerializeField]
	private float cameraSpeed = 0f;

	private float maxX = 0f;
	private float minY = 0f;

	// Update is called once per frame
	private void Update() {
		GetInput();
	}

	private void GetInput() {
		if (Input.GetKey(KeyCode.W)) {
			transform.Translate(Vector3.up * cameraSpeed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.S)) {
			transform.Translate(Vector3.down * cameraSpeed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.A)) {
			transform.Translate(Vector3.left * cameraSpeed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.D)) {
			transform.Translate(Vector3.right * cameraSpeed * Time.deltaTime);
		}

		transform.position = new Vector3(Mathf.Clamp(transform.position.x, -1f, maxX), Mathf.Clamp(transform.position.y, minY - 1f, 0), transform.position.z);
	}

	public void SetLimits(Vector3 limitPosition) {
		var bottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f));
		//var topLeft = Camera.main.ViewportToWorldPoint(new Vector3(0f, 1f));
		maxX = limitPosition.x - bottomRight.x;
		minY = limitPosition.y - bottomRight.y;
	}
}