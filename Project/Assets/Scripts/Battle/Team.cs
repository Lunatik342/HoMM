using System;
using UnityEngine;

namespace Battle
{
    public enum Team
    {
        TeamLeft = 1,
        TeamRight = 2,
    }

    public static class TeamsHelper
    {
        public static Vector3 GetRotationDirection(this Team team)
        {
            switch (team)
            {
                case Team.TeamLeft:
                    return Vector3.left;
                case Team.TeamRight:
                    return Vector3.right;
                default:
                    throw new ArgumentOutOfRangeException(nameof(team), team, null);
            }
        }
    }
}