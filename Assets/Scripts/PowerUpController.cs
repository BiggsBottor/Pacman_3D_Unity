using UnityEngine;
using System.Collections;
//este Script se activa cuando Pacman se come una bolita especial
public class PowerUpController : MonoBehaviour {

	private GameObject blinky, inky, pinky, clyde; //estas variables controlan el estado de los 4 fantasmas

	void OnTriggerEnter (Collider col){
		if (col.CompareTag("Player")){ //Cuando Pacman contanta con la Bolita
			//Destroy (gameObject);//al destruir el objeto, el script deja de estar activo, por lo que deja funcionar este script
			//asi que se desactivan los componentes que hacen interactuable la bolita (emulando el destroy)
			this.GetComponent<Renderer>().enabled = false;
			this.GetComponent<SphereCollider> ().enabled = false;
			Game.points += 50; //sumando los puntos en la variable de control del script Game
			Game.foodTotal--; //y reduciendo en 1 la cantidad total de bolitas a ser comidas
			Game.foodSound = true; //enviando la orden para que se active el sonido "waka-waka"

			//Estas variables hacen "cazables" los fantasmas
			blinky.GetComponent<GhostsController> ().isHuntable = true;
			inky.GetComponent<GhostsController> ().isHuntable = true;
			pinky.GetComponent<GhostsController> ().isHuntable = true;
			clyde.GetComponent<GhostsController> ().isHuntable = true;
		}
	}

	// Use this for initialization
	void Start () {
		//Busca los Fantasmas en la escena...
		blinky = GameObject.Find ("Blinky");
		inky = GameObject.Find ("Inky");
		pinky = GameObject.Find ("Pinky");
		clyde = GameObject.Find ("Clyde");
		//en caso de que no los encuentre, dará error
		if (!blinky || !inky || !pinky || !clyde) Debug.LogError ("No encuentro uno o varios fantasmas");
		//... e inicializa su variable de cazable a 'false'
		blinky.GetComponent<GhostsController> ().isHuntable = false;
		inky.GetComponent<GhostsController> ().isHuntable = false;
		pinky.GetComponent<GhostsController> ().isHuntable = false;
		clyde.GetComponent<GhostsController> ().isHuntable = false;
	}
}
