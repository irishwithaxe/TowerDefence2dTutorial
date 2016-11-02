using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : Singletone<GameManager> {

	private TowerButton _clickedBtn;

	private int _currency;

	[SerializeField]
	private Text CurrecyText = null;

	public GameObject TowerPrefab {
		get {
			if (_clickedBtn == null)
				return null;
			return _clickedBtn.TowerPrefab;
		}
	}

	public int Currency {
		get {
			return _currency;
		}

		set {
			_currency = value;
			CurrecyText.text = string.Format(@" {0} <color=lime>$</color>", Currency);
		}
	}

	// Use this for initialization
	void Start() {
		Currency = 5;
	}

	// Update is called once per frame
	void Update() {
		HandleInput();
	}

	public void TowerPick(TowerButton tb) {
		if (tb.Price > Currency)
			return;

		_clickedBtn = tb;
		Hover.Instance.Activate(tb.Sprite);
	}

	public void BuyTower() {
		Currency = Currency - _clickedBtn.Price;
		TowerUnpick();
	}

	private void TowerUnpick() {
		Hover.Instance.Deactivate();
		_clickedBtn = null;
	}

	private void HandleInput() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			HandleEscape();
			HandleRightMouseBtn();
		}
		if (Input.GetMouseButtonDown(1))
			HandleRightMouseBtn();
	}

	private void HandleEscape() {

	}

	private void HandleRightMouseBtn() {
		TowerUnpick();
	}
}
