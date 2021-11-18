using UnityEngine;
using System.Collections;
//este Script se activa cuando Pacman se come una bolita normal
public class FoodController : MonoBehaviour {
	
	void OnTriggerEnter (Collider col){ 
		if (col.CompareTag("Player")){ //Cuando Pacman contanta con la Bolita
			//Destroy (gameObject); //al destruir el objeto, el script deja de estar activo, por lo que deja funcionar este script
			//asi que se desactivan los componentes que hacen interactuable la bolita (emulando el destroy)
			this.GetComponent<Renderer>().enabled = false;
			this.GetComponent<SphereCollider> ().enabled = false;
			Game.points += 10; //sumando los puntos en la variable de control del script Game
			Game.foodTotal--; //y reduciendo en 1 la cantidad total de bolitas a ser comidas
			Game.foodSound = true; //enviando la orden para que se active el sonido "waka-waka"
		}
	}
}
