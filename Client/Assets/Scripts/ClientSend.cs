using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region Packets
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            // _packet.Write(UIManager.instance.usernameField.text);
            _packet.Write(Client.instance.defaultNames[Client.instance.myId]);

            SendTCPData(_packet);
        }
    }

    public static void PlayerMovement(bool[] _inputs, float _rotation)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_inputs.Length);
            foreach (bool _input in _inputs)
            {
                _packet.Write(_input);
            }
            _packet.Write(_rotation);

            SendUDPData(_packet);
        }
    }
    
    public static void Shoot()
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerShoot))
        {
            SendUDPData(_packet);
        }
    }
    #endregion
}
