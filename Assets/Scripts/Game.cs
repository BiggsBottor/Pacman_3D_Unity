using UnityEngine;
using System.Collections;
using UnityEngine.UI; //esta librería es necesaria para el uso de las variables del canvas
using UnityEngine.SceneManagement; //esta librería es necesaria para la carga de escena
//Este script está activo y controla diversas cosas del juego
public class Game : MonoBehaviour {
	//Variables públicas
	public Text txtPoints, txtHighPoints, txtLifes, txtCentral; //textos del canvas
	public Image imageCherry, imgPacman1, imgPacman2, imgPacman3; //imágenes del canvas: la cereza y el indicador de vidas
	public GameObject pauseMenu; //Panel de pausa, para poder activarlo/desactivarlo
	public AudioSource asMusic, asSounds; //Las fuentes de audio para la reproducción del audio de fondo y los diversos sonidos
	public AudioClip acReady, acSiren, acWaza, acDie, acEatFruit, acEatPill, acEatGhost, acExtraLife; //los archivos de audio
	public CameraController camCtrl;
	public Camera mapCam;
	//variables públicas estáticas 
	//dado que estos colores no están por código (p.e. Color.red, Color.cyan, etc), hay que prepararlos por código 
	//Los colores en formato RGB deben tener valores flotantes entre 0 y 1
	public static Color myBlue = new Color (0f, 0.2f, 1f, 1f); //el azul para cuando están en modo 'cazables'
	public static Color myPink = new Color (0.9f, 0.6f, 0.8f); //el rosa de Pinky 
	public static Color myOrange = new Color(0.875f, 0.525f, 0.154f);// el naranja de Clyde
	//variables privadas estáticas //estas variables son para los setter/getters
	private static int _points, _lifes, _highPoints, _foodTotal; 
	private static float _prizeTime;
	private static bool _wining, _start, _foodSound, _fruitSound, _eatGhost;
	//variables privadas 
	private GameObject cherry, blinky, inky, pinky, clyde; //referencia de la cereza para activarla en el canvas y los 4 fantasmas para coger su script
	private float timer = 2.5f; //controla el tiempo en el que estará activo el mensaje 'You Win'
	private bool extraLife, extraLifeSound, isReadyEnd; //variables de control para (1a)saber si ya ha habido vida extra, (2a)para que suene y (3a) para saber si ha acabado la música de 'Ready'
	//Variables de control inspector
	//[SerializeField]private bool startCtrl, winingCtrl, foodSoundCtrl; //no usadas

