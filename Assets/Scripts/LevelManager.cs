using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singletone<LevelManager> {

	[SerializeField]
	private GameObject[] tiles = new GameObject[0];

	[SerializeField]
	private CameraMovement camMove = null;

	[SerializeField]
	private GameObject bluePortal = null;

	[SerializeField]
	private Transform map = null;

	[SerializeField]
	private GameObject redPortal = null;

	private Point blueSpawn;
	private Point redSpawn;

	public TileScript[,] Tiles;

	private Stack<Node> finalPath;

	public Node[] Path {
		get {
			if (finalPath == null)
				GeneratePath();

			return finalPath.ToArray();
		}
	}

	private float _tilexsize = -1f;

	public float TileXSize {
		get {
			if (_tilexsize < 0) {
				_tilexsize = tiles[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
				//_tilexsize += _tilexsize * 0.1f;
			}
			return _tilexsize;
		}
	}

	// Use this for initialization
	private void Start() {
		CreateLevel();
	}

	// Update is called once per frame
	private void Update() {
	}

	public Portal BluePortal { get; set; }

	private void CreateLevel() {
		var worldPos = Camera.main.ScreenToWorldPoint(new Vector3(0f, Screen.height));

		int rows, cols = 0;
		var mapdata = ReadLevelMap(out rows, out cols);
		Tiles = new TileScript[cols, rows];

		var bottomLeftTilePos = Vector3.zero;
		for (int y = 0; y < rows; y++) {
			var charr = mapdata[y].ToCharArray();
			for (int x = 0; x < cols; x++) {
				PlaceTile(charr[x], x, y, worldPos);
			}
		}

		bottomLeftTilePos = Tiles[cols - 1, rows - 1].transform.position;

		camMove.SetLimits(new Vector3(bottomLeftTilePos.x + TileXSize, bottomLeftTilePos.y - TileXSize));

		SpawnPortals(cols, rows);
	}

	private string[] ReadLevelMap(out int rows, out int cols) {
		var rawdata = Resources.Load("Level") as TextAsset;
		var data = rawdata.text.Replace(Environment.NewLine, "#");
		var dataarr = data.Split(new[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
		rows = dataarr.Length;
		cols = dataarr[0].ToCharArray().Length;
		return dataarr;
	}

	private void PlaceTile(char index, int x, int y, Vector3 worldPos) {
		int ind = index - '0';

		var newTile = Instantiate(tiles[ind]);
		var tilescript = newTile.GetComponent<TileScript>();

		tilescript.Setup(x, y, TileXSize, worldPos, map);
	}

	private Vector3 GetTilePos(Point tilepos) {
		return Tiles[tilepos.X, tilepos.Y].transform.position;
	}

	private Vector3 GetTileWorldPos(Point tilepos) {
		return Tiles[tilepos.X, tilepos.Y].WoldPosition;
	}

	private void SpawnPortals(int cols, int rows) {
		blueSpawn = new Point(1, 1);
		var bp = Instantiate(bluePortal, GetTileWorldPos(blueSpawn), Quaternion.identity) as GameObject;
		BluePortal = bp.GetComponent<Portal>();
		BluePortal.name = "BluePortal";

		redSpawn = new Point(cols - 2, rows - 2);
		Instantiate(redPortal, GetTileWorldPos(redSpawn), Quaternion.identity);
	}

	public void GeneratePath() {
		finalPath = AStar.GetPath(blueSpawn, redSpawn);
	}
}