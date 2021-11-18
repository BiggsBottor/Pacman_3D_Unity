using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GhostController : MonoBehaviour {

//	Vector3 up = Vector3.zero, 
//			right = new Vector3 (0, 90, 0),
//			down = new Vector3 (0, 180, 0),
//			left = new Vector3 (0, 270, 0),
//			currentDirBlinky = Vector3.zero,
//			currentDirInky = Vector3.zero,
//			currentDirPinky = Vector3.zero,
//			currentDirClyde = Vector3.zero;
	public static int curB, curI, curP, curC;
	public float blinkySpeed, InkySpeed, PinkySpeed, ClydeSpeed;
	public string dir;
	//public List<GameObject> blinkyWP, InkyWP, InkyWpPatrol, PinkyWP, PinkyWpPatrol, ClydeWP, ClydeWpPatrol;
	//GameObject o = null;
	//bool InkyOut, PinkyOut, ClydeOut = false;



	// Use this for initialization
	void Start () {
		curB = curC = curI = curP = 0;
		blinkySpeed = InkySpeed = PinkySpeed = ClydeSpeed = 4f;
		/*Estructura para la patrulla de los fantasmas
		if (this.gameObject.name == "Blinky") {
//			//Normal Move
//			o = GameObject.Find ("WPx7y10");//0
//			blinkyWP.Add (o);
//			o = GameObject.Find ("WPx7y7");//1
//			blinkyWP.Add (o);
//			o = GameObject.Find ("WPx13y7");//2
//			blinkyWP.Add (o);
//			o = GameObject.Find ("WPx13y5");//3
//			blinkyWP.Add (o);
//			o = GameObject.Find ("WPx17y5");//4
//			blinkyWP.Add (o);
//			o = GameObject.Find ("WPx17y2");//5
//			blinkyWP.Add (o);
//			o = GameObject.Find ("WPx19y2");//6
//			blinkyWP.Add (o);
//			o = GameObject.Find ("WPx19y9");//7
//			blinkyWP.Add (o);
//			o = GameObject.Find ("WPx17y9");//8
//			blinkyWP.Add (o);
//			o = GameObject.Find ("WPx17y7");//9
//			blinkyWP.Add (o);
//			o = GameObject.Find ("WPx15y7");//10
//			blinkyWP.Add (o);
//			o = GameObject.Find ("WPx15y5");//11
//			blinkyWP.Add (o);
//			o = GameObject.Find ("WPx1y5");//12
//			blinkyWP.Add (o);
//			o = GameObject.Find ("WPx1y2");//13
//			blinkyWP.Add (o);
//			o = GameObject.Find ("WPx3y2");//14
//			blinkyWP.Add (o);
//			o = GameObject.Find ("WPx3y18");//15
//			blinkyWP.Add (o);
//			o = GameObject.Find ("WPx1y18");//16
//			blinkyWP.Add (o);
//			o = GameObject.Find ("WPx1y15");//17
//			blinkyWP.Add (o);
//			o = GameObject.Find ("WPx9y15");//18
//			blinkyWP.Add (o);
//			o = GameObject.Find ("WPx9y13");//19
//			blinkyWP.Add (o);
//			o = GameObject.Find ("WPx7y13");//20
//			blinkyWP.Add (o);
		}
		if (this.gameObject.name == "Inky") {
//			//ghost home
//			o = GameObject.Find ("WPx9y9");//0
//			InkyWpPatrol.Add (o);
//			o = GameObject.Find ("WPx9y11");//1
//			InkyWpPatrol.Add (o);
//			o = GameObject.Find ("WPx9y10");//2
//			InkyWpPatrol.Add (o);
//			o = GameObject.Find ("WPx7y10");//3
//			InkyWpPatrol.Add (o);
//			//Normal Move
//			o = GameObject.Find ("WPx7y10");//0
//			InkyWP.Add (o);
//			o = GameObject.Find ("WPx7y11");//1
//			InkyWP.Add (o);
//			o = GameObject.Find ("WPx5y11");//2
//			InkyWP.Add (o);
//			o = GameObject.Find ("WPx5y13");//3
//			InkyWP.Add (o);
//			o = GameObject.Find ("WPx3y13");//4
//			InkyWP.Add (o);
//			o = GameObject.Find ("WPx3y5");//5
//			InkyWP.Add (o);
//			o = GameObject.Find ("WPx9y5");//6
//			InkyWP.Add (o);
//			o = GameObject.Find ("TPL");//7
//			InkyWP.Add (o);
//			o = GameObject.Find ("WPx9y15");//8
//			InkyWP.Add (o);
//			o = GameObject.Find ("WPx17y15");//9
//			InkyWP.Add (o);
//			o = GameObject.Find ("WPx17y18");//10
//			InkyWP.Add (o);
//			o = GameObject.Find ("WPx19y18");//11
//			InkyWP.Add (o);
//			o = GameObject.Find ("WPx19y11");//12
//			InkyWP.Add (o);
//			o = GameObject.Find ("WPx17y11");//13
//			InkyWP.Add (o);
//			o = GameObject.Find ("WPx17y13");//14
//			InkyWP.Add (o);
//			o = GameObject.Find ("WPx15y13");//15
//			InkyWP.Add (o);
//			o = GameObject.Find ("WPx15y9");//16
//			InkyWP.Add (o);
//			o = GameObject.Find ("WPx13y9");//17
//			InkyWP.Add (o);
//			o = GameObject.Find ("WPx13y7");//18
//			InkyWP.Add (o);
//			o = GameObject.Find ("WPx7y7");//19
//			InkyWP.Add (o);
		}
		if (this.gameObject.name == "Pinky") {
//			//ghost home Pinky
//			o = GameObject.Find ("WPx9y9");//0
//			PinkyWpPatrol.Add (o);
//			o = GameObject.Find ("WPx9y11");//1
//			PinkyWpPatrol.Add (o);
//			o = GameObject.Find ("WPx9y10");//2
//			PinkyWpPatrol.Add (o);
//			o = GameObject.Find ("WPx7y10");//3
//			PinkyWpPatrol.Add (o);
//			//Normal move Pinky
//			o = GameObject.Find ("WPx7y10");//0
//			PinkyWP.Add (o);
//			o = GameObject.Find ("WPx7y7");//1
//			PinkyWP.Add (o);
//			o = GameObject.Find ("WPx11y7");//2
//			PinkyWP.Add (o);
//			o = GameObject.Find ("WPx11y13");//3
//			PinkyWP.Add (o);
//			o = GameObject.Find ("WPx13y13");//4
//			PinkyWP.Add (o);
//			o = GameObject.Find ("WPx13y15");//5
//			PinkyWP.Add (o);
//			o = GameObject.Find ("WPx9y15");//6
//			PinkyWP.Add (o);
//			o = GameObject.Find ("TPR");//7
//			PinkyWP.Add (o);
//			o = GameObject.Find ("WPx9y5");//8
//			PinkyWP.Add (o);
//			o = GameObject.Find ("WPx1y5");//9
//			PinkyWP.Add (o);
//			o = GameObject.Find ("WPx1y2");//10
//			PinkyWP.Add (o);
//			o = GameObject.Find ("WPx3y2");//11
//			PinkyWP.Add (o);
//			o = GameObject.Find ("WPx3y11");//12
//			PinkyWP.Add (o);
//			o = GameObject.Find ("WPx1y11");//13
//			PinkyWP.Add (o);
//			o = GameObject.Find ("WPx1y18");//14
//			PinkyWP.Add (o);
//			o = GameObject.Find ("WPx3y18");//15
//			PinkyWP.Add (o);
//			o = GameObject.Find ("WPx3y13");//16
//			PinkyWP.Add (o);
//			o = GameObject.Find ("WPx5y13");//17
//			PinkyWP.Add (o);
//			o = GameObject.Find ("WPx5y11");//18
//			PinkyWP.Add (o);
//			o = GameObject.Find ("WPx7y11");//19
//			PinkyWP.Add (o);
		}
		if (this.gameObject.name == "Clyde") {
//			//ghost home
//			o = GameObject.Find ("WPx9y11");//0
//			ClydeWpPatrol.Add (o);
//			o = GameObject.Find ("WPx9y9");//1
//			ClydeWpPatrol.Add (o);
//			o = GameObject.Find ("WPx9y10");//2
//			ClydeWpPatrol.Add (o);
//			o = GameObject.Find ("WPx7y10");//3
//			ClydeWpPatrol.Add (o);
//			//Normal move
//			o = GameObject.Find ("WPx7y10");//0
//			ClydeWP.Add (o);
//			o = GameObject.Find ("WPx7y11");//1
//			ClydeWP.Add (o);
//			o = GameObject.Find ("WPx5y11");//2
//			ClydeWP.Add (o);
//			o = GameObject.Find ("WPx5y13");//3
//			ClydeWP.Add (o);
//			o = GameObject.Find ("WPx3y13");//4
//			ClydeWP.Add (o);
//			o = GameObject.Find ("WPx3y11");//5
//			ClydeWP.Add (o);
//			o = GameObject.Find ("WPx1y11");//6
//			ClydeWP.Add (o);
//			o = GameObject.Find ("WPx1y18");//7
//			ClydeWP.Add (o);
//			o = GameObject.Find ("WPx5y18");//8
//			ClydeWP.Add (o);
//			o = GameObject.Find ("WPx5y15");//9
//			ClydeWP.Add (o);
//			o = GameObject.Find ("WPx17y15");//10
//			ClydeWP.Add (o);
//			o = GameObject.Find ("WPx17y18");//11
//			ClydeWP.Add (o);
//			o = GameObject.Find ("WPx19y18");//12
//			ClydeWP.Add (o);
//			o = GameObject.Find ("WPx19y2");//13
//			ClydeWP.Add (o);
//			o = GameObject.Find ("WPx17y2");//14
//			ClydeWP.Add (o);
//			o = GameObject.Find ("WPx17y3");//15
//			ClydeWP.Add (o);
//			o = GameObject.Find ("WPx15y3");//16
//			ClydeWP.Add (o);
//			o = GameObject.Find ("WPx15y2");//17
//			ClydeWP.Add (o);
//			o = GameObject.Find ("WPx13y2");//18
//			ClydeWP.Add (o);
//			o = GameObject.Find ("WPx13y7");//19
//			ClydeWP.Add (o);
//			o = GameObject.Find ("WPx7y7");//20
//			ClydeWP.Add (o);
		}
		*/
	}
	
	// Update is called once per frame
	void FixedUpdate () {
//		BlinkyMove ();
//
//		if (!InkyOut) InkyHome ();
//		else InkyMove ();
//
//		if (!PinkyOut) PinkyHome ();
//		else PinkyMove ();
//
//		if (!ClydeOut) ClydeHome ();
//		else ClydeMove ();
		//DirToStringDebug ();
	}
	/*
	void BlinkyMove (){
		if (this.gameObject.name == "Blinky"){
			currentDirBlinky = left;
			this.transform.localEulerAngles = currentDirBlinky;
			if (!PacManController.isDeadCtrl) {
				if (this.transform.position != listGhosts.blinkyWP [curB].transform.position) {
					Vector3 pathB = Vector3.MoveTowards (this.transform.position, listGhosts.blinkyWP [curB].transform.position, blinkySpeed * Time.fixedDeltaTime);
					this.transform.position = pathB;
					if (curB == 0 || curB == 1 || curB == 3 || curB == 5 || curB == 9 || curB == 11 || curB == 13 || curB == 17 || curB == 19) {
						currentDirBlinky = left;
					} else if (curB == 7 || curB == 15) {
						currentDirBlinky = right;
					} else if (curB == 8 || curB == 10 || curB == 12 || curB == 16 || curB == 20) {
						currentDirBlinky = up;
					} else if (curB == 2 || curB == 4 || curB == 6 || curB == 14 || curB == 18) {
						currentDirBlinky = down;
					}
					this.transform.localEulerAngles = currentDirBlinky;
				} else {
					curB = (curB + 1) % listGhosts.blinkyWP.Count;
				}
			} else {
				curB = 0;
				this.transform.position = listGhosts.blinkyWP [curB].transform.position;
				this.transform.localEulerAngles = left;
			}

		}
	}

	void InkyHome (){
		if (this.gameObject.name == "Inky") {
			currentDirInky = right;
			this.transform.localEulerAngles = currentDirInky;
			if (!PacManController.isDeadCtrl) {
				if (this.transform.position != listGhosts.InkyWpPatrol [curI].transform.position) {
					Vector3 pathI = Vector3.MoveTowards (this.transform.position, listGhosts.InkyWpPatrol [curI].transform.position, InkySpeed * Time.fixedDeltaTime);
					this.transform.position = pathI;
					if (curI == 0) {
						currentDirInky = left;
					} else if (curI == 1) {
						currentDirInky = right;
					} else if (curI >= 2) {
						currentDirInky = up;
					}
					this.transform.localEulerAngles = currentDirInky;
				} else {
					curI++;
					if (curI > 1 && curB < 5) {
						curI = 0;
						InkyOut = false;
					} else if (curI >= 4){
						curI = 0;
						InkyOut = true;
					}
				}
			} else {
				InkyOut = false;
				curI = 0;
				this.transform.position = listGhosts.InkyWpPatrol [curI].transform.position;
				this.transform.localEulerAngles = right;
			}
		}
	}

	void InkyMove (){
		if (this.gameObject.name == "Inky") {
			currentDirInky = right;
			this.transform.localEulerAngles = currentDirInky;
			if (!PacManController.isDeadCtrl) {
				if (this.transform.position != listGhosts.InkyWP [curI].transform.position) {
					if (curI == 7) {
						this.transform.Translate (-Vector3.forward * Time.fixedDeltaTime * InkySpeed);
						if (this.transform.position.x <= 9f && this.transform.position.x >= 8.5f) {
							curI = 8;
						}
					} else {
						Vector3 pathI = Vector3.MoveTowards (this.transform.position, listGhosts.InkyWP [curI].transform.position, InkySpeed * Time.fixedDeltaTime);
						this.transform.position = pathI;
					}
					if (curI == 5 || curI == 7 || curI == 8 || curI == 12 || curI == 16 || curI == 18) {
						currentDirInky = left;
					} else if (curI == 0 || curI == 1 || curI == 10 || curI == 14) {
						currentDirInky = right;
					} else if (curI == 2 || curI == 4 || curI == 13 || curI == 15 || curI == 17 || curI == 19) {
						currentDirInky = up;
					} else if (curI == 6 || curI == 9 || curI == 11) {
						currentDirInky = down;
					}
					this.transform.localEulerAngles = currentDirInky;
				} else {
					curI = (curI + 1) % listGhosts.InkyWP.Count;
				}
			} else {
				InkyOut = false;
				curI = 0;
				this.transform.position = listGhosts.InkyWpPatrol [curI].transform.position;
				this.transform.localEulerAngles = right;
			}
		}
	}

	void PinkyHome (){
		if (this.gameObject.name == "Pinky") {
			currentDirPinky = down;
			this.transform.localEulerAngles = currentDirPinky;
			if (!PacManController.isDeadCtrl) {
				if (this.transform.position != listGhosts.PinkyWpPatrol [curP].transform.position) {
					Vector3 pathP = Vector3.MoveTowards (this.transform.position, listGhosts.PinkyWpPatrol [curP].transform.position, PinkySpeed * Time.fixedDeltaTime);
					this.transform.position = pathP;
					if (curP == 0) {
						currentDirPinky = left;
					} else if (curP == 1) {
						currentDirPinky = right;
					} else if (curP >= 2) {
						currentDirPinky = up;
					}
					this.transform.localEulerAngles = currentDirPinky;
				} else {
					curP++;
					if (curP > 1 && curI < 5) {
						curP = 0;
						PinkyOut = false;
					} else if (curP >= 4){
						curP = 0;
						PinkyOut = true;
					}
				}
			} else {
				PinkyOut = false;
				curP = 2;
				this.transform.position = listGhosts.PinkyWpPatrol [curP].transform.position;
				curP = 0;
				this.transform.localEulerAngles = down;
			}
		}
	}

	void PinkyMove (){
		if (this.gameObject.name == "Pinky") {
			currentDirPinky = left;
			this.transform.localEulerAngles = currentDirPinky;
			if (!PacManController.isDeadCtrl) {
				if (this.transform.position != listGhosts.PinkyWP [curP].transform.position) {
					if (curP == 7) {
						this.transform.Translate (-Vector3.forward * Time.fixedDeltaTime * PinkySpeed);
						if (this.transform.position.x >= -10f && this.transform.position.x <= -9.5f) {
							curP = 8;
						}
					} else {
						Vector3 pathP = Vector3.MoveTowards (this.transform.position, listGhosts.PinkyWP [curP].transform.position, PinkySpeed * Time.fixedDeltaTime);
						this.transform.position = pathP;
					}
					if (curP == 0 || curP == 1 || curP == 10 || curP == 16 || curP == 18) {
						currentDirPinky = left;
					} else if (curP == 3 || curP == 5 || curP == 7 || curP == 8 || curP == 12 || curP == 14) {
						currentDirPinky = right;
					} else if (curP == 6 || curP == 9 || curP == 13 || curP == 15) {
						currentDirPinky = up;
					} else if (curP == 2 || curP == 4 || curP == 11 || curP == 17 || curP == 19) {
						currentDirPinky = down;
					}
					this.transform.localEulerAngles = currentDirPinky;
				} else {
					curP = (curP + 1) % listGhosts.PinkyWP.Count;
				}
			} else {
				PinkyOut = false;
				curP = 2;
				this.transform.position = listGhosts.PinkyWpPatrol [curP].transform.position;
				curP = 0;
				this.transform.localEulerAngles = down;
			}
		}
	}

	void ClydeHome(){
		if (this.gameObject.name == "Clyde") {
			currentDirClyde = left;
			this.transform.localEulerAngles = currentDirClyde;
			if (!PacManController.isDeadCtrl) {
				if (this.transform.position != listGhosts.ClydeWpPatrol [curC].transform.position) {
					Vector3 pathC = Vector3.MoveTowards (this.transform.position, listGhosts.ClydeWpPatrol [curC].transform.position, ClydeSpeed * Time.fixedDeltaTime);
					this.transform.position = pathC;
					if (curC == 1) {
						currentDirClyde = left;
					} else if (curC == 0) {
						currentDirClyde = right;
					} else if (curC >= 2) {
						currentDirClyde = up;
					}
					this.transform.localEulerAngles = currentDirClyde;
				} else {
					curC++;
					if (curC > 1 && curP < 5) {
						curC = 0;
						ClydeOut = false;
					} else if (curC >= 4){
						curC = 0;
						ClydeOut = true;
					}
				}
			} else {
				ClydeOut = false;
				curC = 0;
				this.transform.position = listGhosts.ClydeWpPatrol [curC].transform.position;
				this.transform.localEulerAngles = left;
			}
		}
	}

	void ClydeMove(){
		if (this.gameObject.name == "Clyde"){
			currentDirClyde = left;
			this.transform.localEulerAngles = currentDirClyde;
			Debug.Log ("curC " + curC + " Dir " + currentDirClyde);
			if (!PacManController.isDeadCtrl) {
				if (this.transform.position != listGhosts.ClydeWP [curC].transform.position) {
					Vector3 pathC = Vector3.MoveTowards (this.transform.position, listGhosts.ClydeWP [curC].transform.position, ClydeSpeed * Time.fixedDeltaTime);
					this.transform.position = pathC;
					if (curC == 5 || curC == 9 || curC == 13 || curC == 17) {
						currentDirClyde = left;
					} else if (curC == 0 || curC == 1 || curC == 3 || curC == 7 || curC == 11 || curC == 15 || curC == 19) {
						currentDirClyde = right;
					} else if (curC == 2 || curC == 4 || curC == 6 || curC == 14 || curC == 16 || curC == 18 || curC == 20) {
						currentDirClyde = up;
					} else if (curC == 8 || curC == 10 || curC == 12) {
						currentDirClyde = down;
					}
					this.transform.localEulerAngles = currentDirClyde;
				} else {
					curC = (curC + 1) % listGhosts.ClydeWP.Count;
				}
			} else {
				curC = 0;
				this.transform.position = listGhosts.ClydeWP [curC].transform.position;
				this.transform.localEulerAngles = left;
			}
		}
	}
*/
//	void DirToStringDebug (){
//		if (currentDirClyde == left)
//			dir = "Left";
//		else if (currentDirClyde == right)
//			dir = "Right";
//		else if (currentDirClyde == up)
//			dir = "Up";
//		else if (currentDirClyde == down)
//			dir = "Down";
//	}
}