	//variables Getter/Setters estáticas
	//controla si se ha comido un fantasma para activar el sonido correspondiente
	public static bool eatGhost{
		get { return _eatGhost; }
		set { _eatGhost = value; }
	}
	//controla si se ha comido la fruta para activar el sonido correspondiente
	public static bool fruitSound{
		get { return _fruitSound; }
		set { _fruitSound = value; }
	}
	//controla se se ha comido una bolita para activar el sonido correspondiente
	public static bool foodSound {
		get { return _foodSound; }
		set { _foodSound = value; }
	}
	//controla si se ha acabado la música de ready para que se puedan mover los fantasmas y Pacman
	public static bool start {
		get { return _start; }
		set { _start = value; }
	}
	//controla si se ha ganado la partida para que dejen de moverse los fantasmas y Pacman
	public static bool wining {
		get { return _wining; }
		set { _wining = value; }
	}
	//controla la cantidad de bolitas para activar el final de partida
	public static int foodTotal {
		get { return _foodTotal; }
		set { _foodTotal = value; }
	}
	//controla el tiempo máximo en el que estará visible la cereza para ser comida
	public static float prizeTime {
		get { return _prizeTime; }
		set { _prizeTime = value; }
	}
	//controla los puntos que consigue el jugador
	public static int points {
		get { return _points; }
		set {
			if (_points < 0) //esto no haría falta ya que no se descuentan puntos, pero por si acaso :P
				_points = 0;
			else
			_points = value; 
		}
	}
	//controla la cantidad máxima de puntos conseguidos
	public static int highPoints{
		get { return _highPoints; }
		set {
			if (_highPoints < 0) // igual que en points, por si acaso
				_highPoints = 0;
			else
				_highPoints = value;
		}
	}
	//controla la cantidad de vidas del jugador
	public static int lifes {
		get { return _lifes; }
		set {
			if (_lifes > 3) //no puede haber más de 3 vidas
				_lifes = 3;
			else
			_lifes = value; }
	}
	//método que controla el reseteo para reinicializar variables
	public void Reset (){
		//Reseteo de la música para que suene la de 'Ready'
		asMusic.Stop();
		if (asMusic.loop) asMusic.loop = false;
		if (asMusic.pitch != 1f) asMusic.pitch = 1f;
		if (asMusic.clip != acReady) asMusic.clip = acReady;
		asMusic.volume = .4f;
		asMusic.Play ();
		//antes de que acabe el 'Ready' se reinicia las variables de inicio activando el mensaje central
		isReadyEnd = false;
		start = false;
		txtCentral.enabled = true;
		txtCentral.text = "Ready";
		//comprobar el estado de la cereza || al usar el forzado de reseto mediante la tecla 'R' 
		//no se aplica el reset de Pacman correctamente y no muestra correctamente la cereza. De manera normal si va bien
		//Debug.Log ("cereza comida " + PrizesController.prizeEat + " cereza mostrada " + imageCherry.enabled);
	}
	//Método que controla la reproducción de la música normal de fondo
	void PlayMusic (){
		if (!asMusic.loop) asMusic.loop = true;
		if (asMusic.pitch != .75f) asMusic.pitch = .75f;
		if (asMusic.clip != acSiren) asMusic.clip = acSiren;
		if (!asMusic.isPlaying) asMusic.Play ();
	}
	//Método que controla la reproducción de la música en modo caza de fondo
	void PlayHuntMusic (){
		if (!asMusic.loop) asMusic.loop = true;
		if (asMusic.pitch != 1f) asMusic.pitch = 1f;
		if (asMusic.clip != acWaza) asMusic.clip = acWaza;
		if (!asMusic.isPlaying) asMusic.Play ();
	}
	//Método que controla la reproducción de la música de muerte de Pacman
	void PlayDeadMusic (){
		if (asMusic.loop) asMusic.loop = false;
		if (asMusic.pitch != 1f) asMusic.pitch = 1f;
		if (asMusic.clip != acDie) asMusic.clip = acDie;
		if (!asMusic.isPlaying) asMusic.Play ();
	}
	//Método que controla la reproducción de diversos sonidos
	void PlaySounds (){
		if (foodSound || eatGhost) {
			if (foodSound && !eatGhost) { //si come bolitas suena el "waka-waka"	
				asSounds.PlayOneShot (acEatPill);
				foodSound = false;
			}

			if (eatGhost && !foodSound) { //si come un fantasma, suena el sonido correspondiente
				asSounds.PlayOneShot (acEatGhost);
				eatGhost = false;
			}
			if (foodSound && eatGhost) { //en caso de que deban reproducirse ambos sonidos, el del fantasma tiene prioridad (aunque pasa muy poco)
				asSounds.PlayOneShot (acEatGhost);
				eatGhost = false;
				foodSound = false;
			}
		}  
		//si se come la fruta suena el sonido correspondiente
		if (fruitSound) {
			asSounds.PlayOneShot (acEatFruit);
			fruitSound = false;
		}
		//si se llegan a cierta cantidad de puntos, suena el aviso de vida extra (aunque el máximo de vidas son 3)
		if (extraLifeSound) {
			asSounds.PlayOneShot (acExtraLife);
			extraLifeSound = false;
		}
	}

