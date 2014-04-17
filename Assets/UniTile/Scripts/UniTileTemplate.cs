using UnityEngine;
using System.Collections;

[System.Serializable]
public class UniTileTemplate 
{
	public string name;
	[HideInInspector] public TileInstance[] selectedTilesList	= new TileInstance[0];
	[HideInInspector] public int[] selectedTiles				= new int[0];
	[HideInInspector] public int selectedTilesWidth				= 0;
	[HideInInspector] public TileInstance selectedTile 			= new TileInstance(0);
	[HideInInspector] public TileInstance selectedTileEnd 		= new TileInstance(0);
	[HideInInspector] public bool tilesPicked 					= false;
	
	public int selectedTilesHeight {
		get {
			return selectedTilesList.Length / selectedTilesWidth;
		}
	}
	
	public void Init(UniTileTemplate other) 
	{
		other.CopyTo(this);
		this.name = "Template "+UniTileManager.instance.templateCount;
	}
	
	public void CopyTo(UniTileTemplate other)
	{
		other.name 						= name;
		other.selectedTilesList 		= new TileInstance[this.selectedTilesList.Length];
		for(int i = 0; i < this.selectedTilesList.Length; i++) {
			other.selectedTilesList[i]	= this.selectedTilesList[i].Clone();
		}
		other.selectedTilesWidth 		= selectedTilesWidth;
		other.selectedTile 				= selectedTile.Clone();
		other.selectedTileEnd 			= selectedTileEnd.Clone();
		other.tilesPicked 				= tilesPicked;
	}
	
	public void CopyInto(UniTileTemplate other) {
		for(int i = 0; i < other.selectedTilesWidth; i++) {
			for(int j = 0; j < other.selectedTilesList.Length / other.selectedTilesWidth; j++) {
				other.selectedTilesList[i + j * other.selectedTilesWidth] = new TileInstance(-1);
				
				int prevIndex = i + j * selectedTilesWidth;
				
				if(selectedTilesList.Length > 0) {
					if(i < selectedTilesWidth) {
						if(j < selectedTilesList.Length / selectedTilesWidth) {
							other.selectedTilesList[i + j * other.selectedTilesWidth] = selectedTilesList[prevIndex].Clone();
						}
					}
				}
			}
		}
	}
	
	public TileInstance GetTileData(int x, int y) {
		if(this.selectedTilesList == null 
		   || this.selectedTilesList.Length <= x + y * this.selectedTilesWidth
		   || x < 0 || x >= selectedTilesWidth || y < 0)
			return null;
		
		return this.selectedTilesList[x + y * this.selectedTilesWidth];
	}
	
	public void Clear()
	{
		selectedTilesWidth = 1;
		selectedTilesList = new TileInstance[1];
		selectedTilesList[0] = new TileInstance(0);
	}
	
	public void Resize(int x, int y) {
		TileInstance[] newList = new TileInstance[x * y];
		
		for(int i = 0; i < x; i++) {
			for(int j = 0; j < y; j++) {
				newList[i + j * x] = new TileInstance(-1);
				
				int prevIndex = i + j * selectedTilesWidth;
				
				if(selectedTilesList.Length > 0) {
					if(i < selectedTilesWidth) {
						if(j < selectedTilesList.Length / selectedTilesWidth) {
							newList[i + j * x] = selectedTilesList[prevIndex];
						}
					}
				}
			}
		}
		
		this.selectedTilesWidth = x;
		this.selectedTilesList = newList;
	}
}
