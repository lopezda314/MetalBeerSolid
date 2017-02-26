﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraScript: MonoBehaviour {

	public Texture2D crosshairTexture;

	public Transform CameraTarget;
	private float x = 0.0f;
	private float y = 0.0f;

	private float mouseXSpeedMod = .8f;
	private float mouseYSpeedMod = .8f;

	public float MaxViewDistance = 15f;
	public float MinViewDistance = 1f;
	public int ZoomRate;
//	private int lerpRate = 5;
	private float distance = 3f;
	private float desireDistance;
	private float correctedDistance;
	private float currentDistance;

	public float cameraTargetHeight = 1.0f;
    public Text controlsText;
    CursorLockMode wantedMode;

	//checks if first person mode is on
	private bool click = false;
	//stores cameras distance from player
	private float curDist = 0;

	void Awake () {
		wantedMode = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	// Use this for initialization
	void Start () {
		Vector3 Angles = transform.eulerAngles;
		x = Angles.x;
		y = Angles.y;
		currentDistance = distance;
		desireDistance = distance;
		correctedDistance = distance;

        wantedMode = CursorLockMode.Locked;
//        Cursor.visible = true;
		Cursor.lockState = wantedMode;
		Cursor.visible = false;
        
	}


	// Update is called once per frame
	void LateUpdate () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
            {
                wantedMode = CursorLockMode.Locked;
                Time.timeScale = 1;
            }
            else
            {
                wantedMode = CursorLockMode.Locked;
                Time.timeScale = 0;
            }
        }
        Cursor.lockState = wantedMode;
        if (Time.timeScale == 1)
        {
            controlsText.enabled = false;
			Cursor.visible = false;
//			Screen.lockCursor = true;
            x += Input.GetAxis("Mouse X") * mouseXSpeedMod;
            y += -Input.GetAxis("Mouse Y") * mouseYSpeedMod;
            /*if (Input.GetMouseButton (0)) {//0 mouse btn izq, 1 mouse btn der
                 x += Input.GetAxis("Mouse X") * mouseXSpeedMod;
                y += Input.GetAxis("Mouse Y") * mouseYSpeedMod;
            }else
                if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                float targetRotantionAngle = CameraTarget.eulerAngles.y;
                float cameraRotationAngle = transform.eulerAngles.y;
                x = Mathf.LerpAngle(cameraRotationAngle,targetRotantionAngle, lerpRate * Time.deltaTime);
            }*/

            y = ClampAngle(y, -10, 90);
            Quaternion rotation = Quaternion.Euler(y, x, 0);

            desireDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * ZoomRate * Mathf.Abs(desireDistance);
            desireDistance = Mathf.Clamp(desireDistance, MinViewDistance, MaxViewDistance);
            correctedDistance = desireDistance;

            Vector3 position = CameraTarget.position - (rotation * Vector3.forward * desireDistance);

            RaycastHit collisionHit;
            Vector3 cameraTargetPosition = new Vector3(CameraTarget.position.x, CameraTarget.position.y + cameraTargetHeight, CameraTarget.position.z);

            bool isCorrected = false;
            if (Physics.Linecast(cameraTargetPosition, position, out collisionHit))
            {
                position = collisionHit.point;
                correctedDistance = Vector3.Distance(cameraTargetPosition, position);
                isCorrected = true;
            }

            //?
            //condicion ? first_expresion : second_expresion;
            //(input > 0) ? isPositive : isNegative;

            currentDistance = !isCorrected || correctedDistance > currentDistance ? Mathf.Lerp(currentDistance, correctedDistance, Time.deltaTime * ZoomRate) : correctedDistance;

            position = CameraTarget.position - (rotation * Vector3.forward * currentDistance + new Vector3(0, -cameraTargetHeight, 0));

            transform.rotation = rotation;
            transform.position = position;

            //CameraTarget.rotation = rotation;

            float cameraX = transform.rotation.x;
            //checks if right mouse button is pushed
            //		if(Input.GetMouseButton(2))
            //		{
            //sets CHARACTERS x rotation to match cameras x rotation. If commented out, the player doesn't change its rotation with camera 
            CameraTarget.eulerAngles = new Vector3(cameraX, transform.eulerAngles.y, transform.eulerAngles.z);

            //		}
            //checks if middle mouse button is pushed down
            if (Input.GetMouseButtonDown(2))
            {
                //if middle mouse button is pressed 1st time set click to true and camera in front of player and save cameras position before mmb.
                //if mmb is pressed again set camera back to it's position before we clicked mmb 1st time and set click to false
                if (click == false)
                {
                    click = true;
                    curDist = distance;
                    distance = distance - distance - 1;
                }
                else
                {
                    distance = curDist;
                    click = false;
                }
            }
        }
        else
        {
            Cursor.visible = true;
            controlsText.enabled = true;
        }

	}


    void OnGUI()
    {
//		GUI.color = Color.white;
//		GUI.backgroundColor = Color.white;
//		Texture2D texture = new Texture2D(1, 1);
//		texture.SetPixel(0,0,Color.white);
//		texture.Apply();
//		GUI.skin.box.normal.background = texture;
		GUI.DrawTexture(new Rect((Screen.width - crosshairTexture.width) / 2, (Screen.height - crosshairTexture.height) / 2, crosshairTexture.width, crosshairTexture.height), crosshairTexture);
    }

	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360) {
			angle += 360;      
		}
		if (angle > 360) {
			angle -= 360;      
		}
		return Mathf.Clamp (angle,min,max);
	}
}
