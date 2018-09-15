using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public static class InputManager {

    static GamePadState[] state = new GamePadState[2];
    static GamePadState[] prevState = new GamePadState[2];

    public static void UpdateControllerStates() {
        for (var i = 0; i < 2; i++) {
            prevState[i] = state[i];
            state[i] = GamePad.GetState((PlayerIndex)i);
        }
    }

    public static bool InteractInput(int player) {
        return Input.GetKeyDown(KeyCode.E) && player == 0 || prevState[player].Buttons.X == ButtonState.Released && state[player].Buttons.X == ButtonState.Pressed;
    }

    public static bool SecondaryInteractInput(int player) {
        return Input.GetKeyDown(KeyCode.R) && player == 0 || prevState[player].Buttons.Y == ButtonState.Released && state[player].Buttons.Y == ButtonState.Pressed;
    }

    public static bool PickUpInput(int player) {
        return Input.GetMouseButtonDown(1) && player == 0 || prevState[player].Triggers.Left <= .7f && state[player].Triggers.Left > .7f;
    }

    public static bool Cancel(int player) {
        return Input.GetMouseButtonDown(1) && player == 0 || prevState[player].Buttons.B == ButtonState.Released && state[player].Buttons.B == ButtonState.Pressed;
    }

    public static bool JumpInput(int player) {
        return Input.GetKeyDown(KeyCode.Space) && player == 0 || prevState[player].Buttons.A == ButtonState.Released && state[player].Buttons.A == ButtonState.Pressed;
    }

    public static bool RotatePlacement(int player) {
        return Input.GetKey(KeyCode.R) && player == 0 || state[player].Buttons.Y == ButtonState.Pressed;
    }

    public static bool AutoSellWhilePlacing(int player) {
        return Input.GetKey(KeyCode.LeftShift) && player == 0 || state[player].Triggers.Left > .5f;
    }

    public static bool PlaceAsset(int player) {
        return Input.GetMouseButtonDown(0) && player == 0 || prevState[player].Triggers.Right <= .5f && state[player].Triggers.Right > .5f;
    }

    public static float CycleHotbar(int player) {
        if (Input.mouseScrollDelta.y != 0 && player == 0) return Input.mouseScrollDelta.y;
        if (prevState[player].Buttons.RightShoulder == ButtonState.Released && state[player].Buttons.RightShoulder == ButtonState.Pressed) return 1;
        if (prevState[player].Buttons.LeftShoulder == ButtonState.Released && state[player].Buttons.LeftShoulder == ButtonState.Pressed) return -1;
        return 0;
    }

    public static float Scroll(int player) {
        if (Input.mouseScrollDelta.y != 0 && player == 0) return Input.mouseScrollDelta.y;
        return state[player].ThumbSticks.Right.Y;
    }

    public static bool BuildModeInput(int player) {
        return Input.GetKeyDown(KeyCode.B) && player == 0 || prevState[player].Buttons.Back == ButtonState.Released && state[player].Buttons.Back == ButtonState.Pressed;
    }

    public static Vector2 LookInput(int player) {
        if (player == 0) {
            return new Vector2(Input.GetAxis("Mouse X") + state[player].ThumbSticks.Right.X, Input.GetAxis("Mouse Y") + state[player].ThumbSticks.Right.Y);
        }
        return new Vector2(state[player].ThumbSticks.Right.X, state[player].ThumbSticks.Right.Y);
    }

    public static Vector2 MovementInput(int player) {
        if (player == 0) {
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        return new Vector2(state[player].ThumbSticks.Left.X, state[player].ThumbSticks.Left.Y);
    }


}
