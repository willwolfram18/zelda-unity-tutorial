using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
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
    
    private CinemachineConfiner _confiner;

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

        PolygonCollider2D newRoom = _confiner.m_BoundingShape2D.gameObject == roomA.gameObject ? roomB : roomA;
        _confiner.m_ConfineScreenEdges = false;
        float verticalMod = newRoom.transform.position.y > _confiner.m_BoundingShape2D.transform.position.y ? 1 : -1;
        other.transform.position += new Vector3(playerChange.x, verticalMod * playerChange.y, 0);
        
        yield return new WaitForEndOfFrame();

        _confiner.m_BoundingShape2D = newRoom;
        _confiner.m_ConfineScreenEdges = true;
    }
}
