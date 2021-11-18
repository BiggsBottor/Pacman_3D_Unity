using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//ESte script controla las acciones de Pacman
public class PacManController : MonoBehaviour {

	//variables públicas
	public float movementSpeed = 0f; //controla la velocidad de Pacman
	public GameObject ghostPoints; //referencia del canvas de puntos al matar un fantasma
	//variables públicas estáticas
	public static bool isDeadCtrl = false; //variable de control para que los demás scripts sepan si Pacman está muerto
	public static bool isGameOver = false; //variable de control para saber si ha acabado el juego

	private Animator animator = null; //variable para enviar valores a las variables de la animación de Pacman
	//Variables de control de hacia dónde mira Pacman
	private Vector3 up = Vector3.zero, 
					right = new Vector3 (0, 90, 0),
					down = new Vector3 (0, 180, 0),
					left = new Vector3 (0, 270, 0),
					currentDirection = Vector3.zero;
	private Vector3 initialPos = Vector3.zero; //Guarda la posición inicial, inicializada
	private bool isMoving, isDead, bPointsTimer; //variables de control de; (1)si se mueve, (2)si está muerto Pacman y (3)el activador del temporizador para mostrar la puntuación en el mapa
	private float timer = 1.5f; //temporizador para el tiempo en que se muestra el contador de puntos
	private Game game; //referencia del script Game

	//en pruebas
	//private Vector3 screenPos;
	//private Camera camera;
	//[SerializeField]bool valid;

	//En este método se resetea el estado de Pacman
	public void Reset () {
		if (Game.lifes > 0) { //si la cantidad de vidas es mayor que 0
			this.transform.position = initialPos; // Pacman va a la posición inicial
			animator.SetBool ("isDead", false); // se inicializa la variable del animator de control de si está muerto
			isDead = animator.GetBool ("isDead"); //se le pasa el valor a la variable
			animator.SetBool ("isMoving", false); //se inicializa la variable del aniamtor de control de si se mueve 
			currentDirection = down; //al iniciar mira hacia abajo
			Game.points = 0; // se inicializa el contador de puntos
			game.Reset (); // y se llama al método de Game para que reinicie
			if (!PrizesController.prizeEat) { //si la fruta no ha sido comida
				LevelCreator.cherry.SetActive (false); //se desactiva para poder ser comida de nuevo
				Game.prizeTime = 15f; //y se reinicia su temporizador
			} else {
				if (MenuController.newGame) {
					PrizesController.prizeEat = false;
					MenuController.newGame = false;
				}
			}
		} else { //si no le quedan vidas a Pacman
			isGameOver = true; //es fin de juego
		}
		//valid = false; //en pruebas para movimiewnto mediante raycasting
	}

	// Use this for initialization
	void Start () {
		QualitySettings.vSyncCount = 0; //se desactiva el vSync Count para evitar problemas

		//camera = Camera.main; // en pruebas
		Game.lifes = 3; //al iniciar, se reinicializan las vidas de Pacman
		initialPos = this.transform.position; //se guarda la posición de Pacman
		animator = GetComponent<Animator> (); //se coge el componente 'Animator' de Pacman
		ghostPoints = GameObject.FindWithTag ("DeathPoints"); //se busca en escena el canvas que muestra los puntos al matar un fantasma
		game = GameObject.FindWithTag ("Game").GetComponent<Game> (); //se guarda el script Game
		Reset (); //y se llama al método para resetear a Pacman
	}
	
