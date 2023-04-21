using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlLayer
{
    WORLD,
    MENU,
    POPUP
}

public enum InputCode
{
    CONFIRM,
    CANCEL,
    UP,
    DOWN,
    LEFT,
    RIGHT,
    JUMP,
	RHAND,
	LHAND,
    POWER,
    SWITCH_LEFT,
    SWITCH_RIGHT
}

public enum StateSignal
{
    ENTER,
    TICK,
    FIXED_TICK,
    EXIT
}

public enum UsableState
{
    DORMANT,
    BLOCKED,
    ACTIVE
}

public enum PaintMode
{
    NONE,
    ADD,
    ERASE
}