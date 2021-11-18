using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	//variables públicas
	public float minDist, maxDist, maxHeight, angle, speed;//, angleMulti; //variables para el movimiento de la cámara
	//variables privadas
	private GameObject player; //usada para mirar al jugador
	private float dist; // variable para el cálculo de la distancia respecto al jugador
	private Vector3 camPos, lookAt; //la 1a variable es la posición de la cámara y la segunda se usa para mirar al jugador

	// Use this for initialization
	void Start () {
	//cogemos el player
		player = GameObject.FindWithTag("Player");
		//si no está, dará un error por consola
		if (!player) Debug.LogError ("No encuentro al Player");
		dist = 4f; // asignamos un valor medio a la distancia de la cámara
		speed = 20f;
	}

	// Update is called once per frame
	void LateUpdate () { //se usa Lateupdate dado que la cámara se mueve despues que se mueva todo en cada frame
		MoveCamera (); //lo único que hace el Update es llamar a la funcion para mover la cámara
	}
	//esta función se encarga del movimiento de la cámara y de seguir al jugador
	void MoveCamera(){
		//coge la posición del jugador
		lookAt = player.transform.position;

		angle -= Input.GetAxis ("Mouse X") * 180f * Time.deltaTime; //angulo igual al mouse

		//Calcula lo necesario para mantener la posición y la rotación
		camPos.x = (Mathf.Cos (angle * Mathf.Deg2Rad) * dist) + player.transform.position.x;
		camPos.z = (Mathf.Sin (angle * Mathf.Deg2Rad) * dist) + player.transform.position.z;
		camPos.y = player.transform.position.y + maxHeight;

		this.transform.position = camPos; //tras el cálculo, le pasa el valor a la posición de la cámara
	
		//mira hacia la posicion
		this.transform.LookAt (lookAt);

		//Reduce la distancia con la camara
		if (Input.GetAxis ("Mouse ScrollWheel") > 0){
			dist -= speed * Time.deltaTime;
			if (dist < minDist)
				dist = minDist;
		}
		//Aumenta la distancia con la camara
		if (Input.GetAxis ("Mouse ScrollWheel") < 0){	
			dist += speed * Time.deltaTime;
			if (dist > maxDist)
				dist = maxDist;
		}
	}
}
