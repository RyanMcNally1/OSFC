using System.Collections;
using UnityEngine;

public class Gate : Interactable
{
    [Header("Movement")]
    public Transform gate;
    public float openHeight = 5f;
    public float openSpeed = 3f;

    private bool isOpen;
    private Vector3 closedPosition;
    private Vector3 openPosition;

    void Start()
    {
        if (gate == null)
            gate = transform;

        closedPosition = gate.position;
        openPosition = closedPosition + Vector3.up * openHeight;
    }

    public bool requiresBossDefeat = false;

    public override void Interact()
    {
        if (isOpen)
            return;

        if (requiresBossDefeat && !EnemyAI.BossDefeated)
        {
            Debug.Log("It's locked...");
            return;
        }

        isOpen = true;
        StartCoroutine(OpenGate());
    }

    IEnumerator OpenGate()
    {
        while (Vector3.Distance(gate.position, openPosition) > 0.01f)
        {
            gate.position = Vector3.MoveTowards(
                gate.position,
                openPosition,
                openSpeed * Time.deltaTime
            );

            yield return null;
        }

        gate.position = openPosition;
    }
}
