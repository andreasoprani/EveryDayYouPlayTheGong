﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

	[Header("Movement")]
	public float Velocity = 2f;
	public float RunIncrement = 1.2f;
	public float CrawlDecrement = 0.7f;

	[Header("Collisions")] 
	public RayCastPositions RayCastPositions;
	public LayerMask ObstacleLayer;
	public LayerMask InteractiveLayer;
	public LayerMask PlayableLayer;

	[Header("Interaction")] public float InteractionDetectionRadius = 1f;
	
	// Stores all the keycodes for the player's actions.
	private IDictionary<string, KeyCode> _actionKeyCodes;
	
	private Transform _tr;

	// Direction in which the player is facing.
	private Vector3 facing;
	
	// Use this for initialization
	void Start ()
	{
		_tr = GetComponent<Transform>();

		//TODO: load keys from file + add edit
		_actionKeyCodes = new Dictionary<string, KeyCode>()
		{
			{"Run", KeyCode.LeftControl},
			{"Crawl", KeyCode.LeftShift},
			{"Play", KeyCode.Space},
			{"Interact", KeyCode.X}
		};
		
		facing = Vector3.up;

	}
	
	// Update is called once per frame
	void Update ()
	{
		Move();

		GameObject interactiveObject = HasInteraction();
		
		if (interactiveObject != null && Input.GetKeyDown(_actionKeyCodes["Interact"]))
		{
			Interact(interactiveObject);
		} else if (Input.GetKeyDown(_actionKeyCodes["Play"]))
		{
			Play();
		}
		
	}

	/// <summary>
	/// Calculates the velocity of the player and updates the position.
	/// Must be called once per frame.
	/// </summary>
	void Move()
	{
		float movementVelocity = Velocity;
		
		if (Input.GetKey(_actionKeyCodes["Run"]) && !Input.GetKey(_actionKeyCodes["Crawl"]))
			movementVelocity *= RunIncrement;
		if (Input.GetKey(_actionKeyCodes["Crawl"]))
			movementVelocity *= CrawlDecrement;
		
		float deltaHorizontal = Input.GetAxis("Horizontal") * movementVelocity * Time.deltaTime;
		float deltaVertical = Input.GetAxis("Vertical") * movementVelocity * Time.deltaTime;
		
		//TODO: update facing.
		
		if (CanMoveHorizontally(deltaHorizontal))
		{
			_tr.position += new Vector3(deltaHorizontal, 0, 0);
		} 
		if (CanMoveVertically(deltaVertical))
		{
			_tr.position += new Vector3(0, deltaVertical, 0);
		}
	}

	/// <summary>
	/// Checks if the player can move horizontally.
	/// </summary>
	/// <param name="delta">the quantity of movement intended to perform</param>
	/// <returns>true if the player can move of delta horizontally, false otherwise.</returns>
	bool CanMoveHorizontally(float delta)
	{
		Vector3 position = Vector3.zero;

		if (delta > 0) position = RayCastPositions.Right.position; // Going right
		else if (delta < 0) position = RayCastPositions.Left.position; // Going left

		RaycastHit2D hit = Physics2D.Raycast(position, _tr.right * Math.Sign(delta), delta, ObstacleLayer);

		if (hit.collider != null) return false;
		else return true;
	}

	/// <summary>
	/// Checks if the player can move vertically.
	/// </summary>
	/// <param name="delta">the quantity of movement intended to perform</param>
	/// <returns>true if the player can move of delta vertically, false otherwise.</returns>
	bool CanMoveVertically(float delta)
	{
		Vector3 position = Vector3.zero;

		if (delta > 0) position = RayCastPositions.Up.position; // Going up
		else if (delta < 0) position = RayCastPositions.Down.position; // Going down

		RaycastHit2D hit = Physics2D.Raycast(position, _tr.up * Math.Sign(delta), delta, ObstacleLayer);

		if (hit.collider != null) return false;
		else return true;
	}

	void Play()
	{
		//TODO: implement
	}

	/// <summary>
	/// Triggers an event called "Interact" + name of the GameObject.
	/// </summary>
	/// <param name="obj">GameObject to interact with.</param>
	void Interact(GameObject obj)
	{
		// Triggers the event to interact with the object.
		EventManager.TriggerEvent("Interact" + obj.ToString());
	}

	/// <summary>
	/// Allows to detect an interactive object in the facing direction (within the InteractionDetectionRadius range).
	/// </summary>
	/// <returns>the object detected or null if nothing was detected.</returns>
	GameObject HasInteraction()
	{
		Vector3 position = RayCastPositions.Vector3ToRaycastPosition(facing).position;

		RaycastHit2D hit = Physics2D.Raycast(position, facing, InteractionDetectionRadius, InteractiveLayer);

		if (hit.collider != null)
		{
			EventManager.TriggerEvent("InteractionGUIElement");
			return hit.collider.transform.parent.gameObject;
		}
		else return null;

	}
	
}