	// Update is called once per frame
	void Update () {
		isDeadCtrl = isDead; //para que los demás scripts sepan si está muerto Pacman, se pasa el valor de la variable
		if (!Game.wining && Game.start) { //si no se ha acabado el juego y ha empezado la parida
			Movement (); //Pacman se puede mover
			if (MenuController.option == 2) MovementToCam (); //los valores de dirección se adaptarán dependiende de hacia dónde mire la cámara
		} else { //sino, no se puede mover
			isMoving = false;
			animator.SetBool ("isMoving", isMoving); //asignando el valor a la variable del Animator
		}
			PointsTimer (); //en cada frame se comprueba si algún fantasma ha muerto

	}
	void FixedUpdate(){
		//ValidDirection (); //en pruebas
	}
	//Método para cambiar los valores de las direcciónes de hacia dónde se mueve Pacman
	void MovementToCam (){
		Camera cam = Camera.main; //toma el valor de la cámara para tomar su valor de rotación Y
		//en caso de que la cámara esté mirando hacia "arriba" (hacia el eje +Z), las direcciónes tendrán el valor por defecto
		if ((cam.transform.eulerAngles.y < 45f && cam.transform.eulerAngles.y > 0f) || (cam.transform.eulerAngles.y < 360f && cam.transform.eulerAngles.y > 315f)) {
			//Debug.Log ("up = up");
			up = Vector3.zero;
			right = new Vector3 (0, 90, 0);
			down = new Vector3 (0, 180, 0);
			left = new Vector3 (0, 270, 0);
		}
		//a partir de aquí, los valores de las direcciones cambian según si...
		//...la cámara esté mirando hacia "la izquierda" (hacia el eje -X)
		if (cam.transform.eulerAngles.y < 135f && cam.transform.eulerAngles.y > 45f){ 
			//Debug.Log ("up = left");
			left = Vector3.zero;
			up = new Vector3 (0, 90, 0);
			right = new Vector3 (0, 180, 0);
			down = new Vector3 (0, 270, 0);
		}
		//...la cámara esté mirando hacia "abajo" (hacia el eje -Z)
		if (cam.transform.eulerAngles.y < 225f && cam.transform.eulerAngles.y > 135f) {
			//Debug.Log ("up = down");
			down = Vector3.zero;
			left = new Vector3 (0, 90, 0);
			up = new Vector3 (0, 180, 0);
			right = new Vector3 (0, 270, 0);
		}
		//...la cámara esté mirando hacia "la derecha" (hacia el eje +X)
		if (cam.transform.eulerAngles.y < 315f && cam.transform.eulerAngles.y > 225f) {
			//Debug.Log ("up = right");
			right = Vector3.zero;
			down = new Vector3 (0, 90, 0);
			left = new Vector3 (0, 180, 0);
			up = new Vector3 (0, 270, 0);
		}
	}
	//Método de control del movimiento de Pacman
	void Movement (){
		isMoving = true; //al moverse, se le pasa el valor a la variable
		isDead = animator.GetBool ("isDead"); //y se coge el estado actual de muerto de Pacman

		if (isDead) isMoving = false; //si está muerto, Pacman no se puede mover
		//en caso contrario se moverá según la direción de las flechas del teclado
		else if (Input.GetKey (KeyCode.UpArrow)) currentDirection = up;
		else if (Input.GetKey (KeyCode.RightArrow)) currentDirection = right;
		else if (Input.GetKey (KeyCode.DownArrow)) currentDirection = down;
		else if (Input.GetKey (KeyCode.LeftArrow)) currentDirection = left;
		else isMoving = false; //si no se pulsan teclas, Pacman no se mueve

		this.transform.localEulerAngles = currentDirection; //se le asigna la direción de movimiento adecuada al objeto en la escena
		animator.SetBool ("isMoving", isMoving); //y se le pasa el estado de movimiento a la variable del Animator

		if (isMoving) //si Pacman se puede muever...
			this.transform.Translate (Vector3.forward * movementSpeed * Time.deltaTime); //...se hace mover a Pacman
		//En el caso raro en el que pacman se sale del tablero
		if (this.transform.position.y > 1.5f){
			Debug.LogError ("Pacman fuera de mapa/Pacman out of map");
			Time.timeScale = 0;
		}
	}
	//Control de interactuación con objetos
	void OnTriggerEnter (Collider col){
		if (col.CompareTag ("Ghost")) { //si el objeto inpactado es un fantasma
			bool isHuntable = col.GetComponent<GhostsController> ().isHuntable; //se toma el estado del la variable del fantasma de si es cazable
			bool isEated = col.GetComponent<GhostsController> ().isEated; //se toma el estado de la variable del fantasma de si ha sido comido
			if (!isHuntable) { //si el fantasma no es cazable
				animator.SetBool ("isDead", true); //se activa la animación de muerte
				Game.lifes--; //se reduce la cantidad de vidas
				if (Game.lifes <= 0) Game.lifes = 0; //controlando que no sean negativas
			} else { //si el fantasma es cazable
				isEated = true; //se cambia el valor a comido al fantasma
				col.GetComponent<GhostsController> ().isEated = isEated; // actualizando el valor del script del fantasma
				Game.eatGhost = true; //activando el sonido de fantasma comido
				Game.points += 200; //sumando los puntos
				bPointsTimer = true; //activando la opción para que se muestre los puntos de fantasma muerto
				//screenPos = camera.WorldToScreenPoint(col.transform.position); // en pruebas
			}
		}
	}
	//método para mostrar en el mapa los puntos que da el fantasma al morir
	void PointsTimer (){
		ghostPoints = GameObject.FindWithTag ("DeathPoints"); //busca en pantalla el canvas a mostrar
		if (bPointsTimer) { //si se ha activado al matar un fantasma
			ghostPoints.GetComponent<Text> ().enabled = true; //activa el canvas
			timer -= Time.deltaTime; //durante un tiempo controlado en timer
		}
		else { //si no se ha activado 
			timer = 1.5f; //se inicializa el temporizador
			bPointsTimer = false; //y se inicializa el estado de la variable
		}
		if (timer <= 0) { //si el teporizador llega a 0
			timer = 0; //se queda en 0
			bPointsTimer = false; //se inicializa el estado de la variable
			ghostPoints.GetComponent<Text>().enabled = false; //y se deja de mostrar el mensaje en el mapa
		}
		
	}



	//TODO:Con este codigo intento probar movimiento mediante raycasting || está a medias
	void ValidDirection (){
		RaycastHit hit; //con un raycasthit...
		Ray ray = new Ray (this.transform.position, this.transform.forward); //se lanza un raycast para saber que tiene delante Pacaman

		Debug.DrawRay (this.transform.position, this.transform.forward, Color.red); //para que se vea ese rayo se crea uno para que se vea en el editor
		//controlando con que ha tocado el raycast se le permite avanzar a Pacman, aunque está en pruebas
		Physics.Raycast (ray, out hit, 1f);
		if (!hit.collider){ //si no choca con nada, puede avanzar
			//valid = true;
		}else { //sino
			if (hit.collider.gameObject.tag != "Map"){ //si no es una pared, se permite avanzar
				//valid = true;
			} else{ //en caso contrario, no se le permite avanzar
				//valid = false;
			}
		}
	}
}
