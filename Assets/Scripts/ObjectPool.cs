using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

	[SerializeField]
	private GameObject[] objectsPrefabs = new GameObject[0];

	private Dictionary<string, Queue<GameObject>> pool = new Dictionary<string, Queue<GameObject>>();

	public GameObject GetObject(string type) {
		GameObject go = null;
		Queue<GameObject> queue;

		if (pool.TryGetValue(type, out queue)) {
			go = queue.Dequeue();
			go.SetActive(true);

			if (queue.Count == 0)
				pool.Remove(type);
		}
		else
			for (int i = 0; i < objectsPrefabs.Length; i++) {
				if (objectsPrefabs[i].name == type) {
					go = Instantiate(objectsPrefabs[i]) as GameObject;
					go.name = type;
					break;
				}
			}

		return go;
	}

	public void ReleaseGameObject(GameObject go) {
		go.SetActive(false);

		Queue<GameObject> queue;
		if (pool.TryGetValue(go.name, out queue))
			queue.Enqueue(go);
		else {
			queue = new Queue<GameObject>();
			queue.Enqueue(go);
			pool.Add(go.name, queue);
		}
	}
}