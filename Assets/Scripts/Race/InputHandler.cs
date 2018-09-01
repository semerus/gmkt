﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public Action OnUpKey;
    public Action OnDownKey;
    public Action OnLeftKey;
    public Action OnRightKey;
    public Action OnInputFinish;
    
    // Update is called once per frame
	void Update () {
        HandleInput(KeyCode.UpArrow, null, OnUpKey, null);
	    HandleInput(KeyCode.DownArrow, null, OnDownKey, null);
	    HandleInput(KeyCode.LeftArrow, null, OnLeftKey, null);
	    HandleInput(KeyCode.RightArrow, null, OnRightKey, null);

	    if (OnInputFinish != null)
	    {
	        OnInputFinish();
	    }
    }

    void HandleInput(KeyCode key, Action keyDown, Action onKey, Action keyUp)
    {
        if (Input.GetKeyDown(key))
        {
            if (keyDown != null)
            {
                keyDown();
            }
        }

        if (Input.GetKey(key))
        {
            if (onKey != null)
            {
                onKey();
            }
        }

        if (Input.GetKeyUp(key))
        {
            if (keyUp != null)
            {
                keyUp();
            }
        }
    }
}