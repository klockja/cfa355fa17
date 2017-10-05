﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualSorterScript : MonoBehaviour 
{
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Player" || col.tag == "Enemy")
		{
			col.GetComponentInChildren <SpriteRenderer> ().sortingOrder = -1;
		}
	}
	void OnTriggerExit2D(Collider2D col)
	{
		if (col.tag == "Player" || col.tag == "Enemy")
		{
			col.GetComponentInChildren <SpriteRenderer> ().sortingOrder = 0;
		}
	}
}
