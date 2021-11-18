using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Este Script instancia los Waypoints que usan los fantasmas para moverse
public class WaypointMovement : MonoBehaviour {

	public GameObject pfWayPoint, cWayPoints; //referencia del prefab del Waypoint y del objeto vacio en la escena donde se guardarán
	public static bool wpDone = false; //variable para comprobar que ha acabado de instanciar
	//Array que instancia los Waypoint
	public static int[,] Waypoints = new int[21,21] {
	//x: 0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0	   y
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, //0	Leyenda:
		{0,0,1,0,0,1,0,0,0,1,0,1,0,0,0,1,0,0,1,0,0}, //1		0 = vacio
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, //2		1 = Waypoint de giro	
		{0,0,1,0,0,1,0,1,0,1,0,1,0,1,0,1,0,0,1,0,0}, //3		2 = Waypoint TP Izquierdo
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, //4		3 = Waypoint TP Derecho
		{0,0,1,0,0,1,0,1,0,1,0,1,0,1,0,1,0,0,1,0,0}, //5
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, //6
		{0,0,0,0,0,0,0,1,0,1,1,1,0,1,0,0,0,0,0,0,0}, //7
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, //8
		{2,0,0,0,0,1,0,1,0,1,1,1,0,1,0,1,0,0,0,0,3}, //9
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, //10
		{0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,0,0}, //11
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, //12
		{0,0,1,0,0,1,0,1,0,1,0,1,0,1,0,1,0,0,1,0,0}, //13
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, //14
		{0,0,1,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,1,0,0}, //15
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, //16
		{0,0,1,1,0,1,0,1,0,1,0,1,0,1,0,1,0,1,1,0,0}, //17
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, //18
		{0,0,1,0,0,0,0,0,0,1,0,1,0,0,0,0,0,0,1,0,0}, //19
		{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}  //20
	};

	// Use this for initialization
	void Awake (){
		CreateWaypoints ();
	}
	//Método que instancia los Waypoints
	void CreateWaypoints(){
		int x, y, waypointsLength; //variables para el uso de los 'fors' y para el control de la posición
		waypointsLength = (int)Mathf.Sqrt (Waypoints.Length); //se recalcula el 'length' del array como si fuese unidimensional
		GameObject o = null; // variavle temporal para el instanciado de objetos
		Vector3 pos = Vector3.zero; // posición temporal donde se instanciarán los objetos
		pos.y = 1f;	//tódos los objetos se instanciarán  en la posición 1 de Y
		//los 'fors' recorren el array para instanciar los waypoints
		for (x = 0; x < waypointsLength; x++){
			for (y = 0; y < waypointsLength; y++) {
				if (Waypoints [y, x] != 0) { //en todos los casos que no sea 0, se calcula la nueva posición de X y Z de los objetos a instanciar
					pos.x = -(waypointsLength / 2) + x - 0.5f;
					pos.z = (waypointsLength / 2) - y + 0.5f;
				}
				if (Waypoints [y, x] == 1) { //normal waypoint
					o = Instantiate (pfWayPoint, pos, pfWayPoint.transform.rotation, cWayPoints.transform) as GameObject; //se instancia el waypoint en el objeto de escena indicado al final
					o.name = "WPx" + y.ToString() + "y" + x.ToString(); //y se le asigna un nuevo nombre según su posición
				}
				if (Waypoints [y, x] == 2) { //Teleport Left
					o = Instantiate (pfWayPoint, pos, pfWayPoint.transform.rotation, cWayPoints.transform) as GameObject; //se instancia el waypoint en el objeto de escena indicado al final
					o.name = "TPL"; //y se le asigna un nuevo nombre según su posición
				}
				if (Waypoints [y, x] == 3) { //Teleport Right
					o = Instantiate (pfWayPoint, pos, pfWayPoint.transform.rotation, cWayPoints.transform) as GameObject; //se instancia el waypoint en el objeto de escena indicado al final
					o.name = "TPR"; //y se le asigna un nuevo nombre según su posición
				}
				if (x == waypointsLength - 1 && y == waypointsLength - 1)
					wpDone = true; // caundo llege al final de los 'for', se activa la variable que avisa que ya están todos instanciados
			}
		}
	}
}
