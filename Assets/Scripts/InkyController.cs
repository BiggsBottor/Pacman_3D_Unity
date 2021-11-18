using UnityEngine;
using System.Collections;
//Este script controla las acciones Inky
public class InkyController : MonoBehaviour {
	//variables públicas
	public GameObject[] inkyWpPatrol = new GameObject[4]; //array que guarda la ruta de Inky en la zona central
	public GameObject[] inkyWP = new GameObject[20]; //array que guarda la ruta normal de Inky
	public bool inkyIsEated, inkyIsHuntable; //variables de control de si es cazable o si ha sido comido
	//variables públicas estáticas
	public static int curI;//variable de control del waypoint actual
	//variables privadas
	GameObject o; //variable temporal para asignar el waypont al array
	//variables de control de la dirección a la que mira Inky
	Vector3 up = Vector3.zero, 
			right = new Vector3 (0, 90, 0),
			down = new Vector3 (0, 180, 0),
			left = new Vector3 (0, 270, 0),
			currentDirInky = Vector3.zero;

	float inkySpeed, huntableTime, timer, maxTime; //variables varias: velocidad, tiempo en el que es cazable y el tiempo en el que parpadea
	bool inkyOut = false; //variable para controlar si está o no en la zona central
	Renderer colorInky; //variable para pasar el color al objeto
	Color myColor; //referencia al color azul al cambiar a modo cazable
	GameObject myMouth; //referencia a la boca de asustado de Inky

