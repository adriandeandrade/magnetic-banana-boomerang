using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapActivator : Interactable
{
	[SerializeField] private Trap linkedTrap;

    public override void Interact()
	{
		ActivateTrap();
	}

	public void ActivateTrap()
	{
		if(linkedTrap == null)
		{
			Debug.LogError("No trap assigned to this activator. Please assign a trap in the linked trap field!");
			return;
		}

		linkedTrap.Activate();
	}
}
