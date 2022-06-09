using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.XR;

public class OnCollisionActivateButton : MonoBehaviourPunCallbacks, IPunObservable
{
    public GameObject otherPlayerHead;
    private string handshake_button = "Handshake Button";
    private string handshake2_messageCanva = "Handshake 2 message";
    private string handshake2_waitingCanva = "Handshake 2 waiting";
    private string handshake2_confirmCanva = "Handshake 2 confirm";
    private string handshake1_confirmCanva = "Handshake Confirm";
    //private PhotonView photonViewParent;

    private int sceneIndex;

    private bool h2_messageActive;

    private List<InputDevice> devices = new List<InputDevice>();
    private InputDeviceCharacteristics rControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
    private InputDevice targetDevice;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(h2_messageActive);
        }
        else
        {
            this.h2_messageActive = (bool)stream.ReceiveNext();
            //Debug.Log($"{this.h2_messageActive}");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (sceneIndex == 1)
        {
            GameObject.Find(handshake_button).GetComponent<Button>().interactable = false;
        } else if (sceneIndex == 2)
        {
            InputDevices.GetDevicesWithCharacteristics(rControllerCharacteristics, devices);

            if (devices.Count > 0)
            {
                targetDevice = devices[0];
            }


            this.transform.Find(handshake1_confirmCanva).gameObject.SetActive(false);
            this.transform.Find(handshake2_waitingCanva).gameObject.SetActive(false);
            this.transform.Find(handshake2_confirmCanva).gameObject.SetActive(false);
            h2_messageActive = false;
        }
        //photonViewParent = GetComponent<PhotonView>();
    }

    void Update()
    {
        if(sceneIndex == 2)
        {
            if(h2_messageActive == false)
            {
                //Debug.Log($"messaggio è {h2_messageActive}");
                this.transform.Find(handshake2_messageCanva).gameObject.SetActive(false);
            } else
            {
                //Debug.Log($"messaggio è {h2_messageActive}");
                //Debug.Log($"oggetto è {this.transform.Find(handshake2_messageCanva).gameObject}");
                this.transform.Find(handshake2_messageCanva).gameObject.SetActive(true);
                this.transform.Find(handshake2_messageCanva).gameObject.GetComponent<Canvas>().enabled = true;
            }

            targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
            if(primaryButtonValue == true)
            {
                Debug.Log("pressing button A");
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {        
        if (collider.gameObject.name == "Head")
        {
            //Debug.Log($"{collider.transform.GetComponent<PhotonView>()}");
            PhotonView colliderPhotonView;
            colliderPhotonView = collider.transform.GetComponent<PhotonView>();
            if (!colliderPhotonView.IsMine)
            {
                otherPlayerHead = collider.gameObject;
                //Debug.Log($"indice scena è {sceneIndex}");
                if(sceneIndex == 1)
                {
                    GameObject.Find(handshake_button).GetComponent<Button>().interactable = true;
                } else if(sceneIndex == 2)
                {
                    h2_messageActive = true;
                }
                
            }
        }
        
    }

    private void OnTriggerStay(Collider collider)
    {        
        if (collider.gameObject.name == "Head")
        {
            PhotonView colliderPhotonView;
            colliderPhotonView = collider.transform.GetComponent<PhotonView>();
            if (!colliderPhotonView.IsMine)
            {
                otherPlayerHead = collider.gameObject;
                if (sceneIndex == 1)
                {
                    GameObject.Find(handshake_button).GetComponent<Button>().interactable = true;
                }
                else if (sceneIndex == 2)
                {
                    h2_messageActive = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {

        if (collider.gameObject.name == "Head")
        {
            PhotonView colliderPhotonView;
            colliderPhotonView = collider.transform.GetComponent<PhotonView>();
            if (!colliderPhotonView.IsMine)
            {
                otherPlayerHead = collider.gameObject;
                if (sceneIndex == 1)
                {
                    GameObject.Find(handshake_button).GetComponent<Button>().interactable = false;
                }
                else if (sceneIndex == 2)
                {
                    h2_messageActive = false;
                }
            }
        }
    }
}