	// Use this for initialization
	void Start () {
		curI = 0; //al iniciar, se inicializa la variable a 0 para que sea el primer waypoint
		inkySpeed = 4f; //velocidad normal de Inky (la misma para todos)
		huntableTime = 10f; //tiempo máximo que es cazable Inky (el mismo para todos)
		maxTime = 0.5f; //tiempo máximo inicial del parpadeo entre colores
		timer = maxTime; //se iguala timer a maxtime para el parpadeo
		inkyIsHuntable = this.GetComponent<GhostsController>().isHuntable = false; //se guarda el estado inicial de cazable del script de los fantasmas
		inkyIsEated = this.GetComponent<GhostsController>().isEated = false; //se guarda el estado inicial de comido del script de los fantasmas
		colorInky = this.GetComponentInChildren<Renderer> (); //se guarda el estado del renderizador de Inky
		myColor = Game.myBlue; //se guarda el color azul para el modo cazable
		myMouth = this.gameObject.transform.GetChild (0).GetChild (2).gameObject; //se guarda la 'boca' de Inky, que es hija del hijo de donde está este script
		//se guardan ambas rutas de Inky
		if (WaypointMovement.wpDone) {
			//ghost home Inky
			o = GameObject.Find ("WPx9y9");//0
			inkyWpPatrol [0] = o;
			o = GameObject.Find ("WPx9y11");//1
			inkyWpPatrol [1] = o;
			o = GameObject.Find ("WPx9y10");//2
			inkyWpPatrol [2] = o;
			o = GameObject.Find ("WPx7y10");//3
			inkyWpPatrol [3] = o;
			//Normal Move Inky
			o = GameObject.Find ("WPx7y10");//0
			inkyWP[0] = o;
			o = GameObject.Find ("WPx7y11");//1
			inkyWP[1] = o;
			o = GameObject.Find ("WPx5y11");//2
			inkyWP[2] = o;
			o = GameObject.Find ("WPx5y13");//3
			inkyWP[3] = o;
			o = GameObject.Find ("WPx3y13");//4
			inkyWP[4] = o;
			o = GameObject.Find ("WPx3y5");//5
			inkyWP[5] = o;
			o = GameObject.Find ("WPx9y5");//6
			inkyWP[6] = o;
			o = GameObject.Find ("TPL");//7
			inkyWP[7] = o;
			o = GameObject.Find ("WPx9y15");//8
			inkyWP[8] = o;
			o = GameObject.Find ("WPx17y15");//9
			inkyWP[9] = o;
			o = GameObject.Find ("WPx17y18");//10
			inkyWP[10] = o;
			o = GameObject.Find ("WPx19y18");//11
			inkyWP[11] = o;
			o = GameObject.Find ("WPx19y11");//12
			inkyWP[12] = o;
			o = GameObject.Find ("WPx17y11");//13
			inkyWP[13] = o;
			o = GameObject.Find ("WPx17y13");//14
			inkyWP[14] = o;
			o = GameObject.Find ("WPx15y13");//15
			inkyWP[15] = o;
			o = GameObject.Find ("WPx15y9");//16
			inkyWP[16] = o;
			o = GameObject.Find ("WPx13y9");//17
			inkyWP[17] = o;
			o = GameObject.Find ("WPx13y7");//18
			inkyWP[18] = o;
			o = GameObject.Find ("WPx7y7");//19
			inkyWP[19] = o;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//si el juego no se ha ganado y ha empezado
		if (!Game.wining && Game.start) {
			if (!inkyOut) InkyHome (); //si Inky no ha salido, se mueve dentro de la zona central
			else InkyMove (); //sino, se mueve normal
			Huntable (); //y se comprueba si es cazable
		}
	}
	//método de control para saber si Inky es cazable
	void Huntable (){
		inkyIsHuntable = this.GetComponent<GhostsController> ().isHuntable; //se actualiza el estado de la variable para saber si es cazable
		inkyIsEated = this.GetComponent<GhostsController> ().isEated; //se actualiza el estado de la variable para saber si ha sido comido
		if (inkyIsHuntable && !inkyIsEated) { //si Inky es cazable y no ha sido comido
			inkySpeed = 2.5f; //se reduce su velocidad
			huntableTime -= Time.deltaTime; //se activa el tiempo en el que será cazable
			if (huntableTime <= 10f) { //en caso de que el tiempo haya empezado se cambia el color de Inky y se activa la boca
				colorInky.material.SetColor ("_Color", myColor);
				myMouth.SetActive (true);
			}
			if (huntableTime <= 3f) { //si el tiempo es inferior a 3 segundos, se llama al parpadeo de colores
				Blink ();
			}
			if (huntableTime <= 0) { //si el tiempo acaba
				huntableTime = 10f; //se vuelve a inicializar la variable de tiempo
				maxTime = 0.5f; //se reinicializa la variable de tiempo de parpadeo
				inkySpeed = 4f; //se restablece la velocidad de Inky
				inkyIsHuntable = false; //Inky deja de ser cazable
				this.GetComponent<GhostsController> ().isHuntable = inkyIsHuntable; //pasando el valor al script de los fantasmas
				colorInky.material.SetColor ("_Color", Color.cyan); //se devuelve al color normal
				myMouth.SetActive (false); //y se le desactiva la boca
			}
		} else if (inkyIsHuntable && inkyIsEated) { //en caso de que Inky sea cazable y haya sido comido
			ResetInky (); //se llama al método de reseteo
		}
	}
	//método que controla el parpadeo de colores en modo cazable
	void Blink () {
		timer -= Time.deltaTime; //se empieza a descontar un temporizador para intercambiar el color de Inky
		if (timer <= 0f) { //Al llegar a 0
			if (colorInky.material.color != Color.cyan) { //si el color no es cian (el color normal)
				colorInky.material.SetColor ("_Color", Color.cyan); // se vuelve cian y se reinicia el temporizador
				maxTime -= 0.05f;
				timer = maxTime;
			} else { //en caso contrario, vuelve al azul y se vuelve a inicializar el temporizador
				colorInky.material.SetColor ("_Color", myColor);
				timer = maxTime;
			}
		}
		if (maxTime <= 0f) { //cuando se haya reducido el tiempo máximo del temporizador a 0 el temporizador se para
			maxTime = timer = 0;
		}
	}
	//método de control de movimiento de Inky dentro de la zona central
	void InkyHome (){
		currentDirInky = right; //al iniciar Clyde mira a la derecha
		this.transform.localEulerAngles = currentDirInky; //y le pasa el valor al objeto en escena
		if (!PacManController.isDeadCtrl) { //si Pacman no ha muerto
			if (this.transform.position != inkyWpPatrol [curI].transform.position) { //si Inky no ha llegado al siguiente waypoint
				/*se crea un vector3 que acerca a Clyde al siguiente waypoint con la instrucción de mover adelante según su posición actual,
				 *la posición en la que está el siguiente waypoint y la velocidad en función del tiempo multiplicado por la variable de la velocidad*/
				Vector3 pathI = Vector3.MoveTowards (this.transform.position, inkyWpPatrol [curI].transform.position, inkySpeed * Time.fixedDeltaTime);
				this.transform.position = pathI; //una vez calculado el nuevo vector3, Inky se mueve a esa posición
				//estas comprobaciones hacen que Inky mire hacia el siguiente waypoint
				if (curI == 0) {
					currentDirInky = left;
				} else if (curI == 1) {
					currentDirInky = right;
				} else if (curI >= 2) {
					currentDirInky = up;
				}
				this.transform.localEulerAngles = currentDirInky; //pasando el resultado al objeto en escena
			} else { //en caso de que Inky esté en el waypoint que toca, se modifica la variable para que Inky vaya al siguiente
				curI++;
				if (curI > 1 && BlinkyController.curB < 5) { //si Blinky no ha llegado al 5o waypoint, Inky se mueve en la zona central
					curI = 0;
					inkyOut = false;
				} else if (curI >= 4){ //si no, sale de la zona central
					curI = 0;
					inkyOut = true;
				}
			}
		} else { //en caso de que Pacman esté muerto, se resetea a Inky
			ResetInky ();
		}
	}
	//método de control de movimiento normal de Inky
	void InkyMove (){
		currentDirInky = right; //al iniciar Clyde mira a la derecha
		this.transform.localEulerAngles = currentDirInky; //y le pasa el valor al objeto en escena
		if (!PacManController.isDeadCtrl) { //si Pacman no ha muerto
			if (this.transform.position != inkyWP [curI].transform.position) { //si Inky no ha llegado al siguiente waypoint
				if (curI == 7) { //como Inky va de lado a lado del mapa, al llegar al 7o waypoint, Inky se mueve hacia adelante
					this.transform.Translate (-Vector3.forward * Time.fixedDeltaTime * inkySpeed);
					if (this.transform.position.x <= 9f && this.transform.position.x >= 8.5f) { //hasta comprobar que ha llegado al otro lado
						curI = 8; //permitiendo seguir la ruta (aunque a veces, en modo cazable no lo hace bien)
					}
				} else { //en los otros casos..., si Inky no ha llegado al siguiente waypoint
					/*se crea un vector3 que acerca a Clyde al siguiente waypoint con la instrucción de mover adelante según su posición actual,
				 *la posición en la que está el siguiente waypoint y la velocidad en función del tiempo multiplicado por la variable de la velocidad*/
					Vector3 pathI = Vector3.MoveTowards (this.transform.position, inkyWP [curI].transform.position, inkySpeed * Time.fixedDeltaTime);
					this.transform.position = pathI; //una vez calculado el nuevo vector3, Inky se mueve a esa posición
				}
				//estas comprobaciones hacen que Inky mire hacia el siguiente waypoint
				if (curI == 5 || curI == 7 || curI == 8 || curI == 12 || curI == 16 || curI == 18) {
					currentDirInky = left;
				} else if (curI == 0 || curI == 1 || curI == 10 || curI == 14) {
					currentDirInky = right;
				} else if (curI == 2 || curI == 4 || curI == 13 || curI == 15 || curI == 17 || curI == 19) {
					currentDirInky = up;
				} else if (curI == 6 || curI == 9 || curI == 11) {
					currentDirInky = down;
				}
				this.transform.localEulerAngles = currentDirInky; //pasando el resultado al objeto en escena
			} else { //en caso de que Inky esté en el waypoint que toca, se modifica la variable para que Inky vaya al siguiente
				curI = (curI + 1) % inkyWP.Length; //con un "elegante" operando de 'resto' se "evita" el uso de un if
			}
		} else { //en caso de que Pacman esté muerto, se resetea a Inky
			ResetInky ();
		}
	}
	//método que resetea el estado de Inky
	void ResetInky (){
		inkyOut = false; //se reinicia el estado de fuera de la zona central para que haga el moviento de inicio
		inkySpeed = 4f; //se inicializa la velocidad normal
		huntableTime = 10f; //por si acaso, se reinica la variable de tiempo cazable
		maxTime = 0.5f; //por si acaso, se reinica la variable del temporizador de parpadeo
		timer = maxTime; //por si acaso, se reinicializan las variables del temporizador del parpadeo
		colorInky.material.SetColor ("_Color", Color.cyan); //por si acaso, se reinicializa el color de Inky
		myMouth.SetActive (false); //al igual que se desactiva su boca
		curI = 0; //se vuelve a la posición inicial con la variable del índice del array
		this.transform.position = inkyWpPatrol [curI].transform.position; //y esta se usa para volver a Inky a la posición inicial
		this.transform.localEulerAngles = right; //mirando a la derecha, por si acaso
		//por si acaso se desactivan los estados de cazable y comido, tanto de este script como del de los fantasmas (pero solo del de Inky)
		inkyIsHuntable = false;
		this.GetComponent<GhostsController> ().isHuntable = inkyIsHuntable;
		inkyIsEated = false;
		this.GetComponent<GhostsController> ().isEated = inkyIsEated;
	}
}
