#define DEBUG
//#undef DEBUG

using UnityEngine;

public class Controlling : MonoBehaviour
{

#if DEBUG
    const string MOVE_FORWARD_MESSAGE   =                   "Avancer";
    const string MOVE_BACKWARD_MESSAGE  =                   "Reculer";
    const string MOVE_RIGHT_MESSAGE     =              "Droite crabe";
    const string MOVE_LEFT_MESSAGE      =              "Gauche crabe";
    const string MOVE_UP_MESSAGE        =                    "Monter";
    const string MOVE_DOWN_MESSAGE      =                 "Descendre";
    const string LEAN_LEFT_MESSAGE      =        "Se penche à gauche";
    const string LEAN_RIGHT_MESSAGE     =         "Se penche à doite";
    const string LEAN_FORWARD_MESSAGE   =      "Se penche en arrière";
    const string LEAN_BACKWARD_MESSAGE  =        "Se penche en avant";
    const string TURN_RIGHT_MESSAGE     =     "Tourne vers la droite";
    const string TURN_LEFT_MESSAGE      =     "Tourne vers la gauche";
    const string STOP_MESSAGE           =          "Position de base";
#else
    const string MOVE_FORWARD_MESSAGE   = "[768;512;512;512;512;512]";
    const string MOVE_BACKWARD_MESSAGE  = "[256;512;512;512;512;512]";
    const string MOVE_RIGHT_MESSAGE     = "[512;768;512;512;512;512]";
    const string MOVE_LEFT_MESSAGE      = "[512;256;512;512;512;512]";
    const string MOVE_UP_MESSAGE        = "[512;512;768;512;512;512]";
    const string MOVE_DOWN_MESSAGE      = "[512;512;256;512;512;512]";
    const string LEAN_LEFT_MESSAGE      = "[512;512;512;768;512;512]";
    const string LEAN_RIGHT_MESSAGE     = "[512;512;512;256;512;512]";
    const string LEAN_FORWARD_MESSAGE   = "[512;512;512;512;768;512]";
    const string LEAN_BACKWARD_MESSAGE  = "[512;512;512;512;256;512]";
    const string TURN_RIGHT_MESSAGE     = "[512;512;512;512;512;768]";
    const string TURN_LEFT_MESSAGE      = "[512;512;512;512;512;256]";
    const string STOP_MESSAGE           = "[512;512;512;512;512;512]";
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
                UDPNetworking.log += "Action not defined\n";
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
            case "Stop":        // Stop
                UDPNetworking.Socket_SendMessage(STOP_MESSAGE);
                break;
            case "Start":       // Se connecter au serveur
                UDPNetworking.Connection();
                break;
            case "ShutDown":    // Se déconnecter du serveur
                UDPNetworking.Disconnection();
                break;
            default:            // Aucun bouton correspondant : on n'envoi rien
                UDPNetworking.log += "Button not recognized\n";
                break;
        }
    }
}
