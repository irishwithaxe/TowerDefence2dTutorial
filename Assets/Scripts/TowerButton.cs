using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour {

	[SerializeField]
	private GameObject towerPrefab = null;

	[SerializeField]
	private Sprite sprite = null;

	[SerializeField]
	private int price = -10;

	[SerializeField]
	private Text priceText = null;

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