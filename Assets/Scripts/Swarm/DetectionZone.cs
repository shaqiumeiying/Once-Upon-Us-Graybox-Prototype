using UnityEngine;

public class EnemyDetectionZone : MonoBehaviour
{
    private EnemyAIChase enemy;

    void Start()
    {
        enemy = GetComponentInParent<EnemyAIChase>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.SetPlayerInRange(other.transform);
            Debug.Log("Player detected by enemy.");
        }
    }

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        enemy.ClearPlayer();
    //    }
    //}
}
