using UnityEngine;
using System.Collections;
//Este script controla las acciones Blinky
public class BlinkyController : MonoBehaviour {
	//variables públicas
	public GameObject[] blinkyWP = new GameObject[21]; //array que guarda la ruta de Blinky
	public bool blinkyIsHuntable, blinkyIsEated; //variables de control de si es cazable o si ha sido comido
	//variables públicas estáticas
	public static int curB; //variable de control del waypoint actual
	//variables privadas
	GameObject o; //variable temporal para asignar el waypont al array
	//variables de control de la dirección a la que mira Blinky
	Vector3 up = Vector3.zero, 
			right = new Vector3 (0, 90, 0),
			down = new Vector3 (0, 180, 0),
			left = new Vector3 (0, 270, 0),
			currentDirBlinky = Vector3.zero;

	float blinkySpeed, huntableTime, timer, maxTime; //variables varias: velocidad, tiempo en el que es cazable y el tiempo en el que parpadea
	Renderer colorBlinky; //variable para pasar el color al objeto
	Color myColor; //referencia al color azul al cambiar a modo cazable
	GameObject myMouth; //referencia a la boca de asustado de Blinky

	// Use this for initialization
	void Start () {
		curB = 0; //al iniciar, se inicializa la variable a 0 para que sea el primer waypoint
		blinkySpeed = 4f; //velocidad normal de Blinky (la misma para todos)
		huntableTime = 10f; //tiempo máximo que es cazable Blinky (el mismo para todos)
		maxTime = 0.5f; //tiempo máximo inicial del parpadeo entre colores
		timer = maxTime; //se iguala timer a maxtime para el parpadeo
		blinkyIsHuntable = this.GetComponent<GhostsController>().isHuntable = false; //se guarda el estado inicial de cazable del script de los fantasmas
		blinkyIsEated = this.GetComponent<GhostsController>().isEated = false; //se guarda el estado inicial de comido del script de los fantasmas
		colorBlinky = this.GetComponentInChildren<Renderer> (); //se guarda el estado del renderizador de Blinky
		myColor = Game.myBlue; //se guarda el color azul para el modo cazable
		myMouth = this.gameObject.transform.GetChild (0).GetChild (2).gameObject; //se guarda la 'boca' de Blinky, que es hija del hijo de donde está este script
		//Se guarda la ruta de Blinky
		if (WaypointMovement.wpDone){ //comprobando primero si se han instanciado los waypoints
			//Normal Move Blinky
			o = GameObject.Find ("WPx7y10");//0
			blinkyWP [0] = o;
			o = GameObject.Find ("WPx7y7");//1
			blinkyWP [1] = o;
			o = GameObject.Find ("WPx13y7");//2
			blinkyWP [2] = o;
			o = GameObject.Find ("WPx13y5");//3
			blinkyWP [3] = o;
			o = GameObject.Find ("WPx17y5");//4
			blinkyWP [4] = o;
			o = GameObject.Find ("WPx17y2");//5
			blinkyWP [5] = o;
			o = GameObject.Find ("WPx19y2");//6
			blinkyWP [6] = o;
			o = GameObject.Find ("WPx19y9");//7
			blinkyWP [7] = o;
			o = GameObject.Find ("WPx17y9");//8
			blinkyWP [8] = o;
			o = GameObject.Find ("WPx17y7");//9
			blinkyWP [9] = o;
			o = GameObject.Find ("WPx15y7");//10
			blinkyWP [10] = o;
			o = GameObject.Find ("WPx15y5");//11
			blinkyWP [11] = o;
			o = GameObject.Find ("WPx1y5");//12
			blinkyWP [12] = o;
			o = GameObject.Find ("WPx1y2");//13
			blinkyWP [13] = o;
			o = GameObject.Find ("WPx3y2");//14
			blinkyWP [14] = o;
			o = GameObject.Find ("WPx3y18");//15
			blinkyWP [15] = o;
			o = GameObject.Find ("WPx1y18");//16
			blinkyWP [16] = o;
			o = GameObject.Find ("WPx1y15");//17
			blinkyWP [17] = o;
			o = GameObject.Find ("WPx9y15");//18
			blinkyWP [18] = o;
			o = GameObject.Find ("WPx9y13");//19
			blinkyWP [19] = o;
			o = GameObject.Find ("WPx7y13");//20
			blinkyWP [20] = o;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//si el juego no se ha ganado y ha empezado...
		if (!Game.wining && Game.start) {
			BlinkyMove (); //se llama al método que hace que se mueva Blinky
			Huntable (); //y se comprueba si es cazable
		}
	}
	//método de control para saber si Blinky es cazable
	void Huntable (){
		blinkyIsHuntable = this.GetComponent<GhostsController> ().isHuntable; //se actualiza el estado de la variable para saber si es cazable
		blinkyIsEated = this.GetComponent<GhostsController> ().isEated; //se actualiza el estado de la variable para saber si ha sido comido
		if (blinkyIsHuntable && !blinkyIsEated) { //si Blinky es cazable y no ha sido comido
			blinkySpeed = 2.5f; //se reduce su velocidad
			huntableTime -= Time.deltaTime; //se activa el tiempo en el que será cazable
			if (huntableTime <= 10f) { //en caso de que el tiempo haya empezado se cambia el color de Blinky y se activa la boca
				colorBlinky.material.SetColor ("_Color", myColor);
				myMouth.SetActive (true);
			}
			if (huntableTime <= 3f) { //si el tiempo es inferior a 3 segundos, se llama al parpadeo de colores
				Blink ();
			}
			if (huntableTime <= 0) { //si el tiempo acaba
				huntableTime = 10f; //se vuelve a inicializar la variable de tiempo
				maxTime = 0.5f; //se reinicializa la variable de tiempo de parpadeo
				blinkySpeed = 4f; //se restablece la velocidad de Blinky
				blinkyIsHuntable = false; //Blinky deja de ser cazable
				this.GetComponent<GhostsController> ().isHuntable = blinkyIsHuntable; //pasando el valor al script de los fantasmas
				colorBlinky.material.SetColor ("_Color", Color.red); //se devuelve al color normal
				myMouth.SetActive (false); //y se le desactiva la boca
			}
		} else if (blinkyIsHuntable && blinkyIsEated) { //en caso de que Blinky sea cazable y haya sido comido
			ResetBlinky (); //se llama al método de reseteo
		}
	}
	//método que controla el parpadeo de colores en modo cazable
	void Blink () {
		timer -= Time.deltaTime; //se empieza a descontar un temporizador para intercambiar el color de Blinky
		if (timer <= 0f) { //Al llegar a 0
			if (colorBlinky.material.color != Color.red) { //si el color no es rojo (el color normal)
				colorBlinky.material.SetColor ("_Color", Color.red); // se vuelve rojo y se reinicia el temporizador
				maxTime -= 0.05f;
				timer = maxTime;
			} else { //en caso contrario, vuelve al azul y se vuelve a inicializar el temporizador
				colorBlinky.material.SetColor ("_Color", myColor);
				timer = maxTime;
			}
		}
		if (maxTime <= 0f) { //cuando se haya reducido el tiempo máximo del temporizador a 0 el temporizador se para
			maxTime = timer = 0;
		}
	}
	//método de control de movimiento de Blinky
	void BlinkyMove (){
		currentDirBlinky = left; //al iniciar Blinky mira a la izquierda
		this.transform.localEulerAngles = currentDirBlinky; //y le pasa el valor al objeto en escena
		if (!PacManController.isDeadCtrl) { //si Pacman no ha muerto
			if (this.transform.position != blinkyWP [curB].transform.position) { //si Blinky no ha llegado al siguiente waypoint
				/*se crea un vector3 que acerca a Blinky al siguiente waypoint con la instrucción de mover adelante según su posición actual,
				 *la posición en la que está el siguiente waypoint y la velocidad en función del tiempo multiplicado por la variable de la velocidad*/
				Vector3 pathB = Vector3.MoveTowards (this.transform.position, blinkyWP [curB].transform.position, blinkySpeed * Time.fixedDeltaTime);
				this.transform.position = pathB; //una vez calculado el nuevo vector3, Blinky se mueve a esa posición
				//estas comprobaciones hacen que Blinky mire hacia el siguiente waypoint
				if (curB == 0 || curB == 1 || curB == 3 || curB == 5 || curB == 9 || curB == 11 || curB == 13 || curB == 17 || curB == 19) {
					currentDirBlinky = left;
				} else if (curB == 7 || curB == 15) {
					currentDirBlinky = right;
				} else if (curB == 8 || curB == 10 || curB == 12 || curB == 16 || curB == 20) {
					currentDirBlinky = up;
				} else if (curB == 2 || curB == 4 || curB == 6 || curB == 14 || curB == 18) {
					currentDirBlinky = down;
				}
				this.transform.localEulerAngles = currentDirBlinky; //pasando el resultado al objeto en escena
			} else { //en caso de que Blinky esté en el waypoint que toca, se modifica la variable para que Blinky vaya al siguiente
				curB = (curB + 1) % blinkyWP.Length; //con un "elegante" operando de 'resto' se "evita" el uso de un if
			}
		} else { //en caso de que Pacman esté muerto, se resetea a Blinky
			ResetBlinky ();
		}
	}
	//método que resetea el estado de Blinky
	void ResetBlinky (){
		huntableTime = 10f; //por si acaso, se reinica la variable de tiempo cazable
		blinkySpeed = 4f; //se inicializa la velocidad normal
		maxTime = 0.5f; //por si acaso, se reinica la variable del temporizador de parpadeo
		timer = maxTime; //por si acaso, se reinicializan las variables del temporizador del parpadeo
		colorBlinky.material.SetColor ("_Color", Color.red); //por si acaso, se reinicializa el color de Blinky
		myMouth.SetActive (false); //al igual que se desactiva su boca
		curB = 0; //se vuelve a la posición inicial con la variable del índice del array
		this.transform.position = blinkyWP [curB].transform.position; //y esta se usa para volver a Blinky a la posición inicial
		this.transform.localEulerAngles = left; //mirando a la izquierda, por si acaso
		//por si acaso se desactivan los estados de cazable y comido, tanto de este script como del de los fantasmas (pero solo del de Blinky)
		blinkyIsHuntable = false; 
		this.GetComponent<GhostsController> ().isHuntable = blinkyIsHuntable;
		blinkyIsEated = false;
		this.GetComponent<GhostsController> ().isEated = blinkyIsEated;
	}
}
