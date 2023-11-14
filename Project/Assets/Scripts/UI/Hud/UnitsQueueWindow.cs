using System.Collections.Generic;
using Battle.BattleFlow;
using Battle.Units;
using DG.Tweening;
using UISystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud
{
    public class UnitsQueueWindow : UIWindow
    {
        [SerializeField] private UnitsQueueSlot _slotPrefab;
        [SerializeField] private Transform _slotsParent;
        
        private UnitsQueueService _queueService;
        private List<UnitsQueueSlot> _unitsSlots = new List<UnitsQueueSlot>();

        public void PassParameters(UnitsQueueService queueService)
        {
            _queueService = queueService;
        }

        public override void OnShow()
        {
            var unitsList = _queueService.CurrentTurnQueue;
            unitsList.UnitAdded += UnitsListOnUnitAdded;
            unitsList.UnitRemoved += UnitsListOnUnitRemoved;

            for (int i = 0; i < unitsList.SourceList.Count; i++)
            {
                var unit = unitsList.SourceList[i];
                UnitsListOnUnitAdded(unit, i);
            }
        }

        private void UnitsListOnUnitAdded(Unit unit, int index)
        {
            var slot = Instantiate(_slotPrefab, _slotsParent);
            slot.transform.SetSiblingIndex(index);
            _unitsSlots.Insert(index, slot);
            slot.SetUnit(unit);
        }

        private void UnitsListOnUnitRemoved(Unit unit, int index)
        {
            var targetSlot = _unitsSlots[index];
            _unitsSlots.RemoveAt(index);

            DOTween.Sequence()
                .Append(targetSlot.transform.DOScale(0, 0.5f).SetEase(Ease.InOutSine))
                .OnUpdate(() =>
                {
                    //Terrible for perfomance but looks nice, fine for now
                    LayoutRebuilder.ForceRebuildLayoutImmediate(_slotsParent.GetComponent<RectTransform>());
                })
                .AppendCallback(() => Destroy(targetSlot.gameObject));
        }

        public override void OnHide()
        {
            var unitsList = _queueService.CurrentTurnQueue;
            unitsList.UnitAdded -= UnitsListOnUnitAdded;
            unitsList.UnitRemoved -= UnitsListOnUnitRemoved;
        }
    }
}
