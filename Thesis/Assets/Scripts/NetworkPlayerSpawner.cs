using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    private GameObject spawnedPlayerPrefab;
    private int flagH1 = 0;
    private int flagH2 = 0;

    private int sceneIndex;

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if(sceneIndex == 1)
        {
            //Debug.Log("It's scene 1");
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player H1", transform.position, transform.rotation);
            spawnedPlayerPrefab.name = $"Network Player H1 {+flagH1}";
            flagH1++;
        } else if(sceneIndex == 2)
        {
            //Debug.Log("It's scene 2");
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player H2", transform.position, transform.rotation);
            spawnedPlayerPrefab.name = $"Network Player H2 {+flagH2}";
            flagH2++;
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if(sceneIndex == 1)
        {
            PhotonNetwork.Destroy(spawnedPlayerPrefab);
            flagH1--;
        } else if(sceneIndex == 2)
        {
            PhotonNetwork.Destroy(spawnedPlayerPrefab);
            flagH2--;
        }        
    }
}
