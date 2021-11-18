
using UnityEngine;
using System.Collections;
//Este script controla las acciones Pinky
public class PinkyController : MonoBehaviour {
	//variables públicas
	public GameObject[] pinkyWpPatrol = new GameObject[4]; //array que guarda la ruta de Pinky en la zona central
	public GameObject[] pinkyWP = new GameObject[20]; //array que guarda la ruta normal de Pinky
	public bool pinkyIsEated, pinkyIsHuntable; //variables de control de si es cazable o si ha sido comido
	//variables públicas estáticas
	public static int curP; //variable de control del waypoint actual
	//variables privadas
	GameObject o; //variable temporal para asignar el waypont al array
	//variables de control de la dirección a la que mira Pinky
	Vector3 up = Vector3.zero, 
			right = new Vector3 (0, 90, 0),
			down = new Vector3 (0, 180, 0),
			left = new Vector3 (0, 270, 0),
			currentDirPinky = Vector3.zero;

	float pinkySpeed, huntableTime, timer, maxTime; //variables varias: velocidad, tiempo en el que es cazable y el tiempo en el que parpadea
	bool pinkyOut = false; //variable para controlar si está o no en la zona central
	Renderer colorPinky; //variable para pasar el color al objeto
	Color myColor; //referencia al color azul al cambiar a modo cazable
	GameObject myMouth; //referencia a la boca de asustado de Pinky

