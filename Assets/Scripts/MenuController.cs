using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//Este Script se usa en la escena del menú principal
public class MenuController : MonoBehaviour {
	//variables públicas
	public GameObject optionsPanel, camControls; //referéncia del panel de opciones y a la referencia de los controles de cámara
	public Image check1, check2; //referéncia de las imágenes de check de las opciones: check1 Normal, check2 alt cam
	//variables públicas estáticas
	public static bool newGame; //variable de control de nuevo juego tras volver de una partida al menú principal
	public static int option; //variable de control de la opción de la cámara
	//este método es llamado para cargar la escena de juego
	public void PlayGame (){
		SceneManager.LoadScene ("Level 1");
		newGame = true;
	}
	//este método es llamado para salir del juego
	public void ExitGame (){
		Application.Quit ();
	}
	//éste método llama al menú de opciones
	public void OptionsPanel (){
		optionsPanel.SetActive (true);
	}
	//método del menú de opciones de cámara
	public void CameraOptions (int _option){
		//la opción 1 activará la cámara fija cenital
		if (_option == 1) { //Normal Cam
			check1.enabled = true;
			check2.enabled = false;
			camControls.SetActive (false);
		}
		//la opción 2 activará la cámara de seguimiento más el minimapa
		if (_option == 2) { //Alternative Cam + MiniMap
			check1.enabled = false;
			check2.enabled = true;
			camControls.SetActive (true);
		}
		//esto quita el menú de opcion
		if (_option == 3) { //Back
			optionsPanel.SetActive (false);
		}
		if (_option != 3) option = _option; //mientras no sea 3, se le pasa el valor a la variable estática
	}

	void Start (){
		//La opción por defecto será la 1 y se mantendrá la opción al volver de la partida
		if (option == 2) {
			check1.enabled = false;
			check2.enabled = true;
			camControls.SetActive (true);
			option = 2;
		} else {
			check1.enabled = true;
			check2.enabled = false;
			camControls.SetActive (false);
			option = 1;
		}
	}
}
