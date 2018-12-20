﻿using System.Collections;
using System.Collections.Generic;
using Objects;
using UnityEngine;
using Utility;

public class NPCInteractable : InGameObject
{

	public Dialogue StandardDialogue;
	public Dialogue DayOverDialogue;
	public Dialogue PlayingDialogue;
	public List<Dialogue> Dialogues;

	private Canvas _questMarkCanvas;
	private int _questCounter;
	
	public void Start()
	{
		bool hasQuest = false;

		if (StandardDialogue != null) StandardDialogue.ResetReproduced();
		if (DayOverDialogue != null) DayOverDialogue.ResetReproduced();
		if (PlayingDialogue != null) PlayingDialogue.ResetReproduced();

		foreach (Dialogue d in Dialogues)
		{
			d.ResetReproduced();
			if (d.CanStartQuest())
			{
				hasQuest = true;
				_questCounter++;
			}
		}
		
		if (hasQuest)
		{
			_questMarkCanvas = GetComponentInChildren<Canvas>();
			EventManager.StartListening("CheckQuestMark", ShowQuestMark);
			ShowQuestMark();
		}
	}

	public override void Interact()
	{
		Dialogue lastAvailable = LastAvailableDialogue();
		
		if(DayProgressionManager.Instance.IsDayOver() && DayOverDialogue != null) DayOverDialogue.StartDialogue();
		else if (lastAvailable == null && StandardDialogue != null) StandardDialogue.StartDialogue();
		else if (lastAvailable != null)
		{
			lastAvailable.StartDialogue();

			if (lastAvailable.CanStartQuest())
			{
				_questMarkCanvas.enabled = false;
				_questCounter--;

				if (_questCounter == 0)
					EventManager.StopListening("CheckQuestMark", ShowQuestMark);

			}
		}
	}

	public void InstantDialogue(Dialogue dialogue)
	{
		dialogue.StartDialogue();
	}

	private Dialogue LastAvailableDialogue()
	{
		return Dialogues.FindLast(d => d.IsAvailable());
	}

	public override bool IsPlayable()
	{
		if (PlayingDialogue != null) return true;
		return false;
	}

	public override void Play()
	{
		PlayingDialogue.StartDialogue();
	}

	/// <summary>
	/// Checks whether the next dialogue can activate a new quest and activates the quest mark.
	/// </summary>
	private void ShowQuestMark()
	{
		Dialogue lastAvailable = LastAvailableDialogue();

		if (lastAvailable != null && lastAvailable.CanStartQuest())
			_questMarkCanvas.enabled = true;
	}
	
}
