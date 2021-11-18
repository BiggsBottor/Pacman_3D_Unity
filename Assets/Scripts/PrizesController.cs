using UnityEngine;
using System.Collections;
//este Script se activa cuando Pacman se come la fruta
public class PrizesController : MonoBehaviour {

	public static bool prizeEat = false; //esta variable avisa si ha sido comido

	void OnTriggerEnter (Collider col){
		if (col.CompareTag("Player")) { //Cuando Pacman contanta con la cereza
			Game.points += 200; //suma los puntos en la variable de control del script Game
			prizeEat = true; //cambiando el estado de la variable de control para confirmar que ha sido comida
			Game.fruitSound = true; //enviando la orden para que se active el sonido correspondiente
		}
	}
}
