﻿using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour {

	private Transform player;
	private Transform key;
	private GameObject mentos;

	public delegate void DoorOpen ();
	public static event DoorOpen onDoorOpen;

	void Awake () {
		
	}

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player").transform;
		key = GameObject.FindWithTag("key").transform;
		mentos = GameObject.FindWithTag ("MentosT");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LateUpdate()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 10))
			{
//				string tag = hit.transform.gameObject.tag;
                print("test");
				key = player.transform.Find("key");
				if (hit.transform.gameObject == gameObject && key != null) //if key child was found, do this
				{
                    print("tutorial door open");
					gameObject.transform.FindChild("door_tutorial").transform.Rotate(Vector3.up, 95); //rotate the door
					mentos.SetActive(false); //delete Mentos in tutorial room
					key.gameObject.SetActive(false);

					onDoorOpen ();
				}
				else
				{
					print("key not found");
				}
			}
		}
	}
}
