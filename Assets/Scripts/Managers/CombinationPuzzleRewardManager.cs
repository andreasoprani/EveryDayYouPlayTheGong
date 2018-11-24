﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinationPuzzleRewardManager : MonoBehaviour
{

	public List<CombinationPuzzleObject> PuzzlePiece;
	public Pickup Reward;
	
	//TO BE DELETED WHEN PLAYER BECOMES A SINGLETON
	public Player John;

	private int _currentIndex;

	public void Selection(CombinationPuzzleObject guess)
	{
		Debug.Log(PuzzlePiece.IndexOf(guess)+" "+_currentIndex+ " "+ PuzzlePiece.Count );
		if (PuzzlePiece.IndexOf(guess) == _currentIndex)
		{
			_currentIndex++;
			if (_currentIndex  == PuzzlePiece.Count)
				PuzzleSolved();
		}
		else
		{
			_currentIndex = 0;
		}
		Debug.Log(_currentIndex);
	}

	private void PuzzleSolved()
	{
		foreach (CombinationPuzzleObject item in PuzzlePiece)
		{
			item.setSolved(true);
			
		}
		Reward.Interact(John);
	}
}
