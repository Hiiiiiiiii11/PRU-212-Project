using UnityEngine;

public class SetFloatBehaviour : StateMachineBehaviour
{
	public string floatName;
	public bool undateOnStateEnter, undateOnStateExit;
	public bool undateOnStateMachineEnter, undateOnStateMachineExit;
	public float valueOnEnter, valueOnExit;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (undateOnStateEnter) animator.SetFloat(floatName, valueOnEnter);
	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (undateOnStateExit) animator.SetFloat(floatName, valueOnExit);
	}

	override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
	{
		if (undateOnStateMachineEnter) animator.SetFloat(floatName, valueOnEnter);
	}

	override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
	{
		if (undateOnStateMachineExit) animator.SetFloat(floatName, valueOnExit);
	}
}
