using UnityEngine;


/*
 * Here is the script which enable/disable turn and move left and right buttons.
 */

public class ToggleActive : MonoBehaviour
{
    public void toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
