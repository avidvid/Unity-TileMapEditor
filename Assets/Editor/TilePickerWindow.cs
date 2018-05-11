using UnityEngine;
using System.Collections;
using UnityEditor;

public class TilePickerWindow : EditorWindow {

	public enum Scale
	{
		x1,
		x2,
		x3,
		x4,
		x5
	}

	Scale scale;
	Vector2 currentSelection = Vector2.zero;

	public Vector2 scrollPosition = Vector2.zero;

	[MenuItem("Window/Tile Picker")]
	public static void OpenTilePickerWindow(){

		//Debug.Log ("Create new windows menu");
		var window = EditorWindow.GetWindow(typeof(TilePickerWindow));
		var title = new GUIContent ();
		title.text = "Tile Picker";
		window.titleContent = title;
	}

	void OnGUI(){
		if (Selection.activeGameObject == null)
			return;

		//var selection = Selection.activeGameObject.GetComponent<TileMap> ();
		var selection = ((GameObject)Selection.activeGameObject).GetComponent<TileMap> ();

		if (selection != null) {
			var texture2D = selection.texture2D;
			if(texture2D != null){
				//1 With original size 
				//GUI.DrawTexture(new Rect(0, 0 , texture2D.width, texture2D.height), texture2D);
				//2 With Scale dropdown resizing zoom 
				scale = (Scale)EditorGUILayout.EnumPopup("Zoom", scale);
				var newScale = ((int)scale) + 1; //first value is 0 + 1 = 1
				var newTextureSize = new Vector2(texture2D.width, texture2D.height) * newScale;
				//Dropdown to be in the middle of windows 
				var offset = new Vector2(10 , 25);

				//Add Scroll Bar
					//Size of the window 
				var viewPort = new Rect(0,0, position.width-5, position.height-5);
					//Object that we are showing in the windows 
				var contentSize = new Rect(0,0, newTextureSize.x + offset.x, newTextureSize.y + offset.y);
				scrollPosition = GUI.BeginScrollView(viewPort, scrollPosition, contentSize);
				GUI.DrawTexture(new Rect(offset.x, offset.y , newTextureSize.x, newTextureSize.y), texture2D);
				//It ends to GUI.EndScrollView(); at the end 


				//highlight the tile in the picker 
				var tile = selection.tileSize * newScale;

				//For padding
				tile.x += selection.tilePadding.x * newScale;
				tile.y += selection.tilePadding.y * newScale;

				var grid = new Vector2(newTextureSize.x / tile.x, newTextureSize.y / tile.y);

				var selectionPos = new Vector2(tile.x * currentSelection.x + offset.x,
				                               tile.y * currentSelection.y + offset.y);

				//Style the texture box 
				var boxTex = new Texture2D(1,1);
				boxTex.SetPixel(0,0, new Color(0, 0.5f, 1f, 0.4f));
				boxTex.Apply();
				var style = new GUIStyle(GUI.skin.customStyles[0]);
				style.normal.background = boxTex;

				GUI.Box (new Rect(selectionPos.x, selectionPos.y, tile.x, tile.y), "", style);


				//Moving the slected box 
				var cEvent = Event.current;
				Vector2 mousePos = new Vector2(cEvent.mousePosition.x, cEvent.mousePosition.y);
				if(cEvent.type == EventType.MouseDown && cEvent.button == 0){
					currentSelection.x = Mathf.Floor((mousePos.x + scrollPosition.x) / tile.x);
					currentSelection.y = Mathf.Floor((mousePos.y + scrollPosition.y) / tile.y);

					if(currentSelection.x > grid.x -1)
						currentSelection.x = grid.x -1;

					if(currentSelection.y > grid.y -1)
						currentSelection.y = grid.y -1;

					selection.tileID = (int)(currentSelection.x + (currentSelection.y * grid.x) + 1);

					Repaint();
				}

				GUI.EndScrollView();
			}
		}
	}
}
