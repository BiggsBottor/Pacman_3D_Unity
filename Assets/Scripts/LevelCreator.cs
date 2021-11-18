using UnityEngine;
using System.Collections;
//Este script se encarga de instanciar el mapa
public class LevelCreator : MonoBehaviour {

	//Variables de Inspector
	//estas variables guardan los prefabs y los objetos vacios en los que instanciarán en la escena
	public GameObject pfFloor, mapContainer, walls, pfFood, pfPowerUp, foods, pfPacman, pfGhost, ghosts, pfCherry, cherries;

	//variables NO inspector
	//Variables públicas estáticas
	public static GameObject cherry; //esta variable sirve para indicar que ha sido instanciada
	//variables privadas
	int iFood, iPowerUp; //cuentan la cantidad de bolitas del mapa
	float mapLength; //variable que guarda la cantidad total del array despues de un cálculo matemático
	GameObject o; //variable temporal para la creación del mapa
	Renderer pieceColor; //variable para cambiar el color al objeto instanciado
	Color mycolor; //variable de color temporal
	//array para la creación del mapa
	int[,] map = new int[21,21] { 
	//x: 0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0	   y
		{0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0}, //0		Leyenda:
		{0,2,6,6,6,6,6,6,6,6,2,6,6,6,6,6,6,6,6,2,0}, //1		0 = Vacio
		{0,2,7,2,2,6,2,2,2,1,2,6,2,2,2,6,2,2,7,2,0}, //2		1 = Terreno vacio Transitable (t.v.)
		{0,2,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,2,0}, //3		2 = Muro 
		{0,2,6,2,2,6,2,6,2,2,2,2,2,6,2,6,2,2,6,2,0}, //4		3 = Premios
		{0,2,6,6,6,6,2,6,6,6,2,6,6,6,2,6,6,6,6,2,0}, //5		
		{0,2,2,2,2,6,2,2,2,1,2,1,2,2,2,6,2,2,2,2,0}, //6		5 = Inicio PacMan
		{0,0,0,0,2,6,2,1,1,1,8,1,1,1,2,6,2,0,0,0,0}, //7		6 = Comida
		{2,2,2,2,2,6,2,1,2,2,2,2,2,1,2,6,2,2,2,2,2}, //8		7 = Power Up
		{1,1,1,1,1,6,1,1,2,9,10,11,2,1,1,6,1,1,1,1,1}, //9		8 = Fantasma Rojo aka Blinky
		{2,2,2,2,2,6,2,1,2,2,2,2,2,1,2,6,2,2,2,2,2}, //10		9 =     "    Cyan aka Inky
		{0,0,0,0,2,6,2,1,1,1,3,1,1,1,2,6,2,0,0,0,0}, //11	   10 =     "    Rosa aka Pinky
		{0,2,2,2,2,6,2,1,2,2,2,2,2,1,2,6,2,2,2,2,0}, //12	   11 =     "    Naranja aka Clyde
		{0,2,6,6,6,6,6,6,6,6,2,6,6,6,6,6,6,6,6,2,0}, //13	   
		{0,2,6,2,2,6,2,2,2,6,2,6,2,2,2,6,2,2,6,2,0}, //14
		{0,2,7,6,2,6,6,6,6,6,5,6,6,6,6,6,2,6,7,2,0}, //15
		{0,2,2,6,2,6,2,6,2,2,2,2,2,6,2,6,2,6,2,2,0}, //16
		{0,2,6,6,6,6,2,6,6,6,2,6,6,6,2,6,6,6,6,2,0}, //17
		{0,2,6,2,2,2,2,2,2,6,2,6,2,2,2,2,2,2,6,2,0}, //18
		{0,2,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,2,0}, //19
		{0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,0} //20
	};
		
