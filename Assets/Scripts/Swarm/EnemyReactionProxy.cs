using UnityEngine;

public class EnemyReactionProxy : MonoBehaviour
{
    private EnemyReaction reaction;

    void Start()
    {
        // Get the EnemyReaction from child
        reaction = GetComponentInChildren<EnemyReaction>();
    }

    public void React(Vector3 hitDir)
    {
        if (reaction != null)
            reaction.ReactToHit(hitDir);
    }
}
