using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class StartMenuButton : MonoBehaviour
{
    public string SceneName;
    public Slider progressSlider;

    public Sprite NormalSprite;
    public Sprite HighlightSprite;

    private AsyncOperation async;

    public bool IsHighlighted { get; set; }

    // Use this for initialization

    void Update()
    {
        if(Input.GetButtonDown("Submit"))
        {
            Debug.Log("Submit button pressed.");
        }

        if(this.IsHighlighted && Input.GetButtonDown("Submit"))
        {
            SelectAsync();
        }

        if(this.IsHighlighted)
        {
            GetComponent<SpriteRenderer>().sprite = HighlightSprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = NormalSprite;
        }
    }

    public void SelectAsync()
    {
        if(string.IsNullOrEmpty(this.SceneName))
        {
            Debug.LogWarning("Scene Name is null for menu button object.");
            return;
        }

        if(this.SceneName == "Exit")
        {
            Application.Quit();
        }

        progressSlider.enabled = true;
        StartCoroutine(LoadLevelWithProgress());
    }

    private IEnumerator LoadLevelWithProgress()
    {
        async = SceneManager.LoadSceneAsync(this.SceneName);
        while(!async.isDone)
        {
            this.progressSlider.value = async.progress;
            yield return null;
        }
    }
}
