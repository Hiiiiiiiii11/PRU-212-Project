using UnityEngine;

public class SetBoolBehaviour : StateMachineBehaviour
{
	public string boolName;
	public bool undateOnState;
	public bool undateOnStateMachine;
	public bool valueOnEnter, valueOnExit;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (undateOnState) animator.SetBool(boolName, valueOnEnter);
	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (undateOnState) animator.SetBool(boolName, valueOnExit);
	}

	override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
	{
		if (undateOnStateMachine) animator.SetBool(boolName, valueOnEnter);
	}

	override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
	{
		if (undateOnStateMachine) animator.SetBool(boolName, valueOnExit);
	}
}
