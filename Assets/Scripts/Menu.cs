﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public GameObject character;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("LoadMenu");
    }

    IEnumerator LoadMenu()
    {
        yield return null;

        AsyncOperation load1 = SceneManager.LoadSceneAsync(11, LoadSceneMode.Additive);
        load1.allowSceneActivation = false;
        while (!load1.isDone)
        {

            if (load1.progress >= 0.9f)
                break;
            yield return null;
        }

        yield return null;
        AsyncOperation load2 = SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
        load2.allowSceneActivation = false;
        while (!load2.isDone)
        {

            if (load2.progress >= 0.9f)
                break;
            yield return null;
        }

        yield return null;
        AsyncOperation load3 = SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);
        load3.allowSceneActivation = false;
        while (!load3.isDone)
        {

            if (load3.progress >= 0.9f)
                break;
            yield return null;
        }
        yield return null;

        AsyncOperation load4 = SceneManager.LoadSceneAsync(4, LoadSceneMode.Additive);
        load4.allowSceneActivation = false;
        while (!load4.isDone)
        {

            if (load4.progress >= 0.9f)
                break;
            yield return null;
        }
        yield return null;

        AsyncOperation load5 = SceneManager.LoadSceneAsync(5, LoadSceneMode.Additive);
        load5.allowSceneActivation = false;
        while (!load5.isDone)
        {

            if (load5.progress >= 0.9f)
                break;
            yield return null;
        }
        yield return null;

        AsyncOperation load6 = SceneManager.LoadSceneAsync(6, LoadSceneMode.Additive);
        load6.allowSceneActivation = false;
        while (!load6.isDone)
        {

            if (load6.progress >= 0.9f)
                break;
            yield return null;
        }
        yield return null;

        AsyncOperation load7 = SceneManager.LoadSceneAsync(7, LoadSceneMode.Additive);
        load7.allowSceneActivation = false;
        while (!load7.isDone)
        {
            if (load7.progress >= 0.9f)
                break;
            yield return null;
        }
        yield return null;

        AsyncOperation load8 = SceneManager.LoadSceneAsync(8, LoadSceneMode.Additive);
        load8.allowSceneActivation = false;
        while (!load8.isDone)
        {

            if (load8.progress >= 0.9f)
                break;
            yield return null;
        }
        yield return null;

        AsyncOperation load9 = SceneManager.LoadSceneAsync(9, LoadSceneMode.Additive);
        load9.allowSceneActivation = false;
        while (!load9.isDone)
        {   
            if (load5.progress >= 0.9f)
                break;
            yield return null;
        }
        yield return null;

        load1.allowSceneActivation = true;
        load2.allowSceneActivation = true;
        load3.allowSceneActivation = true;
        load3.allowSceneActivation = true;
        load4.allowSceneActivation = true;
        load5.allowSceneActivation = true;
        load6.allowSceneActivation = true;
        load7.allowSceneActivation = true;
        load8.allowSceneActivation = true;
        load9.allowSceneActivation = true;

        GetComponent<Animator>().SetTrigger("Loaded");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        GetComponent<Animator>().SetTrigger("Play");
        character.GetComponent<Animator>().SetTrigger("Play");
    }

    public void NextScene()
    {
        SceneManager.LoadScene(10);
    }
}