	void Start (){
		//controla si se ha activado el menú de pausa durante la partida, se ha ido al menú principal y se ha vuelto a la partida
		if (Time.timeScale == 0) Time.timeScale = 1; //esto controla que se reinicie el 'timeScale'
		if (PacManController.isGameOver) { //si se acabó la partida anterior, se pueda iniciar otra
			PacManController.isGameOver = false;
			points = 0;
			highPoints = 0;
			lifes = 3;
		}
		cherry = LevelCreator.cherry; //controla que se visualize en el canvas la cereza al ser comida
		prizeTime = 15f; //se inizializa el tiempo máximo que estará activa la cereza
		//inicialización de variables
		wining = false;
		extraLife = false;
		Reset(); //se llama al método de reseteo
		GhostsCtrl (); //se llama al método de búsqueda de los fantasmas
		CamOption ();
	}
	//se buscan los fantasmas para poder llamar a sus scripts de control
	void GhostsCtrl (){
		blinky = GameObject.Find ("Blinky");
		inky = GameObject.Find ("Inky");
		pinky = GameObject.Find ("Pinky");
		clyde = GameObject.Find ("Clyde");
	}

	// Update is called once per frame
	void Update () {
		//control de variables en el inspector //ya no usado
			//lifesCtrl = lifes;
			//startCtrl = start;
			//winingCtrl = wining;
			//foodSoundCtrl = foodSound;
			//RestartDebug (); //vuelve a cargar la escena. sólo para revisar


		GhostsCtrl (); //se llama de nuevo al método para actualizar el estado de las variables del script de los fantasmas...
		//...y asigmnar a estas variables temporales la variable de cada fantasma 'isHuntable' para saber si son cazables
		bool blinkyHuntable = blinky.GetComponent<GhostsController> ().isHuntable;
		bool inkyHuntable = inky.GetComponent<GhostsController> ().isHuntable;
		bool pinkyHuntable = pinky.GetComponent<GhostsController> ().isHuntable;
		bool clydeHuntable = clyde.GetComponent<GhostsController> ().isHuntable;

		//activa la sirena como sonido ambiente o el "waza" durante la caza de pacman...
		if (!PacManController.isGameOver) {// si no se a acabado la partida...
			if (!PacManController.isDeadCtrl) { //...y Pacman no está muerto...
				if (blinkyHuntable || inkyHuntable || pinkyHuntable || clydeHuntable) {//...si alguno de los fantasmas es cazable...
					PlayHuntMusic ();//...sonará la canción de caza (el 'waza')
				} 
				if (!blinkyHuntable && !inkyHuntable && !pinkyHuntable && !clydeHuntable) {//si todos los fantasmas no son cazables... ->
					if (asMusic.clip == acReady && !asMusic.isPlaying) {//si la música es la de 'Ready' y ya no se reproduce...
						//se cambian las variables de inicio para que no se muestre el mensaje de Ready y se pueda reproducir la musica de fondo
						start = true;
						txtCentral.enabled = false;
						isReadyEnd = true;
					}
					if (isReadyEnd)//-> ...y no suena la música de ready...
						PlayMusic ();//sonará la música de fondo (la 'sirena')
				}
			} else {//en caso contrario sonará la de muerte de Pacman
				PlayDeadMusic ();
			}
		}
		//en el update se llama al método de reproducción de sonidos
		PlaySounds ();
		//en caso de que puntos sea mayor de 0 y mayor que la puntuación máxima, será igual a los puntos actuales
		if (points > 0 && highPoints < points) highPoints = points;
		//si la puntuación máxima es mayor de 0, se mostrarán en el canvas, sino no.
		if (highPoints > 0) txtHighPoints.text = highPoints.ToString ("0000");
		else txtHighPoints.text = "";
		//si la puntuación actual es mayor de 0, se mostrarán en el canvas, sino no.
		if (points > 0) txtPoints.text = points.ToString ("0000");
		else txtPoints.text = "";
		//muestra la cantidad de vidas en el canvas
		txtLifes.text = "Lifes : " + lifes.ToString ("0");
		//en caso que la puntuación llege a 70, activará la cereza...
		if (points >= 70) {
			cherry.SetActive (true);
			if (!PrizesController.prizeEat) { //... y si no se ha comido, se activa un contador para poder comerla
				prizeTime -= Time.deltaTime;
				if (prizeTime <= 0) { //si se acaba el tiempo se pierde la oportunidad de comerla
					prizeTime = 0;
					cherry.SetActive (false);
				}
			} else { //si se come se muestra en el canvas que se ha comido, desactivando el objeto del mapa
				cherry.SetActive (false);
				imageCherry.enabled = true;
			}
		}
		//si Pacman se queda sin vidas se mostrará el mensaje de 'Game Over'
		if (PacManController.isGameOver) {
			txtCentral.enabled = true;
			txtCentral.text = "Game Over";
			StopSounds ();//parando todos los sonidos
		}

		if (foodTotal <= 0) { //si Pacman acaba con todas las bolitas...
			foodTotal = 0; //por si acaso, se impide que sea menor de 0
			wining = true; // se activa la variable de control de fin de partida
			start = false; // por si acaso se inicializa la variable de inicio
			StopSounds (); //se para el sonido
			txtCentral.enabled = true; // se activa el mensaje central temporizado 
			txtCentral.text = "You Win";
			timer -= Time.deltaTime;
			if (timer <= 0) { //y cuando sea 0 cambia el mensaje a 'Game Over'
				timer = 0;
				txtCentral.text = "Game Over";
			}
		}
		//lleva la cuenta de vidas para mostrar la cantidad visual de vidas en el canvas
		if (lifes >= 3) { //si son 3, se muestran todas
			imgPacman1.GetComponent<Image> ().enabled = true;
			imgPacman2.GetComponent<Image> ().enabled = true;
			imgPacman3.GetComponent<Image> ().enabled = true;
		}
		if (lifes == 2) { //si son 2, la de más a la derecha ya no se muestra
			imgPacman1.GetComponent<Image> ().enabled = true;
			imgPacman2.GetComponent<Image> ().enabled = true;
			imgPacman3.GetComponent<Image> ().enabled = false;
		}
		if (lifes == 1) { //si solo es 1, sólo se muestra la de más a la izquierda
			imgPacman1.GetComponent<Image> ().enabled = true;
			imgPacman2.GetComponent<Image> ().enabled = false;
			imgPacman3.GetComponent<Image> ().enabled = false;
		}
		if (lifes == 0) { //si no quedan, no se muestra ninguna
			imgPacman1.GetComponent<Image> ().enabled = false;
			imgPacman2.GetComponent<Image> ().enabled = false;
			imgPacman3.GetComponent<Image> ().enabled = false;
		}
		//si se consiguen estos puntos, se consigue vida extra
		if (points >= 1825 && !extraLife) {
			if (lifes < 3) lifes += 1; //pero la cantidad máxima son 3 vidas
			extraLife = true; //si se a conseguido la vida extra, no se consguen más
			extraLifeSound = true; // aún así se avisa de que ha llegado a la cantidad de puntos para la vida extra
		}
		//si la cantidad de puntos es 0(sin importar la puntuación máxima), se puede conseguir la vida extra
		if (points == 0)
			extraLife = false;

		PauseMenu (); //activa el menú de pausa
	}
	//método que se usa para parar todo el sonido
	void StopSounds (){
		asMusic.loop = false;
		asSounds.loop = false;
		asSounds.Stop();
		asMusic.Stop ();
	}
	//activa el menú de pausa con la tecla 'Esc'
	void PauseMenu (){
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Time.timeScale = 0;
			pauseMenu.SetActive (true);
			asMusic.mute = true;
		}
	}
	//según la opción del menú principal
	void CamOption (){
		if (MenuController.option == 1) { //desactiva el minimapa y el script de la cámara
			camCtrl.enabled = false;
			mapCam.enabled = false;
		}
		if (MenuController.option == 2) { //deja el script y el minimapa activo
			camCtrl.enabled = true;
			mapCam.enabled = true;
		}
	}
	//método que controla qué botón se ha pulsado
	public void ExitPauseMenu (int option){
		if (option == 1) { //en caso de 1 se vuelve al juego
			pauseMenu.SetActive (false);
			Time.timeScale = 1;
			asMusic.mute = false;
		}
		if (option == 2) { //en caso de 2 se vuelve al menú principal
			PacManController.isGameOver = true;
			SceneManager.LoadScene ("Menu");
		}
	}
	//método no usado para probar el reinicio del nivel
	void RestartDebug (){
		if (Input.GetKeyDown (KeyCode.R)){
			SceneManager.LoadScene ("Level 1");
			Reset ();
			//falta el reset de script PacmanController, que no voy a poner
		}
	}
}
