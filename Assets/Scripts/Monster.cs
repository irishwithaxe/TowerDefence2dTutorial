using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour {

	public void Spawn() {
		transform.position = LevelManager.Instance.BluePortal.transform.position;

		StartCoroutine(Scale(new Vector3(0.1f, 0.1f), new Vector3(1f,1f)));
	}

	public IEnumerator Scale(Vector3 from, Vector3 to) {
		float progress = 0;

		while (progress <= 1) {
			transform.localScale = Vector3.Lerp(from, to, progress);
			progress += Time.deltaTime;
			yield return null;
		}

		transform.localScale = to;
	}
}
