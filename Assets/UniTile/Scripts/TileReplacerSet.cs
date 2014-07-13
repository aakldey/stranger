using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileReplacerSet : ScriptableObject {
	
	public UniTileTileset tileset;
	public List<TileReplacerRule> rules = new List<TileReplacerRule>(new TileReplacerRule[] {new TileReplacerRule("New Rule")});
	
	public bool entireMap = true;
	public Rect area = new Rect(2, 2, 10, 10);
}

[System.Serializable]
public class TileReplacerRule {
	
	public string name;
	
	public bool visible = true;
	public Vector2 size = new Vector2(1, 1);
	
	public UniTileTemplate source = new UniTileTemplate();
	public UniTileTemplate destination = new UniTileTemplate();
	
	public bool active = true;
	
	public float chance = 100;
	
	public TileReplacerRule(string name) {
		this.name = name;
		this.source.name = "Source";
		this.destination.name = "Destination";
		
		this.source.Clear();
		this.destination.Clear();
		this.source.selectedTilesList[0].id = -1;
		this.destination.selectedTilesList[0].id = -1;
	}
	
	public TileReplacerRule Clone() {
		TileReplacerRule newRule = new TileReplacerRule(this.name + (this.name.Contains("(Clone)") ? "" : " (Clone)"));
		newRule.visible = this.visible;
		newRule.size = this.size;
		this.source.CopyTo(newRule.source);
		this.destination.CopyTo(newRule.destination);
		newRule.active = this.active;
		newRule.chance = this.chance;
		
		
		return newRule;
	}
}