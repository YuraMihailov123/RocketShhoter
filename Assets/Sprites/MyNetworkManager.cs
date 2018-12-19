using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager
{

    public class NetworkMessage : MessageBase
    {
        public float[] coord = new float[2];
    }

    public static string tempNick;

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        NetworkMessage clientData = extraMessageReader.ReadMessage<NetworkMessage>();
        
        GameObject player = Instantiate(playerPrefab, GetStartPosition().position, Quaternion.identity) as GameObject;
        //player.GetComponent<DataPlayer>().playerNick = clientData.nick;
        //player.GetComponent<DataPlayer>().playerId = conn.connectionId;

        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        NetworkMessage clientData = new NetworkMessage();

        //clientData.coord[1]= tempNick;

        ClientScene.AddPlayer(conn, 0, clientData);
    }
}