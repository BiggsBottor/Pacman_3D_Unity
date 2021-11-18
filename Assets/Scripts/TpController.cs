using UnityEngine;
using System.Collections;
//Este Script permite ir de lado a lado del mapa tanto a Pacman como a los fantasmas, emulando el teletransporte entre los 2 puntos
public class TpController : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		//Teniendo en cuenta que el tamaño del mapa no va a cambiar, los valores numéricos se quedan así. sino habría que poner los números en variables controladas en el inspector
		if (this.transform.position.x < -10.5f) { //si el obejto está saliendo por la izquierda
			this.transform.position = new Vector3 (9.5f, this.transform.position.y, this.transform.position.z); //reaparece por el lado derecho
		}
		if (this.transform.position.x > 9.5f) { //si el objeto está saliendo por la derecha
			this.transform.position = new Vector3 (-10.5f, this.transform.position.y, this.transform.position.z); //reaparece por el lado izquierdo
		}
	}
}
