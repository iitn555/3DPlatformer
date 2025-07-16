using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CooldownManager
{
    // Start is called before the first frame update
    public List<Skill> List_Skill = new List<Skill>();
    private Dictionary<string, Skill> Dictionary_Skill = new Dictionary<string, Skill>();

    public void RegisterSkill(string _name, float _cooldown)
    {
        if (!Dictionary_Skill.ContainsKey(_name))
        {
            Skill newskill = new Skill(_name, _cooldown);
            Dictionary_Skill.Add(_name, newskill);
        }
    }

    public bool CheckCooldownSkill(string name) // 쿨다운 체크후 사용
    {

        if (Dictionary_Skill.TryGetValue(name, out Skill skill))
        {
            if (skill.TryUse())
            {
                //Debug.Log($"{name} 사용됨");
                return true;
            }
            else
            {
                //Debug.Log($"{name} 쿨타임: {skill.GetRemainingCooldown():F1}s 남음");
            }
        }
        else
        {
            Debug.Log($"{name} 을 찾을 수 없음!");

        }

        return false;
    }

    public float GetCooldown(string name)
    {
        return Dictionary_Skill.ContainsKey(name) ? Dictionary_Skill[name].GetRemainingCooldown() : 0f;
    }

    public void ResetCooldownTime(string name, float _time)
    {
        if (Dictionary_Skill.TryGetValue(name, out Skill skill))
            skill.SetCoolDownTime(_time);
        else
            Debug.Log($"{name} 찾을 수 없음!");
    }
}

public class Skill
{
    public string SkillName;
    public float fCoolDownTime = 0;
    private float lastUsedTime = -Mathf.Infinity;

    public bool IsReady => Time.time >= lastUsedTime + fCoolDownTime;

    public Skill(string _name, float _cooldown)
    {
        if (string.IsNullOrWhiteSpace(_name))
            Debug.Log("스킬 이름은 비어 있을 수 없습니다.");


        if (_cooldown <= 0f)
            Debug.Log("쿨타임은 0보다 커야 합니다.");

        SkillName = _name;
        fCoolDownTime = _cooldown;
    }


    public bool TryUse()
    {
        if (IsReady)
        {
            lastUsedTime = Time.time;
            return true;
        }
        return false;
    }

    public float GetRemainingCooldown()
    {
        float remaining = (lastUsedTime + fCoolDownTime) - Time.time;
        return Mathf.Max(0f, remaining);
    }

    public void SetCoolDownTime(float _time)
    {
        fCoolDownTime = _time;
    }
}