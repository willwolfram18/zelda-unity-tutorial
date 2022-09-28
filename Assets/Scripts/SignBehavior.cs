#nullable enable
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class SignBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject dialogBox;

    private TMP_Text _dialogText;

    [SerializeField]
    [TextArea(2, 4)]
    private string signText = null!;

    private bool _inRangeOfSign;

    public void OnInteractActionSubmitted(InputAction.CallbackContext context)
    {
        if (dialogBox.activeInHierarchy)
        {
            DeactivateDialogBox();
            return;
        }
        
        if (!_inRangeOfSign)
        {
            return;
        }

        ActivateDialogBox(signText);
    }
    
    private void Start()
    {
        Guard.EditorNotNull(dialogBox, nameof(dialogBox));
        Guard.EditorNotNullOrWhiteSpace(signText, nameof(signText));
        _dialogText = dialogBox.GetRequiredComponentInChildren<TMP_Text>(nameof(dialogBox));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger entered with " + other.gameObject);
        if (!other.CompareTag("Player"))
        {
            Debug.Log("Collision is not a player.");
            return;
        }
        
        _inRangeOfSign = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Trigger exited with " + other.gameObject);
        if (!other.CompareTag("Player"))
        {
            Debug.Log("Trigger exit is not a player.");
            return;
        }

        _inRangeOfSign = false;
        DeactivateDialogBox();
    }

    private void DeactivateDialogBox() => dialogBox.SetActive(false);

    private void ActivateDialogBox(string text)
    {
        _dialogText.SetText(text);
        dialogBox.SetActive(true);
    }
}