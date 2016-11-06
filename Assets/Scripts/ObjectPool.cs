using UnityEngine;
using System.Collections;

public class ObjectPool : MonoBehaviour {

	[SerializeField]
	private GameObject[] objectsPrefabs;

	public GameObject GetObject(string type) {

		for (int i = 0; i < objectsPrefabs.Length; i++) {
			if (objectsPrefabs[i].name == type) {
				var newObj = Instantiate(objectsPrefabs[i]) as GameObject;
				newObj.name = type;
				return newObj;
			}
		}

		return null;
	}
}
