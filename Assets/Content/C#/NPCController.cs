using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform[] waypoints;       // точки маршрута
    public float waypointThreshold = 0.5f; // дистанция для переключения на следующую
    private int currentIndex = 0;       // индекс текущей точки

    [Header("Player detection")]
    public Transform player;            // игрок
    public float triggerDistance = 2f;  // дистанция срабатывания

    [Header("Material control")]
    public Renderer[] droneRenderers;   // список рендереров (несколько мешей)
    public float dissolveSpeed = 3f;    // скорость плавного изменения
    private float currentDissolve = 0f; // текущее значение
    private float targetDissolve = 0f;  // целевое значение

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentIndex].position);
        }
    }

    void Update()
    {
        MoveBetweenPoints();
        CheckPlayerDistance();
        UpdateDissolve();
    }

    void MoveBetweenPoints()
    {
        if (waypoints.Length == 0) return;

        // если почти дошли до текущей точки → берём следующую
        if (!agent.pathPending && agent.remainingDistance <= waypointThreshold)
        {
            currentIndex = (currentIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentIndex].position);
        }
    }

    void CheckPlayerDistance()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= triggerDistance)
            targetDissolve = 1f;
        else
            targetDissolve = 0f;
    }

    void UpdateDissolve()
    {
        currentDissolve = Mathf.Lerp(currentDissolve, targetDissolve, Time.deltaTime * dissolveSpeed);

        foreach (var rend in droneRenderers)
        {
            if (rend != null)
            {
                foreach (var mat in rend.materials)
                {
                    mat.SetFloat("_DissolveStrength", currentDissolve);
                }
            }
        }
    }
}
