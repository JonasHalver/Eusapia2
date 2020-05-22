using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScript : MonoBehaviour
{
    public Slider bar;
    public Text text;
    public Button start;
    public bool startPressed;

    void Start()
    {
        StartCoroutine("Loading");

    }

    IEnumerator Loading()
    {
        yield return null;

        AsyncOperation load1 = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        load1.allowSceneActivation = false;
        while (!load1.isDone)
        {
            bar.value = load1.progress;
            text.text = "Main Scene";
            if (load1.progress >= 0.9f)
                break;
            yield return null;
        }

        yield return null;
        AsyncOperation load2 = SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
        load2.allowSceneActivation = false;
        while (!load2.isDone)
        {
            bar.value = load2.progress;
            text.text = "Buildings";
            if (load2.progress >= 0.9f)
                break;
            yield return null;
        }

        yield return null;
        AsyncOperation load3 = SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);
        load3.allowSceneActivation = false;
        while (!load3.isDone)
        {
            bar.value = load3.progress;
            text.text = "Lights";
            if (load3.progress >= 0.9f)
                break;
            yield return null;
        }
        yield return null;

        AsyncOperation load4 = SceneManager.LoadSceneAsync(4, LoadSceneMode.Additive);
        load4.allowSceneActivation = false;
        while (!load4.isDone)
        {
            bar.value = load4.progress;
            text.text = "Underworld";
            if (load4.progress >= 0.9f)
                break;
            yield return null;
        }
        yield return null;

        AsyncOperation load5 = SceneManager.LoadSceneAsync(5, LoadSceneMode.Additive);
        load5.allowSceneActivation = false;
        while (!load5.isDone)
        {
            bar.value = load5.progress;
            text.text = "Fog";
            if (load5.progress >= 0.9f)
                break;
            yield return null;
        }
        yield return null;

        AsyncOperation load6 = SceneManager.LoadSceneAsync(6, LoadSceneMode.Additive);
        load6.allowSceneActivation = false;
        while (!load6.isDone)
        {
            bar.value = load6.progress;
            text.text = "Dead People";
            if (load6.progress >= 0.9f)
                break;
            yield return null;
        }
        yield return null;

        AsyncOperation load7 = SceneManager.LoadSceneAsync(7, LoadSceneMode.Additive);
        load7.allowSceneActivation = false;
        while (!load7.isDone)
        {
            bar.value = load7.progress;
            text.text = "Mirrors";
            if (load7.progress >= 0.9f)
                break;
            yield return null;
        }
        yield return null;

        AsyncOperation load8 = SceneManager.LoadSceneAsync(8, LoadSceneMode.Additive);
        load8.allowSceneActivation = false;
        while (!load8.isDone)
        {
            bar.value = load8.progress;
            text.text = "Pretty Objects";
            if (load8.progress >= 0.9f)
                break;
            yield return null;
        }
        yield return null;

        AsyncOperation load9 = SceneManager.LoadSceneAsync(9, LoadSceneMode.Additive);
        load9.allowSceneActivation = false;
        while (!load9.isDone)
        {
            bar.value = load9.progress;
            text.text = "Eusapia";
            if (load5.progress >= 0.9f)
                break;
            yield return null;
        }
        yield return null;

        start.gameObject.SetActive(true);

        while (!startPressed)
        {
            yield return null;
        }
        if (startPressed)
        {
            load1.allowSceneActivation = true;
            while (!load1.isDone)
                yield return null;

            Scene loadingscene = SceneManager.GetActiveScene();
            AsyncOperation unload = SceneManager.UnloadSceneAsync(loadingscene);
            yield return null;
            print(unload.isDone);

            //unload.allowSceneActivation = true;
            //load1.allowSceneActivation = true;
            load2.allowSceneActivation = true;
            load3.allowSceneActivation = true;
            load3.allowSceneActivation = true;
            load4.allowSceneActivation = true;
            load5.allowSceneActivation = true;
            load6.allowSceneActivation = true;
            load7.allowSceneActivation = true;
            load8.allowSceneActivation = true;
            load9.allowSceneActivation = true;

            Scene main = SceneManager.GetSceneAt(1);
            SceneManager.SetActiveScene(main);
        }
    }

    public void ButtonPress()
    {
        startPressed = true;
    }
}
