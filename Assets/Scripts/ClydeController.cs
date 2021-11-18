using UnityEngine;
using System.Collections;
//Este script controla las acciones Clyde
public class ClydeController : MonoBehaviour {
	//variables públicas
	public GameObject[] clydeWpPatrol = new GameObject[4]; //array que guarda la ruta de Clyde en la zona central
	public GameObject[] clydeWP = new GameObject[21]; //array que guarda la ruta normal de Clyde 
	public bool clydeIsEated, clydeIsHuntable; //variables de control de si es cazable o si ha sido comido
	//variables públicas estáticas
	public static int curC; //variable de control del waypoint actual
	//variables privadas
	GameObject o; //variable temporal para asignar el waypont al array
	//variables de control de la dirección a la que mira Clyde
	Vector3 up = Vector3.zero, 
			right = new Vector3 (0, 90, 0),
			down = new Vector3 (0, 180, 0),
			left = new Vector3 (0, 270, 0),
			currentDirClyde = Vector3.zero;

	float clydeSpeed, huntableTime, timer, maxTime; //variables varias: velocidad, tiempo en el que es cazable y el tiempo en el que parpadea
	bool clydeOut = false; //variable para controlar si está o no en la zona central
	Renderer colorClyde; //variable para pasar el color al objeto
	Color myColor; //referencia al color azul al cambiar a modo cazable
	GameObject myMouth; //referencia a la boca de asustado de Clyde

