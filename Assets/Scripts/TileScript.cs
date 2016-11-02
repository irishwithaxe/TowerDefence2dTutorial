using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour {

	public Point GridPosition { get; private set; }

	private Color32 fullColor = new Color32(255, 119, 119, 255);

	private Color32 emptyColor = new Color32(96, 255, 90, 255);

	private SpriteRenderer spriteRdr = null;

	public bool IsEmpty { get; set; }

	public bool Walkable { get; set; }

	public Vector2 WoldPosition {
		get {
			var size = GetComponent<SpriteRenderer>().bounds.size;
			return new Vector2(transform.position.x + size.x / 2, transform.position.y - size.y / 2);
		}
	}

	public Color DefaultColor {
		get {
			return defaultColor;
		}

		set {
			defaultColor = value;
			ColorTile(defaultColor);
		}
	}

	// Use this for initialization
	void Start() {
		spriteRdr = GetComponent<SpriteRenderer>();
		IsEmpty = true;
		Walkable = true;
		DefaultColor = Color.white;
	}

	// Update is called once per frame
	void Update() {

	}

	public void Setup(int x, int y, float tileSize, Vector3 worldPos, Transform parent) {
		GridPosition = new Point(x, y);

		transform.position = new Vector3(tileSize * x + worldPos.x, worldPos.y - tileSize * y, 0);
		transform.SetParent(parent);

		LevelManager.Instance.Tiles[x, y] = this;
	}

	private Color defaultColor;

	private void OnMouseOver() {
		if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.TowerPrefab != null) {

			if (IsEmpty)
				ColorTile(emptyColor);
			else
				ColorTile(fullColor);

			if (IsEmpty && Input.GetMouseButtonDown(0))
				PlaceTower();
		}
	}

	private void OnMouseExit() {
		ColorTile(DefaultColor);
	}

	private void PlaceTower() {
		var tower = Instantiate(GameManager.Instance.TowerPrefab, WoldPosition, Quaternion.identity) as GameObject;
		tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;
		tower.transform.SetParent(transform);

		GameManager.Instance.BuyTower();
		Walkable = false;
		IsEmpty = false;
	}

	private void ColorTile(Color color) {
		spriteRdr.color = color;
	}
}
