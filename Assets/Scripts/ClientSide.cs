using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))] // сообщает Unity о том, что нам нужен компонент NetworkView. Данному компоненту NetworkStateSynchronization можно выставить Off.

public class ClientSide : MonoBehaviour
{
    public GameObject playerPrefab; // префаб игрока, который будет создан в процессе игры
    public Vector2 spawnArea = new Vector2(8.0f, 8.0f); // зона спауна

    private Vector3 RandomPosition
    { // случайная позиция в зоне спауна
        get
        {
            return transform.position +
                    transform.right * (Random.Range(0.0f, spawnArea.x) - spawnArea.x * 0.5f) +
                    transform.forward * (Random.Range(0.0f, spawnArea.y) - spawnArea.y * 0.5f);
        }
    }

    [RPC] // сообщает Unity о том, что данный метод можно вызвать из сети
    private void SpawnPlayer(string playerName)
    {
        Vector3 position = RandomPosition; // делаем случайную позицию создания персонажа
        GameObject newPlayer = Network.Instantiate(playerPrefab, position, Quaternion.LookRotation(transform.position - position, Vector3.up), 0) as GameObject; // создаем нового персонажа в сети
        newPlayer.BroadcastMessage("SetPlayerName", playerName); // задаем созданному персонажу имя (оно будет автоматически синхронизировано по сети)
    }

    void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        Network.DestroyPlayerObjects(Network.player); // удаляемся из игры
    }
}
