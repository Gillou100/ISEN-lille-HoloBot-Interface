using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class CursorManager : MonoBehaviour
{
    GestureRecognizer recognizer;

    // Use this for initialization
    void Start()
    {   
        // Set up a GestureRecognizer to detect Select gestures.
        recognizer = new GestureRecognizer();
        recognizer.RecognitionStarted += (args) => {gameObject.GetComponent<Animator>().SetTrigger("tap");};
        recognizer.RecognitionEnded += (args) => {gameObject.GetComponent<Animator>().SetTrigger("endtap");};
        recognizer.StartCapturingGestures();
        
    }
}
