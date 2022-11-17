using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class NetworkHandshakeRespond : MonoBehaviour
{
    //Code responsible for the start of the handshake animation in H1 and H2

    private GameObject rightHand;
    private GameObject rightController;
    private GameObject networkPlayer;
    private GameObject head;
    private Vector3 cameraPosition;

    private GameObject netFakeHandHolder;
    private GameObject netFakeHand;

    private int sceneIndex;

    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        rightHand = this.gameObject;
        rightController = rightHand.transform.parent.gameObject;
        networkPlayer = rightController.transform.parent.gameObject;
        netFakeHandHolder = networkPlayer.transform.GetChild(3).gameObject;
        netFakeHand = netFakeHandHolder.transform.GetChild(0).gameObject;
        head = networkPlayer.transform.GetChild(0).gameObject;
    }

    //Function that saves the camera and hand position (so that the animation starts in the right position) and start
    //the coroutine for the animation
    public void OnHandshakePressed(Vector3 camPosition)
    {
        cameraPosition = camPosition;
        StartCoroutine(Wait());
    }

    //Called at the end of the animation to set back the right object's parent and components
    public void SetBackComp()
    {
        StartCoroutine(Wait2());
    }

    //Coroutine used to start the animation
    public IEnumerator Wait()
    {
        double time = 0.25;
        yield return new WaitForSeconds((float)time);

        rightHand.SetActive(false);
        netFakeHand.SetActive(true);

        netFakeHand.GetComponent<NetworkHandshakeFakeHand>().DoHandshake(head.transform.position, cameraPosition);        
    }

    public IEnumerator Wait2()
    {
        float time = (float)0.75;
        yield return new WaitForSeconds(time);

        netFakeHandHolder.transform.DOMove(rightController.transform.position, time);
        netFakeHandHolder.transform.DORotateQuaternion(rightController.transform.rotation, time);

        yield return new WaitForSeconds(time);

        rightHand.SetActive(true);
        netFakeHand.SetActive(false);

        if(sceneIndex == 2)
        {
            head.gameObject.transform.GetComponent<OnButtonAPressed>().animationGoing = false;
        }        
    }
}
