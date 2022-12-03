using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    Controller controller;
	Animator animator;
	Looker looker;
	Runner runner;
	PaintGun gun;

	void Awake()
    {
        controller = GetComponent<Controller>();
		animator = GetComponentInChildren<Animator>();
		looker = GetComponent<Looker>();
		runner = GetComponent<Runner>();
		gun = GetComponentInChildren<PaintGun>();
	}

    // Update is called once per frame
    void Update()
    {
		if(controller.Pressed(InputCode.SWITCH_LEFT))
		{
			print("Switching Colours");
			// animator.SetTrigger("Switched");
		}
		if (controller.Pressed(InputCode.SWITCH_RIGHT))
		{
			print("Switching Colours");
			// animator.SetTrigger("Switched");
		}

		if (controller.Held(InputCode.RHAND))
		{
			gun.Fire();
			// animator.SetBool("Firing", true);
		}
		else if (controller.Held(InputCode.LHAND))
		{
			print("Erasing");
			// animator.SetBool("Firing", true);

		}
		else if (controller.Released(InputCode.LHAND) || controller.Released(InputCode.RHAND))
		{
			print("Done");
			// animator.SetBool("Firing", false);
		}
    }
}