using UnityEngine;
using UnityEngine.UI;

public class DebugTile : MonoBehaviour {

	[SerializeField]
	private Text ftext;

	[SerializeField]
	private Text gtext;

	[SerializeField]
	private Text htext;

	public Text F {
		get {
			return ftext;
		}

		set {
			ftext = value;
		}
	}

	public Text G {
		get {
			return gtext;
		}

		set {
			gtext = value;
		}
	}

	public Text H {
		get {
			return htext;
		}

		set {
			htext = value;
		}
	}
}