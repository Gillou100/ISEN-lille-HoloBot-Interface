#define DEBUG
//#undef DEBUG

using UnityEngine;


/*
 * Here is the script which assemble the messages which will be send.
 */

public class Controlling : MonoBehaviour
{

#if DEBUG
    private const string MOVE_FORWARD_MESSAGE   =                   "Avancer";
    private const string MOVE_BACKWARD_MESSAGE  =                   "Reculer";
    private const string MOVE_RIGHT_MESSAGE     =              "Droite crabe";
    private const string MOVE_LEFT_MESSAGE      =              "Gauche crabe";
    private const string MOVE_UP_MESSAGE        =                    "Monter";
    private const string MOVE_DOWN_MESSAGE      =                 "Descendre";
    private const string LEAN_LEFT_MESSAGE      =        "Se penche à gauche";
    private const string LEAN_RIGHT_MESSAGE     =         "Se penche à doite";
    private const string LEAN_FORWARD_MESSAGE   =      "Se penche en arrière";
    private const string LEAN_BACKWARD_MESSAGE  =        "Se penche en avant";
    private const string TURN_RIGHT_MESSAGE     =     "Tourne vers la droite";
    private const string TURN_LEFT_MESSAGE      =     "Tourne vers la gauche";
    private const string STOP_MESSAGE           =          "Position de base";
    private const string BUTTON_A               =                  "Bouton A";
#else
    private const string MOVE_FORWARD_MESSAGE   = "[768;512;512;512;512;512]";
    private const string MOVE_BACKWARD_MESSAGE  = "[256;512;512;512;512;512]";
    private const string MOVE_RIGHT_MESSAGE     = "[512;768;512;512;512;512]";
    private const string MOVE_LEFT_MESSAGE      = "[512;256;512;512;512;512]";
    private const string MOVE_UP_MESSAGE        = "[512;512;768;512;512;512]";
    private const string MOVE_DOWN_MESSAGE      = "[512;512;256;512;512;512]";
    private const string LEAN_LEFT_MESSAGE      = "[512;512;512;768;512;512]";
    private const string LEAN_RIGHT_MESSAGE     = "[512;512;512;256;512;512]";
    private const string LEAN_FORWARD_MESSAGE   = "[512;512;512;512;768;512]";
    private const string LEAN_BACKWARD_MESSAGE  = "[512;512;512;512;256;512]";
    private const string TURN_RIGHT_MESSAGE     = "[512;512;512;512;512;768]";
    private const string TURN_LEFT_MESSAGE      = "[512;512;512;512;512;256]";
    private const string STOP_MESSAGE           = "[512;512;512;512;512;512]";
    private const string BUTTON_A               =                      "Demo";
#endif

    public void SendButtonPress(string button)
    {
        UDPNetworking.log += "Button Pressed: " + button + "\n";

        switch (button)
        {
            case "Forward":
                UDPNetworking.Socket_SendMessage(MOVE_FORWARD_MESSAGE);
                break;
            case "Backward":
                UDPNetworking.Socket_SendMessage(MOVE_BACKWARD_MESSAGE);
                break;
            case "Move Right":
                UDPNetworking.Socket_SendMessage(MOVE_RIGHT_MESSAGE);
                break;
            case "Move Left":
                UDPNetworking.Socket_SendMessage(MOVE_LEFT_MESSAGE);
                break;
            case "Up":
                UDPNetworking.Socket_SendMessage(MOVE_UP_MESSAGE);
                break;
            case "Down":
                UDPNetworking.Socket_SendMessage(MOVE_DOWN_MESSAGE);
                break;
            case "Lean Left":
                UDPNetworking.Socket_SendMessage(LEAN_LEFT_MESSAGE);
                break;
            case "Lean Right":
                UDPNetworking.Socket_SendMessage(LEAN_RIGHT_MESSAGE);
                break;
            case "Lean Forward":
                UDPNetworking.Socket_SendMessage(LEAN_FORWARD_MESSAGE);
                break;
            case "Lean Backward":
                UDPNetworking.Socket_SendMessage(LEAN_BACKWARD_MESSAGE);
                break;
            case "Turn Right":
                UDPNetworking.Socket_SendMessage(TURN_RIGHT_MESSAGE);
                break;
            case "Turn Left":
                UDPNetworking.Socket_SendMessage(TURN_LEFT_MESSAGE);
                break;
            case "A":
                UDPNetworking.Socket_SendMessage(BUTTON_A);
                break;
            case "B":
                UDPNetworking.log += "Action not defined\n";
                break;
            case "X":
                UDPNetworking.log += "Action not defined\n";
                break;
            case "Y":
                UDPNetworking.log += "Action not defined\n";
                break;
            case "Stop":
                UDPNetworking.Socket_SendMessage(STOP_MESSAGE);
                break;
            case "Connection":
                UDPNetworking.Connection();
                break;
            case "Disconnection":
                UDPNetworking.Disconnection();
                break;
            default:            // Aucun bouton correspondant : on n'envoi rien
                UDPNetworking.log += "Button not recognized\n";
                break;
        }
    }
}
