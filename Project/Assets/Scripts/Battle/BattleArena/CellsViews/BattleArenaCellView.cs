using System;
using UnityEngine;
using Zenject;

namespace Battle.BattleArena.CellsViews
{
    public class BattleArenaCellView: MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _ornament;
        [SerializeField] private SpriteRenderer _background;

        private SelectionState _prevState;

        public void SetUnreachable()
        {
            _prevState = SelectionState.Unreachable;
            gameObject.SetActive(false);
            _background.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }

        public void SetReachable()
        {
            _prevState = SelectionState.Reachable;
            gameObject.SetActive(true);
            _background.color = new Color(0, 1, 0, 0.5f);
        }

        public void SetObstacle()
        {
            _prevState = SelectionState.Obstacle;
            gameObject.SetActive(true);
            _background.color = new Color(0, 0, 1, 0.5f);
        }

        public void SetPath()
        {
            _prevState = SelectionState.Path;
            gameObject.SetActive(true);
            _background.color = new Color(1, 0, 0, 0.5f);
        }

        public void SetHover()
        {
            gameObject.SetActive(true);
            _background.color = new Color(1, 0.6f, 0, 0.5f);
        }

        public void RestorePrevState()
        {
            switch (_prevState)
            {
                case SelectionState.Unreachable:
                    SetUnreachable();
                    break;
                case SelectionState.Reachable:
                    SetReachable();
                    break;
                case SelectionState.Obstacle:
                    SetObstacle();
                    break;
                case SelectionState.Path:
                    SetPath();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public class Factory: PlaceholderFactory<BattleArenaCellView>
        {
            
        }
    }

    public enum SelectionState
    {
        Unreachable,
        Reachable,
        Obstacle,
        Path
    }
}