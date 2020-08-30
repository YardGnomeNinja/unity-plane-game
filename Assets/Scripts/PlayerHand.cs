using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHand : MonoBehaviour
{
    TMP_Text objectNameOverlay;

    // Start is called before the first frame update
    void Start()
    {
        var parent = transform.parent.gameObject;
        objectNameOverlay = parent.GetComponentInChildren<TMP_Text>();
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("interactable")) {
            objectNameOverlay.SetText(other.name);
        }
    }

    private void OnTriggerExit(Collider other) {
        objectNameOverlay.SetText("");
    }

    // Update is called once per frame
    void Update()
    {
    }
}
