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
                //Left team looks to the right, right team look to the left
                case Team.TeamLeft:
                    return Vector3.right;
                case Team.TeamRight:
                    return Vector3.left;
                default:
                    throw new ArgumentOutOfRangeException(nameof(team), team, null);
            }
        }
        
        public static Team GetOppositeTeam(this Team team)
        {
            return team == Team.TeamLeft ? Team.TeamRight : Team.TeamLeft;
        }
    }
}