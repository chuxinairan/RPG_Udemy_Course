using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_VolumeSlier : MonoBehaviour
{
    public Slider slider;
    public string parameter;
    Button b;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float multiplier;

    public void SliderValue(float _value)
    {
        audioMixer.SetFloat(parameter, Mathf.Log10(_value) * multiplier);
    }

    public void LoadSlider(float _value)
    {
        if(_value >= 0.001)
            slider.value = _value;

        // 在_value和slider.value默认值相等的时候内部会直接返回不会调用回调函数
        if (_value == 0.001f)
            SliderValue(_value);
    }
}