	// Use this for initialization
	void Start () {
		curP = 0; //al iniciar, se inicializa la variable a 0 para que sea el primer waypoint
		pinkySpeed = 4f; //velocidad normal de Pinky (la misma para todos)
		huntableTime = 10f; //tiempo máximo que es cazable Pinky (el mismo para todos)
		maxTime = 0.5f; //tiempo máximo inicial del parpadeo entre colores
		timer = maxTime; //se iguala timer a maxtime para el parpadeo
		pinkyIsHuntable = this.GetComponent<GhostsController>().isHuntable = false; //se guarda el estado inicial de cazable del script de los fantasmas
		pinkyIsEated = this.GetComponent<GhostsController>().isEated = false; //se guarda el estado inicial de comido del script de los fantasmas
		colorPinky = this.GetComponentInChildren<Renderer> (); //se guarda el estado del renderizador de Pinky
		myColor = Game.myBlue; //se guarda el color azul para el modo cazable
		myMouth = this.gameObject.transform.GetChild (0).GetChild (2).gameObject; //se guarda la 'boca' de Pinky, que es hija del hijo de donde está este script
		//se guardan ambas rutas de Pinky
		if (WaypointMovement.wpDone) {
			//Ghost home Pinky
			o = GameObject.Find ("WPx9y9");//0
			pinkyWpPatrol [0] = o;
			o = GameObject.Find ("WPx9y11");//1
			pinkyWpPatrol [1] = o;
			o = GameObject.Find ("WPx9y10");//2
			pinkyWpPatrol [2] = o;
			o = GameObject.Find ("WPx7y10");//3
			pinkyWpPatrol [3] = o;
			//Normal move Pinky
			o = GameObject.Find ("WPx7y10");//0
			pinkyWP[0] = o;
			o = GameObject.Find ("WPx7y7");//1
			pinkyWP[1] = o;
			o = GameObject.Find ("WPx11y7");//2
			pinkyWP[2] = o;
			o = GameObject.Find ("WPx11y13");//3
			pinkyWP[3] = o;
			o = GameObject.Find ("WPx13y13");//4
			pinkyWP[4] = o;
			o = GameObject.Find ("WPx13y15");//5
			pinkyWP[5] = o;
			o = GameObject.Find ("WPx9y15");//6
			pinkyWP[6] = o;
			o = GameObject.Find ("TPR");//7
			pinkyWP[7] = o;
			o = GameObject.Find ("WPx9y5");//8
			pinkyWP[8] = o;
			o = GameObject.Find ("WPx1y5");//9
			pinkyWP[9] = o;
			o = GameObject.Find ("WPx1y2");//10
			pinkyWP[10] = o;
			o = GameObject.Find ("WPx3y2");//11
			pinkyWP[11] = o;
			o = GameObject.Find ("WPx3y11");//12
			pinkyWP[12] = o;
			o = GameObject.Find ("WPx1y11");//13
			pinkyWP[13] = o;
			o = GameObject.Find ("WPx1y18");//14
			pinkyWP[14] = o;
			o = GameObject.Find ("WPx3y18");//15
			pinkyWP[15] = o;
			o = GameObject.Find ("WPx3y13");//16
			pinkyWP[16] = o;
			o = GameObject.Find ("WPx5y13");//17
			pinkyWP[17] = o;
			o = GameObject.Find ("WPx5y11");//18
			pinkyWP[18] = o;
			o = GameObject.Find ("WPx7y11");//19
			pinkyWP[19] = o;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//si el juego no se ha ganado y ha empezado
		if (!Game.wining && Game.start) {
			if (!pinkyOut) PinkyHome (); //si Pinky no ha salido, se mueve dentro de la zona central
			else PinkyMove (); //sino, se mueve normal
			Huntable (); //y se comprueba si es cazable
		}
	}
	//método de control para saber si Pinky es cazable
	void Huntable (){
		pinkyIsHuntable = this.GetComponent<GhostsController> ().isHuntable; //se actualiza el estado de la variable para saber si es cazable
		pinkyIsEated = this.GetComponent<GhostsController> ().isEated; //se actualiza el estado de la variable para saber si ha sido comido
		if (pinkyIsHuntable && !pinkyIsEated) { //si Pinky es cazable y no ha sido comido
			pinkySpeed = 2.5f; //se reduce su velocidad
			huntableTime -= Time.deltaTime; //se activa el tiempo en el que será cazable
			if (huntableTime <= 10f) { //en caso de que el tiempo haya empezado se cambia el color de Pinky y se activa la boca
				colorPinky.material.SetColor ("_Color", myColor);
				myMouth.SetActive (true);
			}
			if (huntableTime <= 3f) { //si el tiempo es inferior a 3 segundos, se llama al parpadeo de colores
				Blink ();
			}
			if (huntableTime <= 0) { //si el tiempo acaba
				huntableTime = 10f; //se vuelve a inicializar la variable de tiempo
				maxTime = 0.5f; //se reinicializa la variable de tiempo de parpadeo
				pinkySpeed = 4f; //se restablece la velocidad de Pinky
				pinkyIsHuntable = false; //Pinky deja de ser cazable
				this.GetComponent<GhostsController> ().isHuntable = pinkyIsHuntable; //pasando el valor al script de los fantasmas
				colorPinky.material.SetColor ("_Color", Game.myPink); //se devuelve al color normal
				myMouth.SetActive (false); //y se le desactiva la boca
			}
		} else if (pinkyIsHuntable && pinkyIsEated) { //en caso de que Pinky sea cazable y haya sido comido
			ResetPinky (); //se llama al método de reseteo
		}
	}
	//método que controla el parpadeo de colores en modo cazable
	void Blink () {
		timer -= Time.deltaTime; //se empieza a descontar un temporizador para intercambiar el color de Pinky
		if (timer <= 0f) { //Al llegar a 0
			if (colorPinky.material.color != Game.myPink) { //si el color no es rosa (el color normal)
				colorPinky.material.SetColor ("_Color", Game.myPink); // se vuelve rosa y se reinicia el temporizador
				maxTime -= 0.05f;
				timer = maxTime;
			} else { //en caso contrario, vuelve al azul y se vuelve a inicializar el temporizador
				colorPinky.material.SetColor ("_Color", myColor);
				timer = maxTime;
			}
		}
		if (maxTime <= 0f) { //cuando se haya reducido el tiempo máximo del temporizador a 0 el temporizador se para
			maxTime = timer = 0;
		}
	}
	//método de control de movimiento de Pinky dentro de la zona central
	void PinkyHome (){
		currentDirPinky = down; //al iniciar Clyde mira abajo
		this.transform.localEulerAngles = currentDirPinky; //y le pasa el valor al objeto en escena
		if (!PacManController.isDeadCtrl) { //si Pacman no ha muerto
			if (this.transform.position != pinkyWpPatrol [curP].transform.position) { //si Pinky no ha llegado al siguiente waypoint
				/*se crea un vector3 que acerca a Clyde al siguiente waypoint con la instrucción de mover adelante según su posición actual,
				 *la posición en la que está el siguiente waypoint y la velocidad en función del tiempo multiplicado por la variable de la velocidad*/
				Vector3 pathP = Vector3.MoveTowards (this.transform.position, pinkyWpPatrol [curP].transform.position, pinkySpeed * Time.fixedDeltaTime);
				this.transform.position = pathP; //una vez calculado el nuevo vector3, Pinky se mueve a esa posición
				//estas comprobaciones hacen que Pinky mire hacia el siguiente waypoint
				if (curP == 0) {
					currentDirPinky = left;
				} else if (curP == 1) {
					currentDirPinky = right;
				} else if (curP >= 2) {
					currentDirPinky = up;
				}
				this.transform.localEulerAngles = currentDirPinky; //pasando el resultado al objeto en escena
			} else { //en caso de que Pinky esté en el waypoint que toca, se modifica la variable para que Pinky vaya al siguiente
				curP++;
				if (curP > 1 && InkyController.curI < 5) { //si Inky no ha llegado al 5o waypoint, Pinky se mueve en la zona central
					curP = 0;
					pinkyOut = false;
				} else if (curP >= 4){ //si no, sale de la zona central
					curP = 0;
					pinkyOut = true;
				}
			}
		} else { //en caso de que Pacman esté muerto, se resetea a Pinky
			ResetPinky ();
		}
	}
	//método de control de movimiento normal de Pinky 
	void PinkyMove (){
		currentDirPinky = left; //al iniciar Pinky mira a la izquierda
		this.transform.localEulerAngles = currentDirPinky; //y le pasa el valor al objeto en escena
		if (!PacManController.isDeadCtrl) { //si Pacman no ha muerto
			if (this.transform.position != pinkyWP [curP].transform.position) { //si Pinky no ha llegado al siguiente waypoint
				if (curP == 7) { //como Pinky va de lado a lado del mapa, al llegar al 7o waypoint, Pinky se mueve hacia adelante
					this.transform.Translate (-Vector3.forward * Time.fixedDeltaTime * pinkySpeed);
					if (this.transform.position.x >= -10f && this.transform.position.x <= -9.5f) { //hasta comprobar que ha llegado al otro lado
						curP = 8; //permitiendo seguir la ruta (aunque a veces, en modo cazable no lo hace bien)
					}
				} else { //en los otros casos..., si Pinky no ha llegado al siguiente waypoint
					/*se crea un vector3 que acerca a Clyde al siguiente waypoint con la instrucción de mover adelante según su posición actual,
				 	 *la posición en la que está el siguiente waypoint y la velocidad en función del tiempo multiplicado por la variable de la velocidad*/
					Vector3 pathP = Vector3.MoveTowards (this.transform.position, pinkyWP [curP].transform.position, pinkySpeed * Time.fixedDeltaTime);
					this.transform.position = pathP; //una vez calculado el nuevo vector3, Pinky se mueve a esa posición
				}
				//estas comprobaciones hacen que Pinky mire hacia el siguiente waypoint
				if (curP == 0 || curP == 1 || curP == 10 || curP == 16 || curP == 18) {
					currentDirPinky = left;
				} else if (curP == 3 || curP == 5 || curP == 7 || curP == 8 || curP == 12 || curP == 14) {
					currentDirPinky = right;
				} else if (curP == 6 || curP == 9 || curP == 13 || curP == 15) {
					currentDirPinky = up;
				} else if (curP == 2 || curP == 4 || curP == 11 || curP == 17 || curP == 19) {
					currentDirPinky = down;
				}
				this.transform.localEulerAngles = currentDirPinky; //pasando el resultado al objeto en escena
			} else { //en caso de que Pinky esté en el waypoint que toca, se modifica la variable para que Pinky vaya al siguiente
				curP = (curP + 1) % pinkyWP.Length; //con un "elegante" operando de 'resto' se "evita" el uso de un if
			}
		} else { //en caso de que Pacman esté muerto, se resetea a Pinky
			ResetPinky ();
		}
	}
	//método que resetea el estado de Pinky
	void ResetPinky (){
		pinkyOut = false; //se reinicia el estado de fuera de la zona central para que haga el moviento de inicio
		pinkySpeed = 4f; //se inicializa la velocidad normal
		huntableTime = 10f; //por si acaso, se reinica la variable de tiempo cazable
		maxTime = 0.5f; //por si acaso, se reinica la variable del temporizador de parpadeo
		timer = maxTime; //por si acaso, se reinicializan las variables del temporizador del parpadeo
		colorPinky.material.SetColor ("_Color", Game.myPink); //por si acaso, se reinicializa el color de Pinky
		myMouth.SetActive (false); //al igual que se desactiva su boca
		curP = 2; //dado que su posición inicial en la zona central no es la 0, primero se inicializa a 2
		this.transform.position = pinkyWpPatrol [curP].transform.position; //y esta se usa para volver a Pinky a la posición inicial
		curP = 0; //y luego se inicializa de nuevo a 0 el índice
		this.transform.localEulerAngles = down; //mirando abajo, por si acaso
		//por si acaso se desactivan los estados de cazable y comido, tanto de este script como del de los fantasmas (pero solo del de Pinky)
		pinkyIsHuntable = false;
		this.GetComponent<GhostsController> ().isHuntable = pinkyIsHuntable;
		pinkyIsEated = false;
		this.GetComponent<GhostsController> ().isEated = pinkyIsEated;
	}
}
