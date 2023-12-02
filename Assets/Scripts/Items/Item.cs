using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item
{
	public enum ItemType
	{
		Rock,
		Wood,
		Gold
	}

	public ItemType itemType;
	public int ammount;
}