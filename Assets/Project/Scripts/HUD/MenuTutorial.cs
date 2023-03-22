using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gdev
{
    public class MenuTutorial : MonoBehaviour
    {
        [SerializeField] private CanvasGroup menuTutorialCanvasGroup;

        private void Start()
        {
            transform.TryGetComponent<CanvasGroup>(out menuTutorialCanvasGroup);
        }
        public void OpenInventory(bool on)
        {
            menuTutorialCanvasGroup.interactable = on;

            if (on == true)
            {
                menuTutorialCanvasGroup.gameObject.SetActive(true);
                menuTutorialCanvasGroup.DOFade(on ? 1 : 0, .15f).SetUpdate(true);
            }
            else
            {
                menuTutorialCanvasGroup.DOFade(on ? 1 : 0, .15f).SetUpdate(true).OnComplete(() =>
                {
                    menuTutorialCanvasGroup.gameObject.SetActive(false);
                });
            }
        }
    }
}