	// Use this for initialization
	void Start () {
		mapLength = Mathf.Sqrt (map.Length); //al ser un array cuadrado, se guarda el valor como si fuese unidimensional
		iFood = iPowerUp = 0; //al iniciar, se inicializan las variables que cuentan la cantidad de bolitas del mapa
		CreateLevel (); //llama al método que crea el mapa
	}
	//Este método crea el mapa
	void CreateLevel (){
		int x, y; //variables temporales para el uso de los 2 'for'
		for (y = 0; y < mapLength; y++) {
			for (x = 0; x < mapLength; x++) {
				if (map [y, x] == 0) {//0 terreno "vacio"
					InstantiateObjects (x, y, 0f, pfFloor, mapContainer); //se llama al constructor que instancia el suelo según la posición de x e y, en el objeto de escena
					pieceColor = o.GetComponent<Renderer> (); // luego coge el renderizador del objeto recien instanciado
					pieceColor.enabled = false; // y lo deshabilita, ya que es zona vacia fuera de paredes
				}
				if (map [y, x] == 1) {//terreno transitable
					InstantiateObjects (x, y, 0f, pfFloor, mapContainer); //se llama al constructor para que instancie el suelo
					pieceColor = o.GetComponent<Renderer> (); // luego coge el renderizador del objeto recien instanciado
					pieceColor.material.SetColor ("_Color", Color.black); // y se le cambia el color a negro
				}
				if (map [y, x] == 2) {//muro
					//primero se instancia el suelo
					InstantiateObjects (x, y, 0f, pfFloor, mapContainer);
					pieceColor = o.GetComponent<Renderer> ();
					pieceColor.material.SetColor ("_Color", Color.black);
					//y luego se instancia la pared aprovechando el prefab del suelo y cambiandole el color a azul
					InstantiateObjects (x, y, 1f, pfFloor, walls);
					pieceColor = o.GetComponent<Renderer> ();
					pieceColor.material.SetColor ("_Color", Color.blue);
					if (x == 10 && y == 8) { //para hacer la "puerta" de la zona central, se le cambia el color y el tamaño
						o.transform.localScale = new Vector3 (0.8f, 0.8f, 0.8f);
						pieceColor = o.GetComponent<Renderer> ();
						pieceColor.material.SetColor ("_Color", Color.magenta);
					}
				}
				if (map [y,x] == 3){ //Premio
					//primero se instancia el suelo
					InstantiateObjects (x, y, 0f, pfFloor, mapContainer);
					pieceColor = o.GetComponent<Renderer> ();
					pieceColor.material.SetColor ("_Color", Color.black);
					//y luego se instancia la cereza
					InstantiateObjects (x, y, 1f, pfCherry, cherries);
					cherry = o; //se le pasa el valor de la variable temporal 'o' a la variable de paso 'cherry'
					o.SetActive (false); //y se desactiva para que no sea interactuable
				}
				if (map [y, x] == 5) { //Pacman
					//primero se instancia el suelo
					InstantiateObjects (x, y, 0f, pfFloor, mapContainer);
					pieceColor = o.GetComponent<Renderer> ();
					pieceColor.material.SetColor ("_Color", Color.black);
					//pacman no se instancia con el constructor porque no va a un objeto vacio de la escena, así que hay que instanciarlo de manera normal
					Vector3 pos = new Vector3 (-(mapLength / 2) + x, 1f,(mapLength / 2) - y); //para ello se le asigna la posición para pasarlo a la instrucción de instanciar
					o = Instantiate (pfPacman, pos, pfPacman.transform.rotation) as GameObject;
					o.gameObject.name = "Pacman"; //y luego se le cambia el nombre
				}
				if (map [y, x] == 6) { //Comida (bolitas amarillas)
					//primero se instancia el suelo
					InstantiateObjects (x, y, 0f, pfFloor, mapContainer);
					pieceColor = o.GetComponent<Renderer> ();
					pieceColor.material.SetColor ("_Color", Color.black);
					//y luego se instancian las bolitas amarillas
					InstantiateObjects (x, y, 1f, pfFood, foods);
					pieceColor = o.GetComponent<Renderer> ();
					pieceColor.material.SetColor ("_Color", Color.yellow); // se le cambia el color
					iFood++; //y se suma a la cantida de bolitas amarillas
				}
				if (map [y, x] == 7) { //PowerUp (bolitas blancas)
					//primero se instancia el suelo
					InstantiateObjects (x, y, 0f, pfFloor, mapContainer);
					pieceColor = o.GetComponent<Renderer> ();
					pieceColor.material.SetColor ("_Color", Color.black);
					//y luego se instancian las bolitas blancas
					InstantiateObjects (x, y, 1f, pfPowerUp, foods);
					pieceColor = o.GetComponent<Renderer> ();
					pieceColor.material.SetColor ("_Color", Color.white); //se le cambia el color (por si acaso)
					iPowerUp++; //y se suma a la cantidad de bolitas blancas
				}
				if (map [y, x] == 8) { //Blinky (rojo)
					//primero se instancia el suelo
					InstantiateObjects (x, y, 0f, pfFloor, mapContainer);
					pieceColor = o.GetComponent<Renderer> ();
					pieceColor.material.SetColor ("_Color", Color.black);
					//luego se instanca a Blinky
					InstantiateObjects (x, y, 1f, pfGhost, ghosts);
					pieceColor = o.GetComponentInChildren<Renderer> (); //se coge el renderizador
					pieceColor.material.SetColor ("_Color", Color.red); //para cambiarle el color a rojo
					o.gameObject.name = "Blinky"; //se le cambia el nombre para buscarlo mejor en la escena
					o.AddComponent<BlinkyController> (); //y con esto se le asigna el script correspondiente que le falta
				}
				if (map [y, x] == 9) { //Inky (cyan)
					//primero se instancia el suelo
					InstantiateObjects (x, y, 0f, pfFloor, mapContainer);
					pieceColor = o.GetComponent<Renderer> ();
					pieceColor.material.SetColor ("_Color", Color.black);
					//luego se instancia a Inky
					InstantiateObjects (x, y, 1f, pfGhost, ghosts);
					pieceColor = o.GetComponentInChildren<Renderer> (); //se coge el renderizador
					pieceColor.material.SetColor ("_Color", Color.cyan); //para cambiarle el color a cian
					o.gameObject.name = "Inky"; //se le cambia el nombre para buscarlo mejor en la escena
					o.AddComponent<InkyController> (); //y con esto se le asigna el script correspondiente que le falta
				}
				if (map [y, x] == 10) { //Pinky (rosa)
					//primero se instancia el suelo
					InstantiateObjects (x, y, 0f, pfFloor, mapContainer);
					pieceColor = o.GetComponent<Renderer> ();
					pieceColor.material.SetColor ("_Color", Color.black);
					//luego se instancia a Pinky
					InstantiateObjects (x, y, 1f, pfGhost, ghosts); 
					mycolor = Game.myPink; //dado que el rosa no está por código, se coge del script Game, donde ya está calculado
					pieceColor = o.GetComponentInChildren<Renderer> (); //se coge el renderizador
					pieceColor.material.SetColor ("_Color", mycolor); //para cambiarle al color rosa, que se coge de la variable de paso
					o.gameObject.name = "Pinky"; //se le cambia el nombre para buscarlo mejor en la escena
					o.AddComponent<PinkyController> (); //y se le asigna el script correspondiente que le falta
				}
				if (map [y, x] == 11) { //Clyde (naranja)
					//primero se instancia el suelo
					InstantiateObjects (x, y, 0f, pfFloor, mapContainer);
					pieceColor = o.GetComponent<Renderer> ();
					pieceColor.material.SetColor ("_Color", Color.black);
					//luego se instancia a Clyde
					InstantiateObjects (x, y, 1f, pfGhost, ghosts);
					mycolor = Game.myOrange; //dado que el naranja no está por código, se coge del script Game, donde ya está calculado
					pieceColor = o.GetComponentInChildren<Renderer> (); //se coge el renderizador
					pieceColor.material.SetColor ("_Color", mycolor); //para cambiarle al color naranja, que se coge de la variable de paso
					o.gameObject.name = "Clyde"; //se le cambia el nombre para buscarlo mejor en la escena
					o.AddComponent<ClydeController> (); //y se le asigna el script correspondiente que le falta
				}
			}
		}
		Game.foodTotal = iFood + iPowerUp; //despues de los 'fors', se le suma la cantidad de bolitas para saber cuando gana el jugador
	}
	/*Este constructor instancia los objetos en escena según el valor de x e y de los 'fors', la posición de Y (ya que instancia tanto suelo como lo que hay encima)
	 *la referencia del Prefab y la referencia del objeto vacio que contendrá el objeto recien instanciado*/
	void InstantiateObjects (int _x, int _y, float posY, GameObject pfGo, GameObject goContainer){
		Vector3 pos = new Vector3 (-(mapLength / 2) + _x, posY, (mapLength / 2) - _y); //primero, se prepara un vector3 y se instancia en una posición desde la mitad negativa de x y z hasta la otra mitad
		o = Instantiate (pfGo, pos, pfGo.transform.rotation, goContainer.transform) as GameObject; //se instancia el objeto
		NoWarning (o); //y se llama a esta función cuyo único propósito es evitar que salga un aviso de "que ha asignado un valor a la variable, pero nunca se usa"
	}
	//esta función sólo evita el aviso en la consola
	public void NoWarning (GameObject _noWarning){
		GameObject noWarning = _noWarning;
		_noWarning = noWarning;
	}
}
