using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using Object = System.Object;

public class RoomTransferBehavior : MonoBehaviour
{
    [SerializeField]
    private Vector2 playerChange;

    [SerializeField]
    private PolygonCollider2D roomA;
    
    [SerializeField]
    private PolygonCollider2D roomB;

    [SerializeField]
    private TMP_Text _roomNameText;
    
    private CinemachineConfiner _confiner;

    [CanBeNull]
    private Coroutine _roomNameDisplay;

    private void Start()
    {
        _confiner = GameObject.FindWithTag("VirtualMainCamera").GetComponent<CinemachineConfiner>();
    }

    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            yield return null;
        }

        if (_roomNameDisplay != null)
        {
            StopCoroutine(_roomNameDisplay);
            _roomNameText.gameObject.SetActive(false);
        }
        
        PolygonCollider2D newRoom = _confiner.m_BoundingShape2D.gameObject == roomA.gameObject ? roomB : roomA;
        _confiner.m_ConfineScreenEdges = false;
        if (newRoom.CompareTag("NamedRoom"))
        {
            _roomNameDisplay = StartCoroutine(DisplayAndFadeRoomName(newRoom));
        }
        
        float verticalMod = newRoom.transform.position.y > _confiner.m_BoundingShape2D.transform.position.y ? 1 : -1;
        other.transform.position += new Vector3(playerChange.x, verticalMod * playerChange.y, 0);
        
        yield return new WaitForEndOfFrame();

        _confiner.m_BoundingShape2D = newRoom;
        _confiner.m_ConfineScreenEdges = true;
    }

    private IEnumerator DisplayAndFadeRoomName(PolygonCollider2D newRoom)
    {
        _roomNameText.text = newRoom.name;
        _roomNameText.alpha = 1;
        _roomNameText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1);

        const float alphaChangeInterval = 0.15f;
        const float totalTimeChange = 0.15f * 10;
        float timePassed = 0;
        while (_roomNameText.alpha > 0)
        {
            _roomNameText.alpha = Mathf.Lerp(_roomNameText.alpha, 0, timePassed / totalTimeChange);
            yield return new WaitForSeconds(alphaChangeInterval);
            timePassed += alphaChangeInterval;
        }
    }
}
