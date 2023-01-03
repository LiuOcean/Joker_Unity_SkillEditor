using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ܲ�����
/// </summary>
public class Skill_Player : MonoBehaviour
{
    private Animation_Controller animation_Controller;

    private bool isPlaying = false;     //��ǰ�Ƿ��ڲ���״̬
    public bool IsPlaying { get => isPlaying; }


    private SkillConfig skillConfig;    //��ǰ���ŵļ�������
    private int currentFrameIndex;      //��ǰ�ǵڼ�֡
    private float playTotalTime;        //��ǰ���ŵ���ʱ��
    private int frameRate;              //��ǰ���ܵ�֡��

    public void Init(Animation_Controller animation_Controller)
    {
        this.animation_Controller = animation_Controller;
    }

    private Action<Vector3, Quaternion> rootMotionAction;
    private Action skillEndAction;

    /// <summary>
    /// ���ż���
    /// </summary>
    /// <param name="skillConfig"> �������� </param>
    public void PlaySkill(SkillConfig skillConfig, Action skillEndAction, Action<Vector3, Quaternion> rootMotionAction = null)
    {
        this.skillConfig = skillConfig;
        this.skillEndAction = skillEndAction;
        this.rootMotionAction = rootMotionAction;

        currentFrameIndex = -1;
        frameRate = skillConfig.FrameRate;
        playTotalTime = 0;
        isPlaying = true;

        TickSkill();
    }

    private void Update()
    {
        if (isPlaying)
        {
            playTotalTime += Time.deltaTime;
            //������ʱ���жϵ�ǰ�ǵڼ�֡
            int targetFrameIndex = (int)(playTotalTime * frameRate);
            //��ֹһ֡�ӳٹ���׷֡
            while (currentFrameIndex < targetFrameIndex)
            {
                //����һ�μ���
                TickSkill();
            }

            //����ﵽ���һ֡�����ܽ���
            if (targetFrameIndex >= skillConfig.FrameCount)
            {
                isPlaying = false;
                skillConfig = null;
                if (rootMotionAction != null) animation_Controller.ClearRootMotionAction();
                rootMotionAction = null;
                skillEndAction?.Invoke();
            }
        }
    }

    private void TickSkill()
    {
        currentFrameIndex += 1;
        //��������
        if (animation_Controller != null && skillConfig.SkillAnimationData.FrameDataDic.TryGetValue(currentFrameIndex, out SkillAnimationEvent skillAnimationEvent))
        {
            animation_Controller.PlaySingleAniamtion(skillAnimationEvent.AnimationClip, 1, true, skillAnimationEvent.TransitionTime);

            if (skillAnimationEvent.ApplyRootMotion)
            {
                animation_Controller.SetRootMotionAction(rootMotionAction);
            }
            else
            {
                animation_Controller.ClearRootMotionAction();

            }

        }

    }

}