	// Use this for initialization
	void Start () {
		curC = 0; //al iniciar, se inicializa la variable a 0 para que sea el primer waypoint
		clydeSpeed = 4f; //velocidad normal de Clyde (la misma para todos)
		huntableTime = 10f; //tiempo máximo que es cazable Clyde (el mismo para todos)
		maxTime = 0.5f; //tiempo máximo inicial del parpadeo entre colores
		timer = maxTime; //se iguala timer a maxtime para el parpadeo
		clydeIsHuntable = this.GetComponent<GhostsController>().isHuntable = false; //se guarda el estado inicial de cazable del script de los fantasmas
		clydeIsEated = this.GetComponent<GhostsController>().isEated = false; //se guarda el estado inicial de comido del script de los fantasmas
		colorClyde = this.GetComponentInChildren<Renderer> (); //se guarda el estado del renderizador de Clyde
		myColor = Game.myBlue; //se guarda el color azul para el modo cazable
		myMouth = this.gameObject.transform.GetChild (0).GetChild (2).gameObject; //se guarda la 'boca' de Clyde, que es hija del hijo de donde está este script
		//se guardan ambas rutas de Clyde
		if (WaypointMovement.wpDone) {
			//ghost home Clyde
			o = GameObject.Find ("WPx9y11");//0
			clydeWpPatrol [0] = o;
			o = GameObject.Find ("WPx9y9");//1
			clydeWpPatrol [1] = o;
			o = GameObject.Find ("WPx9y10");//2
			clydeWpPatrol [2] = o;
			o = GameObject.Find ("WPx7y10");//3
			clydeWpPatrol [3] = o;
			//Normal move Clyde
			o = GameObject.Find ("WPx7y10");//0
			clydeWP [0] = o;
			o = GameObject.Find ("WPx7y11");//1
			clydeWP [1] = o;
			o = GameObject.Find ("WPx5y11");//2
			clydeWP [2] = o;
			o = GameObject.Find ("WPx5y13");//3
			clydeWP [3] = o;
			o = GameObject.Find ("WPx3y13");//4
			clydeWP [4] = o;
			o = GameObject.Find ("WPx3y11");//5
			clydeWP [5] = o;
			o = GameObject.Find ("WPx1y11");//6
			clydeWP [6] = o;
			o = GameObject.Find ("WPx1y18");//7
			clydeWP [7] = o;
			o = GameObject.Find ("WPx5y18");//8
			clydeWP [8] = o;
			o = GameObject.Find ("WPx5y15");//9
			clydeWP [9] = o;
			o = GameObject.Find ("WPx17y15");//10
			clydeWP [10] = o;
			o = GameObject.Find ("WPx17y18");//11
			clydeWP [11] = o;
			o = GameObject.Find ("WPx19y18");//12
			clydeWP [12] = o;
			o = GameObject.Find ("WPx19y2");//13
			clydeWP [13] = o;
			o = GameObject.Find ("WPx17y2");//14
			clydeWP [14] = o;
			o = GameObject.Find ("WPx17y3");//15
			clydeWP [15] = o;
			o = GameObject.Find ("WPx15y3");//16
			clydeWP [16] = o;
			o = GameObject.Find ("WPx15y2");//17
			clydeWP [17] = o;
			o = GameObject.Find ("WPx13y2");//18
			clydeWP [18] = o;
			o = GameObject.Find ("WPx13y7");//19
			clydeWP [19] = o;
			o = GameObject.Find ("WPx7y7");//20
			clydeWP [20] = o;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//si el juego no se ha ganado y ha empezado
		if (!Game.wining && Game.start) {
			if (!clydeOut) ClydeHome (); //si Clyde no ha salido, se mueve dentro de la zona central
			else ClydeMove (); //sino, se mueve normal
			Huntable (); //y se comprueba si es cazable
		}
	}
	//método de control para saber si Clyde es cazable
	void Huntable (){
		clydeIsHuntable = this.GetComponent<GhostsController> ().isHuntable; //se actualiza el estado de la variable para saber si es cazable
		clydeIsEated = this.GetComponent<GhostsController> ().isEated; //se actualiza el estado de la variable para saber si ha sido comido
		if (clydeIsHuntable && !clydeIsEated) { //si Clyde es cazable y no ha sido comido
			clydeSpeed = 2.5f; //se reduce su velocidad
			huntableTime -= Time.deltaTime; //se activa el tiempo en el que será cazable
			if (huntableTime <= 10f) { //en caso de que el tiempo haya empezado se cambia el color de Clyde y se activa la boca
				colorClyde.material.SetColor ("_Color", myColor);
				myMouth.SetActive (true);
			}
			if (huntableTime <= 3f) { //si el tiempo es inferior a 3 segundos, se llama al parpadeo de colores
				Blink ();
			}
			if (huntableTime <= 0) { //si el tiempo acaba
				huntableTime = 10f; //se vuelve a inicializar la variable de tiempo
				maxTime = 0.5f; //se reinicializa la variable de tiempo de parpadeo
				clydeSpeed = 4f;  //se restablece la velocidad de Blinky
				clydeIsHuntable = false; //Clyde deja de ser cazable
				this.GetComponent<GhostsController> ().isHuntable = clydeIsHuntable; //pasando el valor al script de los fantasmas
				colorClyde.material.SetColor ("_Color", Game.myOrange); //se devuelve al color normal
				myMouth.SetActive (false); //y se le desactiva la boca
			}
		} else if (clydeIsHuntable && clydeIsEated) { //en caso de que Clyde sea cazable y haya sido comido
			ResetClyde (); //se llama al método de reseteo
		}
	}
	//método que controla el parpadeo de colores en modo cazable
	void Blink () {
		timer -= Time.deltaTime; //se empieza a descontar un temporizador para intercambiar el color de Clyde
		if (timer <= 0f) { //Al llegar a 0
			if (colorClyde.material.color != Game.myOrange) { //si el color no es naranja (el color normal)
				colorClyde.material.SetColor ("_Color", Game.myOrange); // se vuelve naranja y se reinicia el temporizador
				maxTime -= 0.05f;
				timer = maxTime;
			} else { //en caso contrario, vuelve al azul y se vuelve a inicializar el temporizador
				colorClyde.material.SetColor ("_Color", myColor);
				timer = maxTime;
			}
		}
		if (maxTime <= 0f) { //cuando se haya reducido el tiempo máximo del temporizador a 0 el temporizador se para
			maxTime = timer = 0;
		}
	}
	//método de control de movimiento de Clyde dentro de la zona central
	void ClydeHome(){
		currentDirClyde = left; //al iniciar Clyde mira a la izquierda
		this.transform.localEulerAngles = currentDirClyde; //y le pasa el valor al objeto en escena
		if (!PacManController.isDeadCtrl) { //si Pacman no ha muerto
			if (this.transform.position != clydeWpPatrol [curC].transform.position) { //si Clyde no ha llegado al siguiente waypoint
				/*se crea un vector3 que acerca a Clyde al siguiente waypoint con la instrucción de mover adelante según su posición actual,
				 *la posición en la que está el siguiente waypoint y la velocidad en función del tiempo multiplicado por la variable de la velocidad*/
				Vector3 pathC = Vector3.MoveTowards (this.transform.position, clydeWpPatrol [curC].transform.position, clydeSpeed * Time.fixedDeltaTime);
				this.transform.position = pathC; //una vez calculado el nuevo vector3, Clyde se mueve a esa posición
				//estas comprobaciones hacen que Clyde mire hacia el siguiente waypoint
				if (curC == 1) {
					currentDirClyde = left;
				} else if (curC == 0) {
					currentDirClyde = right;
				} else if (curC >= 2) {
					currentDirClyde = up;
				}
				this.transform.localEulerAngles = currentDirClyde; //pasando el resultado al objeto en escena
			} else { //en caso de que Clyde esté en el waypoint que toca, se modifica la variable para que Clyde vaya al siguiente
				curC++;
				if (curC > 1 && PinkyController.curP < 5) { //si Pinky no ha llegado al 5o waypoint, clyde se mueve en la zona central
					curC = 0;
					clydeOut = false;
				} else if (curC >= 4){ //si no, sale de la zona central
					curC = 0;
					clydeOut = true;
				}
			}
		} else { //en caso de que Pacman esté muerto, se resetea a Clyde
			ResetClyde ();
		}
	}
	//método de control de movimiento normal de Clyde 
	void ClydeMove(){
		currentDirClyde = left; //al iniciar Clyde mira a la izquierda
		this.transform.localEulerAngles = currentDirClyde; //y le pasa el valor al objeto en escena
		if (!PacManController.isDeadCtrl) { //si Pacman no ha muerto
			if (this.transform.position != clydeWP [curC].transform.position) { //si Clyde no ha llegado al siguiente waypoint
				/*se crea un vector3 que acerca a Clyde al siguiente waypoint con la instrucción de mover adelante según su posición actual,
				 *la posición en la que está el siguiente waypoint y la velocidad en función del tiempo multiplicado por la variable de la velocidad*/
				Vector3 pathC = Vector3.MoveTowards (this.transform.position, clydeWP [curC].transform.position, clydeSpeed * Time.fixedDeltaTime);
				this.transform.position = pathC;//una vez calculado el nuevo vector3, Clyde se mueve a esa posición
				//estas comprobaciones hacen que Clyde mire hacia el siguiente waypoint
				if (curC == 5 || curC == 9 || curC == 13 || curC == 17) {
					currentDirClyde = left;
				} else if (curC == 0 || curC == 1 || curC == 3 || curC == 7 || curC == 11 || curC == 15 || curC == 19) {
					currentDirClyde = right;
				} else if (curC == 2 || curC == 4 || curC == 6 || curC == 14 || curC == 16 || curC == 18 || curC == 20) {
					currentDirClyde = up;
				} else if (curC == 8 || curC == 10 || curC == 12) {
					currentDirClyde = down;
				}
				this.transform.localEulerAngles = currentDirClyde; //pasando el resultado al objeto en escena
			} else { //en caso de que Clyde esté en el waypoint que toca, se modifica la variable para que Clyde vaya al siguiente
				curC = (curC + 1) % clydeWP.Length; //con un "elegante" operando de 'resto' se "evita" el uso de un if
			}
		} else { //en caso de que Pacman esté muerto, se resetea a Clyde
			ResetClyde ();
		}
	}
	//método que resetea el estado de Clyde
	void ResetClyde (){
		clydeOut = false; //se reinicia el estado de fuera de la zona central para que haga el moviento de inicio
		clydeSpeed = 4f; //se inicializa la velocidad normal
		huntableTime = 10f; //por si acaso, se reinica la variable de tiempo cazable
		maxTime = 0.5f; //por si acaso, se reinica la variable del temporizador de parpadeo
		timer = maxTime; //por si acaso, se reinicializan las variables del temporizador del parpadeo
		colorClyde.material.SetColor ("_Color", Game.myOrange); //por si acaso, se reinicializa el color de Clyde
		myMouth.SetActive (false); //al igual que se desactiva su boca
		curC = 0; //se vuelve a la posición inicial con la variable del índice del array
		this.transform.position = clydeWpPatrol [curC].transform.position; //y esta se usa para volver a Clyde a la posición inicial
		this.transform.localEulerAngles = left; //mirando a la izquierda, por si acaso
		//por si acaso se desactivan los estados de cazable y comido, tanto de este script como del de los fantasmas (pero solo del de Clyde)
		clydeIsHuntable = false;
		this.GetComponent<GhostsController> ().isHuntable = clydeIsHuntable;
		clydeIsEated = false;
		this.GetComponent<GhostsController> ().isEated = clydeIsEated;
	}
}
