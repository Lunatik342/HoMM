using UnityEngine;

namespace Battle.BattleField.Cells
{
    public class BattleFieldCellView: MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _ornament;
        [SerializeField] private SpriteRenderer _background;

        public void SetUnreachable()
        {
            gameObject.SetActive(false);
            _background.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }

        public void SetReachable()
        {
            gameObject.SetActive(true);
            _background.color = new Color(0, 1, 0, 0.5f);
        }

        public void SetPath()
        {
            gameObject.SetActive(true);
            _background.color = new Color(1, 0, 0, 0.5f);
        }
    }
}