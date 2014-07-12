using UnityEngine;
using System.Collections;

public static class ScreenText {

	public static GUIText text;
	public static TextControll textControll;

	static ScreenText() {
		text = GameObject.FindGameObjectWithTag("Text").GetComponent<GUIText>();
		textControll = GameObject.FindGameObjectWithTag("Text").GetComponent<TextControll>();
	}
}
