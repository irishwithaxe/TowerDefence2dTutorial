using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singletone<GameManager> {
	private TowerButton _clickedBtn;

	public ObjectPool ObjPool { get; set; }

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

	private void Awake() {
		ObjPool = GetComponent<ObjectPool>();
	}

	public int Currency {
		get {
			return _currency;
		}

		set {
			_currency = value;
			CurrecyText.text = string.Format(@" {0} <color=lime>$</color>", _currency);
		}
	}

	// Use this for initialization
	private void Start() {
		Currency = 20;
	}

	// Update is called once per frame
	private void Update() {
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
		if (!Input.GetKey(KeyCode.LeftControl) || _clickedBtn.Price > Currency)
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

	public void StartWave() {
		StartCoroutine(SpawnWave());
	}

	private IEnumerator SpawnWave() {
		LevelManager.Instance.GeneratePath();

		var monsterType = string.Empty;

		var index = 3;// Random.Range(0, 4);
		switch (index) {
			case 0:
				monsterType = "MonsterBlack";
				break;

			case 1:
				monsterType = "MonsterBlue";
				break;

			case 2:
				monsterType = "MonsterGreen";
				break;

			case 3:
				monsterType = "MonsterRed";
				break;

			default:
				throw new System.NotImplementedException("Неожиданное значение типа монстра: " + index);
		}

		var monster = ObjPool.GetObject(monsterType).GetComponent<Monster>();
		monster.Spawn();

		yield return new WaitForSeconds(2.5f);
	}
}