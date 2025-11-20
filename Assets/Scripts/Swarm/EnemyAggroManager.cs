using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroManager : MonoBehaviour
{
    public static List<EnemyAIChase> allMinions = new List<EnemyAIChase>();

    public static void AggroAll(Transform player)
    {
        foreach (var m in allMinions)
        {
            if (m != null)
                m.SetPlayerInRange(player);
        }
    }
}
