﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SBSlider : MonoBehaviour {

    private GameManager gameManager;
    public GameObject nail;
    public GameObject safeBox;
    private int direction;
    public Sprite winEndGameSprite;
    public Sprite loseEndGameSprite;

    public void init(GameManager gm)
    {
        gameManager = gm;
    }



    // Use this for initialization
    void Start () {
        direction = 1;
        //EndGameSprite.get
    }
	
	// Update is called once per frame
	void Update () {

        nail.transform.Rotate(0, 0, Time.deltaTime * 250 * direction, Space.Self);
        //Debug.Log(nail.transform.rotation.z);

        if (nail.transform.rotation.z >= 0.82 && nail.transform.rotation.z <= 0.85)
        {
            direction = -1;

        }
        else if(nail.transform.rotation.z <= -0.82 && nail.transform.rotation.z >= -0.85)
        {
            direction = 1;
        }

        if (InputManager.Instance.GetButtonDown(InputManager.MiniGameButtons.BUTTON1))
        {
            if(nail.transform.rotation.z <= 0.15 && nail.transform.rotation.z >= -0.15)
            {
                safeBox.GetComponent<SpriteRenderer>().sprite = winEndGameSprite;
                Debug.Log("WIN");
                gameManager.EndGame(IMiniGame.MiniGameResult.WIN);
            }
            else
            {
                safeBox.GetComponent<SpriteRenderer>().sprite = loseEndGameSprite;
                gameManager.EndGame(IMiniGame.MiniGameResult.LOSE);
            }
        }
    }
}
