using Battle;
using Battle.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud
{
    public class UnitInQueueView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _unitsCount;
        [SerializeField] private TextMeshProUGUI _unitName;
        [SerializeField] private Image _teamImage;
    
        public void SetUnit(Unit unit)
        {
            _unitsCount.text = unit.Health.AliveUnitsCount.ToString();
            _unitName.text = unit.ToString();
            _teamImage.color = unit.Team == Team.TeamLeft ? Color.green : Color.red;
        }
    }
}
