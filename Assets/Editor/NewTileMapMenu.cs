using UnityEngine;
using System.Collections;
using UnityEditor;

public class NewTileMapMenu {

	//Meta tag that connect this item to our menu 
	//this is the location where we want our manue to show up.
	[MenuItem("GameObject/Tile Map")]
	public static void CreateTileMap(){
		//Debug.Log ("Create new tile map menu");
		GameObject go = new GameObject ("Tile Map");
		go.AddComponent<TileMap> ();
	}
}
