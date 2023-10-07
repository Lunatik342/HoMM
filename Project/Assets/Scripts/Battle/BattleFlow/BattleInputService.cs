using UnityEngine;

namespace Battle.BattleFlow
{
    public class BattleInputService
    {
        public bool IsMousePressed()
        {
            return Input.GetMouseButtonDown(0);
        }
    }
}