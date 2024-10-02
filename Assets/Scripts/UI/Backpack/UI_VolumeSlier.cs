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

        // ��_value��slider.valueĬ��ֵ��ȵ�ʱ���ڲ���ֱ�ӷ��ز�����ûص�����
        if (_value == 0.001f)
            SliderValue(_value);
    }
}
