using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour {

	[SerializeField]
	private GameObject towerPrefab = null;

	[SerializeField]
	private Sprite sprite = null;

	[SerializeField]
	private int price;

	[SerializeField]
	private Text priceText;

	public Sprite Sprite { get { return sprite; } }

	public GameObject TowerPrefab { get { return towerPrefab; } }

	public int Price {
		get {
			return price;
		}
	}

	private void Start() {
		priceText.text = string.Format(@" {0} <color=lime>$</color>", Price);
	}
}
