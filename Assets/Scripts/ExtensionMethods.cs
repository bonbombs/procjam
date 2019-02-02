using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.IO;
using UnityEngine.UI;

public static class ExtensionMethods
{

    #region Animator

    #endregion

    #region UI

    /// <summary>
    /// Causes the UI RectTransform to lerp from a starting position from an ending position over a given time.
    /// </summary>
    /// <param name="tform"></param>
    /// <param name="startpos"></param>
    /// <param name="endpos"></param>
    /// <param name="seconds"></param>
    /// <returns></returns>
    public static IEnumerator UIEaseLerp(this RectTransform tform, Vector3 startpos, Vector3 endpos, float seconds)
    {
        tform.position = startpos;
        tform.gameObject.SetActive(true);
        float t = 0.0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            tform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0.0f, 1.0f, Mathf.SmoothStep(0.0f, 1.0f, t)));
            yield return null;
        }
        tform.position = endpos;
        yield return null;
    }

    public static IEnumerator UILocalEaseLerp(this RectTransform tform, Vector3 endpos, float seconds)
    {
        Vector3 startpos = tform.localPosition;
        tform.gameObject.SetActive(true);
        float t = 0.0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            tform.localPosition = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0.0f, 1.0f, Mathf.SmoothStep(0.0f, 1.0f, t)));
            yield return null;
        }
        tform.localPosition = endpos;
        yield return null;
    }

    public static IEnumerator UISizeLerp(this RectTransform tform, Vector2 endSize, float seconds)
    {
        Vector2 startSize = tform.sizeDelta;
        tform.gameObject.SetActive(true);
        float t = 0.0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            tform.sizeDelta = Vector2.Lerp(startSize, endSize, Mathf.SmoothStep(0.0f, 1.0f, Mathf.SmoothStep(0.0f, 1.0f, t)));
            yield return null;
        }
        tform.sizeDelta = endSize;
        yield return null;
    }

    public static IEnumerator UIScaleLerp(this RectTransform tform, Vector3 endSize, float seconds)
    {
        Vector3 startSize = tform.localScale;
        tform.gameObject.SetActive(true);
        float t = 0.0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            tform.localScale = Vector3.Lerp(startSize, endSize, Mathf.SmoothStep(0.0f, 1.0f, Mathf.SmoothStep(0.0f, 1.0f, t)));
            yield return null;
        }
        tform.localScale = endSize;
        yield return null;
    }

    public static IEnumerator UIAlphaLerp(this RectTransform tform, float endAlpha, float seconds)
    {
        float startAlpha = tform.GetComponent<CanvasGroup>().alpha;
        float t = 0.0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            tform.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(startAlpha, endAlpha, Mathf.SmoothStep(0.0f, 1.0f, Mathf.SmoothStep(0.0f, 1.0f, t)));
            yield return null;
        }
        yield return null;
    }

    public static IEnumerator ColorEaseLerp(this Camera camera, Color endColor, float seconds)
    {
        Color initialColor = camera.backgroundColor;
        float t = 0.0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            camera.backgroundColor = Color.Lerp(initialColor, endColor, Mathf.SmoothStep(0.0f, 1.0f, Mathf.SmoothStep(0.0f, 1.0f, t)));
            yield return null;
        }
        camera.backgroundColor = endColor;
        yield return null;
    }

    public static IEnumerator UIAlphaEaseLerp(this CanvasGroup group, float endAlpha, float seconds)
    {

        float startAlpha = group.alpha;
        float t = 0.0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            group.alpha = Mathf.Lerp(startAlpha, endAlpha, Mathf.SmoothStep(0.0f, 1.0f, Mathf.SmoothStep(0.0f, 1.0f, t)));
            yield return null;
        }
        group.alpha = endAlpha;
        yield return null;
    }

    #endregion

    #region GameObject

    /// <summary>
    /// Causes the Transform to lerp from a starting position from an ending position over a given time.
    /// </summary>
    /// <param name="tform"></param>
    /// <param name="startpos"></param>
    /// <param name="endpos"></param>
    /// <param name="seconds"></param>
    /// <returns></returns>
    public static IEnumerator EaseLerp(this Transform tform, Vector3 startpos, Vector3 endpos, float seconds)
    {
        tform.position = startpos;
        tform.gameObject.SetActive(true);
        float t = 0.0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            tform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0.0f, 1.0f, Mathf.SmoothStep(0.0f, 1.0f, t)));
            yield return null;
        }
        tform.position = endpos;
        yield return null;
    }

    public static IEnumerator RotateEaseLerp(this Transform tform, Vector3 endAngle, float seconds)
    {
        Vector3 startAngle = tform.localEulerAngles;
        tform.gameObject.SetActive(true);
        float t = 0.0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            tform.localEulerAngles = Vector3.Lerp(startAngle, endAngle, Mathf.SmoothStep(0.0f, 1.0f, Mathf.SmoothStep(0.0f, 1.0f, t)));
            yield return null;
        }
        tform.localEulerAngles = endAngle;
        yield return null;
    }

    public static IEnumerator MaterialEaseLerp(this Material mat, Material endMat, float seconds)
    {
        Material startMat = mat;
        float t = 0.0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            mat.Lerp(startMat, endMat, Mathf.SmoothStep(0.0f, 1.0f, Mathf.SmoothStep(0.0f, 1.0f, t)));
            yield return null;
        }
        mat = endMat;
        yield return null;
    }

    /// <summary>
    /// Returns a list of all children of the given game object.
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static List<GameObject> GetAllChildren(this GameObject parent)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform t in parent.transform)
        {
            children.Add(t.gameObject);
        }
        return children;
    }

    /// <summary>
    /// Returns an array of all the GameObjects adjacent in heirachy to the one that this method is called upon. Exclludes self from array by default.
    /// </summary>
    /// <param name="obj">Object to find siblings of.</param>
    /// <returns></returns>
	public static GameObject[] GetSiblings(this GameObject obj, bool excludeSelf = true)
    {
        List<GameObject> list = new List<GameObject>();
        foreach (Transform child in obj.transform.parent)
        {
            if ((!child.gameObject.Equals(obj) && excludeSelf) || !excludeSelf)
            {
                list.Add(child.gameObject);
            }
        }
        GameObject[] siblingList = list.ToArray();
        return siblingList;
    }

    /// <summary>
    /// Returns an array of all the components of type T in the the GameObjects adjacent in heirachy to the one that this method is called upon.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T[] GetComponentsInSiblings<T>(this GameObject obj, bool excludeSelf = true)
    {

        GameObject[] siblingsList = obj.GetSiblings(excludeSelf);

        List<T> componentList = new List<T>();
        for (int i = 0; i < siblingsList.Length; i++)
        {
            if (siblingsList[i].GetComponent<T>() != null)
                componentList.Add(siblingsList[i].GetComponent<T>());
        }
        return componentList.ToArray();
    }

    /// <summary>
	/// Gets or add a component. Usage example:
	/// BoxCollider boxCollider = transform.GetOrAddComponent<BoxCollider>();
	/// </summary>
	static public T GetOrAddComponent<T>(this Component child) where T : Component
    {
        T result = child.GetComponent<T>();
        if (result == null)
        {
            result = child.gameObject.AddComponent<T>();
        }
        return result;
    }

    #endregion

    #region float

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    #endregion

    #region string

    /// <summary>
    /// Removes characters that would invalidate a file name from a string. Also replaces spaces with underscores, out of preference.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string MakeFilenameSafe(this string input)
    {
        input = input.ToLower();
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            input = input.Replace(c.ToString(), string.Empty);
        }
        input = input.Replace(' ', '_');
        return input;
    }

    #endregion
}
