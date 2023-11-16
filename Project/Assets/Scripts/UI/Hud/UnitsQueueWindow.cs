using System.Collections.Generic;
using Battle.BattleFlow;
using Battle.Units;
using DG.Tweening;
using UISystem;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Hud
{
    public class UnitsQueueWindow : UIWindow
    {
        [SerializeField] private UnitInQueueView _unitInQueueViewPrefab;
        [SerializeField] private Transform _unitsViewsParent;
        
        private UnitsQueueService _queueService;
        
        private readonly List<UnitInQueueView> _unitsSlots = new();

        public void PassParameters(UnitsQueueService queueService)
        {
            _queueService = queueService;
        }

        public override void OnShow()
        {
            var unitsList = _queueService.CurrentTurnQueue;
            unitsList.UnitAdded += SpawnUnitView;
            unitsList.UnitRemoved += RemoveUnitInQueueView;

            for (int i = 0; i < unitsList.SourceList.Count; i++)
            {
                var unit = unitsList.SourceList[i];
                SpawnUnitView(unit, i);
            }
        }

        private void SpawnUnitView(Unit unit, int index)
        {
            var slot = Instantiate(_unitInQueueViewPrefab, _unitsViewsParent);
            slot.transform.SetSiblingIndex(index);
            _unitsSlots.Insert(index, slot);
            slot.SetUnit(unit);
        }

        private void RemoveUnitInQueueView(Unit unit, int index)
        {
            var targetSlot = _unitsSlots[index];
            _unitsSlots.RemoveAt(index);
            AnimateItemDeletion(targetSlot);
        }

        private void AnimateItemDeletion(UnitInQueueView targetView)
        {
            //TODO Terrible for perfomance but looks nice, fine for the demo
            DOTween.Sequence()
                .Append(targetView.transform.DOScale(0, 0.4f).SetEase(Ease.InOutSine))
                .OnUpdate(() => { LayoutRebuilder.ForceRebuildLayoutImmediate(_unitsViewsParent.GetComponent<RectTransform>()); })
                .AppendCallback(() => Destroy(targetView.gameObject));
        }

        public override void OnHide()
        {
            var unitsList = _queueService.CurrentTurnQueue;
            unitsList.UnitAdded -= SpawnUnitView;
            unitsList.UnitRemoved -= RemoveUnitInQueueView;
        }
    }
}
