using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    //Code responsible for the spawn of a new player in the room

    private GameObject spawnedPlayerPrefab;
    private int flagH1 = 0;
    private int flagH2 = 0;
    private int flagH3 = 0;
    private int flagH4 = 0;

    private int sceneIndex;

    //Also when a player is spawned, it activates the mayor animation
    private GameObject mayor;
    private GameObject mayor_head;
    private GameObject mayor_left;
    private GameObject mayor_hand_holder;
    private GameObject mayor_right;

    private Animator animator_mayor_head;
    private Animator animator_mayor_left;
    private Animator animator_mayor_right;

    //When a player joins a room it is spawned and a different name is assigned to him depending on the room and on how
    //many players are there
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
            StartCoroutine(waitForMayor());
        } else if(sceneIndex == 2)
        {
            //Debug.Log("It's scene 2");
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player H2", transform.position, transform.rotation);
            spawnedPlayerPrefab.name = $"Network Player H2 {+flagH2}";
            flagH2++;
        } else if (sceneIndex == 3)
        {
            //Debug.Log("It's scene 3");
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player H3", transform.position, transform.rotation);
            spawnedPlayerPrefab.name = $"Network Player H3 {+flagH3}";
            flagH3++;
        } else if (sceneIndex == 4)
        {
            //Debug.Log("It's scene 4");
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player H4", transform.position, transform.rotation);
            spawnedPlayerPrefab.name = $"Network Player H4 {+flagH4}";
            flagH4++;
        }
    }

    //When a player leaves a room it is destroyed 
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
        } else if (sceneIndex == 3)
        {
            PhotonNetwork.Destroy(spawnedPlayerPrefab);
            flagH3--;
        } else if (sceneIndex == 4)
        {
            PhotonNetwork.Destroy(spawnedPlayerPrefab);
            flagH4--;
        }
    }

    IEnumerator waitForMayor()
    {
        //Find mayor
        mayor = GameObject.Find("Mayor");
        mayor_head = mayor.transform.GetChild(0).gameObject;
        mayor_left = mayor.transform.GetChild(1).gameObject;
        mayor_hand_holder = mayor.transform.GetChild(2).gameObject;
        mayor_right = mayor_hand_holder.transform.GetChild(0).gameObject;
        animator_mayor_head = mayor_head.GetComponent<Animator>();
        animator_mayor_left = mayor_left.GetComponent<Animator>();
        animator_mayor_right = mayor_right.GetComponent<Animator>();

        //Wait for 2 seconds
        yield return new WaitForSeconds(1);

        //Start mayor's voice and animation
        mayor_head.GetComponent<AudioSource>().Play();
        animator_mayor_head.Play("MayorSpeech1_head", 0, 0);
        animator_mayor_left.Play("MayorSpeech1_left", 0, 0);
        animator_mayor_right.Play("MayorSpeech1_right", 0, 0);
    }
}
