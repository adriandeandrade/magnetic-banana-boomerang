using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapActivator : Interactable
{
	[SerializeField] private GameObject linkedTrap;

    public override void Interact()
	{
		ActivateTrap();
	}

	public void ActivateTrap()
	{
		Trap trap = linkedTrap.GetComponent<Trap>();
		trap.Activate();
	}
}
