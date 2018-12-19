using UnityEngine;

public class ToggleActive : MonoBehaviour {
    public void toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
