using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
	[SerializeField]
	MainMenu main_menu_prefab;
	[SerializeField]
	PaintGunMenu gun_menu_prefab;

    Controller controller;
	PaintGun gun;

	MainMenu main_menu;
	PaintGunMenu gun_menu;

	void Awake()
    {
        controller = GetComponent<Controller>();
		gun = GetComponentInChildren<PaintGun>();

		main_menu = Instantiate(main_menu_prefab);
		gun_menu = Instantiate(gun_menu_prefab);
		gun_menu.gameObject.SetActive(false);
	}

    // Update is called once per frame
    void Update()
    {
		if(controller.Pressed(InputCode.SWITCH_LEFT))
		{
			gun.ShiftColour(-1);
		}
		if (controller.Pressed(InputCode.SWITCH_RIGHT))
		{
			gun.ShiftColour(1);
		}

		if (controller.Held(InputCode.RHAND))
		{
			gun.SetMode(PaintMode.ADD);
		}
		else if (controller.Held(InputCode.LHAND))
		{
			gun.SetMode(PaintMode.ERASE);
		}
		else // if (controller.Released(InputCode.LHAND) || controller.Released(InputCode.RHAND))
		{
			gun.SetMode(PaintMode.NONE);
		}

		if(controller.Pressed(InputCode.CANCEL))
		{
			main_menu.gameObject.SetActive(true);
		}
		else if (controller.Held(InputCode.POWER))
		{
			gun_menu.gameObject.SetActive(true);
		}
    }
}
