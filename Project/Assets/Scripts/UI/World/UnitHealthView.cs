using Battle.Units;
using UI.Generic;
using UnityEngine;
using Zenject;

public class UnitHealthView : MonoBehaviour
{
    [SerializeField] private AnimatedNumberChanger _unitsCountText;
    [SerializeField] private ProgressBar _heathBar;

    private UnitHealth _unitHealth;
    
    [Inject]
    public void Construct(UnitHealth unitHealth)
    {
        _unitHealth = unitHealth;
    }

    public void Initialize()
    {
        _unitHealth.HealthChanged += DisplayNewHealth;
        _unitHealth.UnitDied += HideUI;
        
        _unitsCountText.SetValue(_unitHealth.AliveUnitsCount);
        _heathBar.SetFillAmount(_unitHealth.GetHealthPercentage());
    }

    private void DisplayNewHealth(int currentHealth, int unitsCount)
    {
        _unitsCountText.SetValueAnimated(unitsCount);
        _heathBar.SetFillAmount(_unitHealth.GetHealthPercentage());
    }

    private void HideUI()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _unitHealth.HealthChanged -= DisplayNewHealth;
        _unitHealth.UnitDied -= HideUI;
    }
}
