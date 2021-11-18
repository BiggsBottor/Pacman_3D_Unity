using UnityEngine;
using System.Collections;
//Este Script controla el estado de la animación de muerte de Pacman
public class PacmaDeathStateBehavior : StateMachineBehaviour {
	PacManController pacManController = null; //variable de referencia del script de Pacman
	static int death = Animator.StringToHash ("Death"); //variable de control para activar la animación de muerte de Pacman

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (!pacManController) //si no encuentra el script de Pacman
			pacManController = GameObject.Find ("Pacman").GetComponent<PacManController> (); // se le pasa del objeto en escena
		if (stateInfo.shortNameHash == death) //al entrar en la animación de muerte
			animator.applyRootMotion = false; //para la animación de movimiento de abrir y cerrar la boca para poder activar la de muerte
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (stateInfo.shortNameHash == death && stateInfo.normalizedTime > stateInfo.length) { //si está en la animación de muerte y ha llegado al final de la animación
			pacManController.Reset (); //llama al script de Pacman para reiniciarlo a su posición inicial
		}	
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (stateInfo.shortNameHash == death) //al salir de la animación de muerte
			animator.applyRootMotion = true; //reactiva la animación de movimiento
	}
}
