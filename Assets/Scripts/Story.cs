﻿using UnityEngine;
using System.Collections;

public class Story : MonoBehaviour {

	private bool isScrolling; // We'll use this for debugging
	private float rotation;   // Default 55deg, but read in from canvas

	// Use this for initialization
	void Start () {
		Setup();
	}
	
	// Update is called once per frame
	void Update () {
		// Check for starting or stopping
		if(Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			isScrolling = !isScrolling;
		}

		// Check if the user wants to quit the application
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

		// If we are scrolling, perform update action
		if (isScrolling)
		{
			// Get the current transform position of the panel
			Vector3 _currentUIPosition = gameObject.transform.position;
//			Debug.Log("Current Positon: " + _currentUIPosition);

			// Increment the Y value of the panel 
			Vector3 _incrementYPosition = 
				new Vector3(_currentUIPosition.x + .019f * Mathf.Sin(Mathf.Deg2Rad * rotation),
					_currentUIPosition.y + .019f * Mathf.Cos(Mathf.Deg2Rad * rotation) ,
					_currentUIPosition.z );

			// Change the transform position to the new one
//			Debug.Log("New Position: " + _incrementYPosition);
			gameObject.transform.position = _incrementYPosition;      
		}
		if (gameObject.transform.position.x > 150) {
			gameObject.SetActive (false);
		}
	}

	void Setup() {
		isScrolling = true;
		rotation = gameObject.GetComponentInParent<Transform>().eulerAngles.x;
	}
}
