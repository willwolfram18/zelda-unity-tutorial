#nullable enable
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SignBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject dialogBox;

    private TMP_Text _dialogText;

    [SerializeField]
    private string signText;

    private bool _inRangeOfSign;

    public void OnInteractActionSubmitted(InputAction.CallbackContext context)
    {
        if (!_inRangeOfSign)
        {
            return;
        }

        _dialogText.text = signText;
        dialogBox.SetActive(true);
    }
    
    private void Start()
    {
        _dialogText = dialogBox.GetComponentInChildren<TMP_Text>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger entered with {0}", other.gameObject);
        if (!other.CompareTag("Player"))
        {
            Debug.Log("Collision is not a player.");
            return;
        }
        
        _inRangeOfSign = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Trigger exited with {0}", other.gameObject);
        if (!other.CompareTag("Player"))
        {
            Debug.Log("Trigger exit is not a player.");
            return;
        }

        _inRangeOfSign = false;
    }
}