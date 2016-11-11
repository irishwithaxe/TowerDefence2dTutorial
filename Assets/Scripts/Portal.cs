using UnityEngine;

public class Portal : MonoBehaviour {
	private Animator animator;

	// Use this for initialization
	private void Start() {
		animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	private void Update() {
	}

	public void Open() {
		animator.SetTrigger("Open");
	}
}