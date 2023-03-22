using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Gdev
{
    public class RestartManager : MonoBehaviour
    {
        [Header("Respawn Settings")]
        [SerializeField] private CanvasGroup restartCanvasGroup;
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_Text counterText;

        [Header("Scene Settings")]
        [SerializeField] private string sceneName;
        [SerializeField] private float timeBeforeRestart;
        [SerializeField] private CanvasGroup sceneCanvasGroup;
        [SerializeField] private Slider sliderScene;
        [SerializeField] private TMP_Text counterSceneText;
        private float timer = 0;
        public bool startRestart;

        private void Start()
        {
            slider.maxValue = timeBeforeRestart;
            slider.value = timeBeforeRestart;
            startRestart = false;
            restartCanvasGroup.DOFade(0, .15f).SetUpdate(true).OnComplete(() =>
            {
                restartCanvasGroup.gameObject.SetActive(false);
            });
            timer = timeBeforeRestart;
        }
        void Update()
        {
            if (startRestart)
            {
                timer -= 1 * Time.deltaTime;
                counterText.text = $"Respawn in {timer.ToString("0")}...";
                slider.value = timer;
                if (timer <= 0)
                {
                    LoadScene(this.sceneName);
                    startRestart = false;
                }
            }
        }

        public async void LoadScene(string sceneName)
        {
            sliderScene.maxValue = 1;
            sliderScene.value = 0;
            var scene = SceneManager.LoadSceneAsync(sceneName);
            scene.allowSceneActivation = false;

            sceneCanvasGroup.gameObject.SetActive(true);
            sceneCanvasGroup.DOFade(1, .15f).SetUpdate(true);
            do
            {
                await Task.Delay(100);
                float progress = Mathf.Clamp01(scene.progress / .9f);
                print(progress);
                counterSceneText.text = $"{progress * 100f}%";
                sliderScene.value = Mathf.Clamp01(progress);
            } while (scene.progress <= .1f);

            await Task.Delay(1000);

            scene.allowSceneActivation = true;
        }
        public void StartRespawn()
        {
            restartCanvasGroup.interactable = false;
            restartCanvasGroup.gameObject.SetActive(true);
            restartCanvasGroup.DOFade(1, .15f).SetUpdate(true);
            startRestart = true;
        }
    }
}
