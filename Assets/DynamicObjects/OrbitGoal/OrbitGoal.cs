using UnityEngine;

public class OrbitGoal : MonoBehaviour
{
    private OrbitGoalAdd[] m_adds;
    private OrbitGoalSub[] m_subs;

    private void Awake()
    {
        m_adds = GetComponentsInChildren<OrbitGoalAdd>();
        m_subs = GetComponentsInChildren<OrbitGoalSub>();
    }

    public bool PlayerInGoal(Player player)
    {
        bool playerInAdd = false;
        foreach (OrbitGoalAdd add in m_adds)
        {
            if (add.PlayerInAdd(player))
            {
                playerInAdd = true;
            }
        }

        if (!playerInAdd)
            return false;

        foreach (OrbitGoalSub sub in m_subs)
        {
            if (sub.PlayerInSub(player))
                return false;
        }

        return true;
    }
}
