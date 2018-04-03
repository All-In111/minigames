﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefCar : MonoBehaviour {

    public float moveSpeed = 2;
	public float endPosition = 2.2f;
	public int actualWay = 0;
	public int lastWay = 0;

	private PolicePursuit policePursuit;
	private bool vertical = true;
	private bool moving = false;
	private bool correctWay = false;
	private Collider2D myCollider = null;

    private bool goUp = true;

    private GameObject falseObject;

    [SerializeField]
    private bool isPolice;

    void Start()
    {
        falseObject = gameObject.transform.GetChild(0).gameObject;
    }

	//*************************************************************************************************
	void Update () {
		if (moving) {
			Move ();
		}
	}

	//*************************************************************************************************
	public void StartMoving(PolicePursuit policePursuit2, int way2){
		myCollider = transform.GetComponent<Collider2D> ();
		moving = true;
        policePursuit = policePursuit2;
		actualWay = way2;
	}

	//*************************************************************************************************
	public bool isMoving(){
		return moving;
	}
	//*************************************************************************************************
	private void Move(){
		if (transform.position.y < endPosition && !correctWay) {
			if (vertical) {
                if (!goUp)
                {
                    transform.Translate(0, -moveSpeed * Time.deltaTime, 0);
                }
                else
                {
                    transform.Translate(0, moveSpeed * Time.deltaTime, 0);
                }
			} else {
				transform.Translate (moveSpeed * Time.deltaTime, 0, 0);
			}
		} else {
			moving = false;
            policePursuit.MovementFinished (correctWay);
		}
	}

    public bool getGoUp()
    {
        return goUp;
    }

    public void setGoUp(bool newGoUp)
    {
        goUp = newGoUp;
    }

    public void setVertical(bool newVertical)
    {
        vertical = newVertical;
    }

	//*************************************************************************************************
	public IEnumerator desactivateCollider(){
		myCollider.enabled = false;
		yield return new WaitForSecondsRealtime (0.1f);
		myCollider.enabled = true;
	}

	//*************************************************************************************************
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Finish" && !isPolice) {
			correctWay = true;
		}
        if (isPolice && col.gameObject.GetComponent<ThiefCar>())
        {
            Debug.LogError("Police Caught you!");
            policePursuit.MovementFinished(false);
        }
	}
	//*************************************************************************************************
	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.tag != "Finish" && !col.gameObject.GetComponent<InverseSpeed>() && !col.gameObject.GetComponent<TrafficLights>()) {
			//SI VA HACIA ARRIBA
			if (vertical) {
                if (col.gameObject.name == "Left") {
					if (transform.position.y >= col.transform.parent.transform.position.y -0.05f) {
						//transform.position = new Vector3 (transform.position.x, col.transform.parent.transform.position.y, transform.position.z);
						vertical = false;
						moveSpeed = Mathf.Abs (moveSpeed);
						lastWay = actualWay;
						actualWay++;
                        falseObject.transform.eulerAngles = new Vector3(falseObject.transform.eulerAngles.x, falseObject.transform.eulerAngles.y, -90);
                    }
				}
			    else if (col.gameObject.name == "Right"){
					if (transform.position.y >= col.transform.parent.transform.position.y-0.05f) {
						//transform.position = new Vector3 (transform.position.x, col.transform.parent.transform.position.y, transform.position.z);
						vertical = false;
						moveSpeed = moveSpeed * -1;
						lastWay = actualWay;
						actualWay--;
                        falseObject.transform.eulerAngles = new Vector3(falseObject.transform.eulerAngles.x, falseObject.transform.eulerAngles.y, 90);
                    }
				}
			}
		//SI VA EN HORIZONTAL
		else {
                if (col.gameObject.name == "Up")
                {
                    if (goUp)
                    {
                        falseObject.transform.eulerAngles = new Vector3(falseObject.transform.eulerAngles.x, falseObject.transform.eulerAngles.y, 0);
                    }
                    else
                    {
                        falseObject.transform.eulerAngles = new Vector3(falseObject.transform.eulerAngles.x, falseObject.transform.eulerAngles.y, 180);
                    }
                }
                if (lastWay < actualWay) {
					if (transform.position.x >= policePursuit.getWays(actualWay).transform.position.x-0.05f) {
						transform.position = new Vector3 (policePursuit.getWays(actualWay).transform.position.x, transform.position.y, transform.position.z);
						moveSpeed = Mathf.Abs (moveSpeed);
						vertical = true;
						StartCoroutine ("desactivateCollider");
					}
				} else {
					if (transform.position.x <= policePursuit.getWays(actualWay).transform.position.x+0.05f) {
						transform.position = new Vector3 (policePursuit.getWays(actualWay).transform.position.x, transform.position.y, transform.position.z);
						moveSpeed = Mathf.Abs (moveSpeed);
						vertical = true;
						StartCoroutine ("desactivateCollider");
					}
				}
			}
		}
	}
}