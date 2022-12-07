using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintGunMenu : MonoBehaviour
{
    Controller controller;
    RadialLayout layout;
    PaintGun gun;

    private void Awake()
    {
        controller = GetComponent<Controller>();
        layout = GetComponent<RadialLayout>();
        gun = FindObjectOfType<PaintGun>();
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        Vector2 m_pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 s_cen = new Vector2(Screen.width, Screen.height) * 0.5f;
        Vector2 m_diff = m_pos - s_cen;

        gun.SetRadius(layout.SelectIndex(m_diff));

        if(Input.GetKeyUp(KeyCode.Mouse2))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            gameObject.SetActive(false);
        }
    }
}
