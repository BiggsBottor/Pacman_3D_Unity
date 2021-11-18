using UnityEngine;
using System.Collections;
//Este Script sólo controla el estado de las variables de cada fantasma
public class GhostsController : MonoBehaviour {

	private bool _isHuntable, _isEated; //estas variables controla si el fantasma es cazable o si es comido, respectivamente

	//Getters/Setters
	public bool isHuntable { //esta variable pública controla si el fantasma es cazable
		get { return _isHuntable; }
		set { _isHuntable = value; }
	}

	public bool isEated { //esta variable pública controla si el fantasma ha sido comido
		get { return _isEated; }
		set { _isEated = value; }
	}
}
